using Api.Dtos;
using Api.Logging;
using Api.Mqtt;
using Api.Products;
using Api.RobotOperations;
using Api.Utils;
using Api.WebSockets;

namespace Api.RobotService;

public class RobotOutbound : IHostedService
{
    private readonly ILogger _logger;
    private readonly MqttConsumer _mqttConsumer;
    private readonly RobotStateHubService _robotStateHubService;
    private readonly RobotState _robotState;
    public RobotOutbound(
        ILoggerFactory loggerFactory, 
        MqttConsumer mqttConsumer, 
        RobotStateHubService robotStateHubService,
        RobotState robotState)
    {
        _logger = loggerFactory.CreateLoggerApi();
        _mqttConsumer = mqttConsumer;
        _robotStateHubService = robotStateHubService;
        _robotState = robotState;

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

        _robotState.Update(newPosition, newDirection, robotStatusDto.Command, robotStatusDto.Event, robotStatusDto.Timestamp);

        string robotStateMessage = Serializer.Serialize(_robotState);
        await _robotStateHubService.SendMessageAsync(robotStateMessage);

        if (robotStatusDto.Command == null)
        {
            _robotState.Reset();
        }
    }
}