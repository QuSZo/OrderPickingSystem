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

        if (robotStatusDto.Event == RobotEventEnum.Movement && robotStatusDto.Command != null)
        {
            if (robotStatusDto.Command.Move == RobotMoveEnum.Stop)
            {
                if (_robotState.OrderDto != null)
                {
                    _robotState.ProductBeingPicked = robotStatusDto.Command.OrderedProduct;
                    _robotState.CurrentPosition = new Position() { X = _robotState.NextPosition.X, Y = _robotState.NextPosition.Y };
                }
            }
            else
            {
                if (_robotState.NextPosition != null)
                {
                    _robotState.CurrentPosition = new Position() { X = _robotState.NextPosition.X, Y = _robotState.NextPosition.Y };
                }

                _robotState.NextPosition = RobotOperation.CalculatePosition(_robotState.CurrentPosition, _robotState.Direction, robotStatusDto.Command.Move);
                _robotState.Direction = RobotOperation.FindNewDirection(
                    _robotState.CurrentPosition.X, 
                    _robotState.CurrentPosition.Y, 
                    _robotState.NextPosition.X, 
                    _robotState.NextPosition.Y);

                if (_robotState.OrderDto != null && _robotState.ProductBeingPicked != null)
                {
                    using (IServiceScope? scope = _serviceProvider.CreateScope())
                    {
                        IOrdersRepository ordersRepository = scope.ServiceProvider.GetRequiredService<IOrdersRepository>();

                        Order order = await ordersRepository.GetByIdAsync(_robotState.OrderDto.OrderId);
                        order.PickedProducts.Add(_robotState.ProductBeingPicked);
                        await ordersRepository.Update(order);

                        _robotState.OrderDto = order.ToDto();
                    }

                    _robotState.ProductBeingPicked = null;
                }
            }
        }

        else if (robotStatusDto.Event == RobotEventEnum.Finished)
        {
            _robotState.CurrentPosition = new Position() { X = _robotState.NextPosition.X, Y = _robotState.NextPosition.Y };

            if (_robotState.OrderDto != null)
            {
                using (IServiceScope? scope = _serviceProvider.CreateScope())
                {
                    IOrdersRepository ordersRepository = scope.ServiceProvider.GetRequiredService<IOrdersRepository>();

                    Order order = await ordersRepository.GetByIdAsync(_robotState.OrderDto.OrderId);
                    order.FinishPickingTime = robotStatusDto.Timestamp;

                    if (robotStatusDto.RobotPIDSummary != null)
                    {
                        double proportionalAbsoluteMean = robotStatusDto.RobotPIDSummary.ProportionalHistory.Average(x => Math.Abs(x));
                        double derivativeAbsoluteMean = robotStatusDto.RobotPIDSummary.DerivativeHistory.Average(x => Math.Abs(x));
                        double integralAbsoluteMean = robotStatusDto.RobotPIDSummary.IntegralHistory.Average(x => Math.Abs(x));
                        double powerDifferenceAbsoluteMean = robotStatusDto.RobotPIDSummary.PowerDifferenceHistory.Average(x => Math.Abs(x));

                        order.ProportionalHistory = robotStatusDto.RobotPIDSummary.ProportionalHistory;
                        order.DerivativeHistory = robotStatusDto.RobotPIDSummary.DerivativeHistory;
                        order.IntegralHistory = robotStatusDto.RobotPIDSummary.IntegralHistory;
                        order.PowerDifferenceHistory = robotStatusDto.RobotPIDSummary.PowerDifferenceHistory;
                        order.ProportionalAbsoluteMean = proportionalAbsoluteMean;
                        order.DerivativeAbsoluteMean = derivativeAbsoluteMean;
                        order.IntegralAbsoluteMean = integralAbsoluteMean;
                        order.PowerDifferenceAbsoluteMean = powerDifferenceAbsoluteMean;
                    }

                    await ordersRepository.Update(order);

                    _robotState.OrderDto = order.ToDto();
                }
            }
        }

        else if (robotStatusDto.Event == RobotEventEnum.Started)
        {
            if (_robotState.OrderDto != null)
            {
                using (IServiceScope? scope = _serviceProvider.CreateScope())
                {
                    IOrdersRepository ordersRepository = scope.ServiceProvider.GetRequiredService<IOrdersRepository>();

                    Order order = await ordersRepository.GetByIdAsync(_robotState.OrderDto.OrderId);
                    order.StartPickingTime = robotStatusDto.Timestamp;
                    await ordersRepository.Update(order);

                    _robotState.OrderDto = order.ToDto();
                }
            }
        }

        if (_robotState.CurrentPosition != _robotState.VisitedPositions.LastOrDefault())
        {
            _robotState.VisitedPositions.Add(new Position() { X = _robotState.CurrentPosition.X, Y = _robotState.CurrentPosition.Y });
        }
        
        _robotState.Command = robotStatusDto.Command;
        _robotState.Event = robotStatusDto.Event;
        _robotState.Timestamp = robotStatusDto.Timestamp;

        string robotStateMessage = Serializer.Serialize(_robotState);
        await _robotStateHubService.SendMessageAsync(robotStateMessage);

        if (robotStatusDto.Event == RobotEventEnum.Finished || robotStatusDto.Event == RobotEventEnum.Stopped)
        {   
            _robotState.Reset();
        }
    }
}