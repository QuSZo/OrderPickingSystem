using Api.Dtos;
using Api.Logging;
using Api.Mqtt;
using Api.Orders;
using Api.RobotOperations;
using Api.TravelingSalesmanAlgorithms;
using Api.Utils;

namespace Api.RobotServices;

public class RobotInbound
{
    private const int StopDurationMsForOne = 1000;
    private readonly ILogger _logger;
    private readonly MqttProducer _mqttProducer;
    private readonly TravelingSalesmanAlgorithmProvider _algorithmProvider;
    private readonly RobotState _robotState;
    private readonly IOrdersRepository _historicalOrdersRepository;

    public RobotInbound(
        ILoggerFactory loggerFactory, 
        MqttProducer mqttProducer, 
        TravelingSalesmanAlgorithmProvider algorithmProvider,
        RobotState robotState,
        IOrdersRepository historicalOrdersRepository)
    {
        _logger = loggerFactory.CreateLoggerApi();
        _mqttProducer = mqttProducer;
        _algorithmProvider = algorithmProvider;
        _robotState = robotState;
        _historicalOrdersRepository = historicalOrdersRepository;
    }

    public async Task SendRawCommands(RobotCommandDto commands)
    {
        _logger.LogInformation("Processing message from server with new robot commands");

        await SendCommands(commands);
    }

    public async Task StartPicking(OrderDto orderDto)
    {
        _logger.LogInformation("Processing message from server with new products to pick");

        if (_robotState.Event != null)
        {
            _logger.LogError("You cannot send commands if the robot is already processing some");
            throw new InvalidOperationException("You cannot send commands if the robot is already processing some");
        }

        List<Position> robotStops = PrepareRobotStops(orderDto.OrderedProducts);
        TspAlgorithmResult result = _algorithmProvider.GetAlgorithm(orderDto.TspAlgorithm).FindPath(robotStops);

        DirectionEnum startDirection = _robotState.Direction;
        List<RobotCommand> moves = GenerateCommands(result.Path, startDirection, orderDto.OrderedProducts);

        RobotCommandDto commands = new RobotCommandDto() { Commands = moves };

        Order order = new Order()
        {
            OrderId = Guid.NewGuid(),
            OrderedProducts = orderDto.OrderedProducts,
            PickedProducts = new List<OrderedProduct>(),
            TspAlgorithm = orderDto.TspAlgorithm,
            Timestamp = orderDto.Timestamp,
            Distance = result.TotalWeight,
            StartPickingTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
        };

        _historicalOrdersRepository.Add(order);
        _robotState.StartPicking(order);
        
        await SendCommands(commands);
    }

    public async Task SendStop()
    {
        _logger.LogInformation("Processing message from server to stop the robot");
        await _mqttProducer.PublishAsync(MqttTopics.RobotStop, string.Empty);
    }

    private async Task SendCommands(RobotCommandDto commands)
    {
        if (_robotState.Event != null)
        {
            _logger.LogError("You cannot send commands if the robot is already processing some");
            throw new InvalidOperationException("You cannot send commands if the robot is already processing some");
        }

        string message = Serializer.Serialize(commands);
        await _mqttProducer.PublishAsync(MqttTopics.RobotCommand, message);
    }

    private List<Position> PrepareRobotStops(List<OrderedProduct> orderedProducts)
    {
        Position startPosition = _robotState.Position;
        Position finishPosition = startPosition;

        List<Position> positionsToSee = new List<Position>();

        List<Position> productPositions = orderedProducts.Select(product => product.Position).ToList();
        positionsToSee.Add(startPosition);
        positionsToSee.AddRange(productPositions);
        positionsToSee.Add(finishPosition);

        return positionsToSee;
    }

    private List<RobotCommand> GenerateCommands(List<Position> positions, DirectionEnum startDirection, List<OrderedProduct> orderedProducts)
    {
        _logger.LogInformation("Generating robot moves from positions");

        List<RobotCommand> moves = new List<RobotCommand>();
        List<OrderedProduct> productsToPick = new List<OrderedProduct>(orderedProducts);

        DirectionEnum oldDirection = startDirection;

        for (int i = 0; i < positions.Count - 1; i++)
        {
            int x_prev = positions[i].X;
            int y_prev = positions[i].Y;

            int x_next = positions[i+1].X;
            int y_next = positions[i+1].Y;

            OrderedProduct? orderedProductOnPosition = productsToPick.SingleOrDefault(orderedProduct => orderedProduct.Position == positions[i]);
            if (orderedProductOnPosition != null)
            {
                RobotCommand stopCommand = new RobotCommand() { Move = RobotMoveEnum.Stop, StopDurationMs = orderedProductOnPosition.Quantity * StopDurationMsForOne, OrderedProduct = orderedProductOnPosition };
                moves.Add(stopCommand);
                productsToPick.Remove(orderedProductOnPosition);
            }

            DirectionEnum newDirection = RobotOperation.FindNewDirection(x_prev, y_prev, x_next, y_next);
            RobotMoveEnum newMove = RobotOperation.GenerateMove(oldDirection, newDirection);

            RobotCommand command = new RobotCommand() { Move = newMove};
            moves.Add(command);

            oldDirection = newDirection;
        }

        _logger.LogInformation($"Generated moves: { string.Join(" -> ", moves) }");
        return moves;
    }
}