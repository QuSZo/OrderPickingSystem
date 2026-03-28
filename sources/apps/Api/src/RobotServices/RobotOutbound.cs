using Api.Dtos;
using Api.Logging;
using Api.Mqtt;
using Api.Orders;
using Api.RobotOperations;
using Api.Utils;
using Api.WebSockets;

namespace Api.RobotServices;

public class RobotOutbound : IHostedService
{
    private readonly ILogger _logger;
    private readonly MqttConsumer _mqttConsumer;
    private readonly RobotStateHubService _robotStateHubService;
    private readonly RobotState _robotState;
    private readonly IServiceProvider _serviceProvider;
    
    public RobotOutbound(
        ILoggerFactory loggerFactory, 
        MqttConsumer mqttConsumer, 
        RobotStateHubService robotStateHubService,
        RobotState robotState,
        IServiceProvider serviceProvider)
    {
        _logger = loggerFactory.CreateLoggerApi();
        _mqttConsumer = mqttConsumer;
        _robotStateHubService = robotStateHubService;
        _robotState = robotState;
        _serviceProvider = serviceProvider;

        _mqttConsumer.ReceivedMessage += ProcessMessage;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _mqttConsumer.SubscribeAsync(MqttTopics.RobotStatus);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _mqttConsumer.ReceivedMessage -= ProcessMessage;

        await _mqttConsumer.UnsubscribeAsync();
    }

    private async Task ProcessMessage(string message)
    {
        _logger.LogInformation("Processing message from robot with new state");

        ReceivedRobotStatusDto? robotStatusDto = Serializer.Deserialize<ReceivedRobotStatusDto>(message);
        if (robotStatusDto == null)
        {
            _logger.LogWarning("Failed to deserialize robot status message");
            return;
        }

        Position newPosition = _robotState.Position; 
        DirectionEnum newDirection = _robotState.Direction; 
        if (robotStatusDto.Command != null)
        {
            newPosition = RobotOperation.CalculatePosition(_robotState.Position, _robotState.Direction, robotStatusDto.Command);
            newDirection = RobotOperation.FindNewDirection(_robotState.Position.X, _robotState.Position.Y, newPosition.X, newPosition.Y);
        }
        if (robotStatusDto.Event == RobotEventEnum.Finished)
        {
            if (_robotState.Order != null)
            {
                using (IServiceScope? scope = _serviceProvider.CreateScope())
                {
                    IOrdersRepository ordersRepository = scope.ServiceProvider.GetRequiredService<IOrdersRepository>();
                    Order order = ordersRepository.SetFinishPickingTime(_robotState.Order.OrderId);
                    _robotState.Order = order;
                }
            }
        }

        _robotState.Update(newPosition, newDirection, robotStatusDto.Command, robotStatusDto.Event, robotStatusDto.Timestamp);

        string robotStateMessage = Serializer.Serialize(_robotState);
        await _robotStateHubService.SendMessageAsync(robotStateMessage);

        if (robotStatusDto.Event == RobotEventEnum.Finished || robotStatusDto.Event == RobotEventEnum.Stopped)
        {   
            _robotState.Reset();
        }
    }
}