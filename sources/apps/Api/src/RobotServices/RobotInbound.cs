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
    private readonly ILogger _logger;
    private readonly MqttProducer _mqttProducer;
    private readonly TravelingSalesmanAlgorithmProvider _algorithmProvider;
    private readonly RobotState _robotState;
    private readonly InMemoryOrdersRepository _historicalOrdersRepository;

    public RobotInbound(
        ILoggerFactory loggerFactory, 
        MqttProducer mqttProducer, 
        TravelingSalesmanAlgorithmProvider algorithmProvider,
        RobotState robotState,
        InMemoryOrdersRepository historicalOrdersRepository)
    {
        _logger = loggerFactory.CreateLoggerApi();
        _mqttProducer = mqttProducer;
        _algorithmProvider = algorithmProvider;
        _robotState = robotState;
        _historicalOrdersRepository = historicalOrdersRepository;
    }

    public async Task SendCommands(RobotCommandDto commands)
    {
        _logger.LogInformation("Processing message from server with new robot commands");

        string message = Serializer.Serialize(commands);
        await _mqttProducer.PublishAsync(MqttTopics.RobotCommand, message);
    }

    public async Task StartPicking(OrderDto orderDto)
    {
        _logger.LogInformation("Processing message from server with new products to pick");

        List<Position> robotStops = PrepareRobotStops(orderDto.OrderedProducts);
        List<Position> path = _algorithmProvider.GetAlgorithm(orderDto.TspAlgorithm).FindPath(robotStops);

        DirectionEnum startDirection = _robotState.Direction;
        List<RobotMoveEnum> moves = GenerateCommands(path, startDirection);

        RobotCommandDto commands = new RobotCommandDto() { Commands = moves };
        string message = Serializer.Serialize(commands);

        Order order = new Order()
        {
            OrderId = Guid.NewGuid(),
            OrderedProducts = orderDto.OrderedProducts,
            TspAlgorithm = orderDto.TspAlgorithm,
            Timestamp = orderDto.Timestamp,
            StartPickingTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
        };

        _historicalOrdersRepository.Add(order);
        _robotState.StartPicking(order);
        
        await _mqttProducer.PublishAsync(MqttTopics.RobotCommand, message);
    }

    public async Task SendStop()
    {
        _logger.LogInformation("Processing message from server to stop the robot");
        await _mqttProducer.PublishAsync(MqttTopics.RobotStop, string.Empty);
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

    private List<RobotMoveEnum> GenerateCommands(List<Position> positions, DirectionEnum startDirection)
    {
        _logger.LogDebug("Generating robot moves from positions");

        List<RobotMoveEnum> moves = new List<RobotMoveEnum>();

        DirectionEnum oldDirection = startDirection;

        for (int i = 0; i < positions.Count - 1; i++)
        {
            int x_prev = positions[i].X;
            int y_prev = positions[i].Y;

            int x_next = positions[i+1].X;
            int y_next = positions[i+1].Y;

            DirectionEnum newDirection = RobotOperation.FindNewDirection(x_prev, y_prev, x_next, y_next);
            RobotMoveEnum newMove = RobotOperation.GenerateMove(oldDirection, newDirection);

            moves.Add(newMove);

            oldDirection = newDirection;
        }

        _logger.LogDebug($"Generated moves: { string.Join(" -> ", moves) }");
        return moves;
    }
}