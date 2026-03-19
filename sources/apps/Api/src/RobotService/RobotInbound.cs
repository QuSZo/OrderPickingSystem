using System.Text.Json;
using Api.Dtos;
using Api.Logging;
using Api.Mqtt;
using Api.RobotOperations;
using Api.TravelingSalesmanAlgorithms;
using Api.Utils;

namespace Api.RobotService;

public class RobotInbound
{
    private readonly ILogger _logger;
    private readonly MqttProducer _mqttProducer;
    private readonly TravelingSalesmanAlgorithmProvider _algorithmProvider;
    private readonly RobotState _robotState;

    public RobotInbound(
        ILoggerFactory loggerFactory, 
        MqttProducer mqttProducer, 
        TravelingSalesmanAlgorithmProvider algorithmProvider,
        RobotState robotState)
    {
        _logger = loggerFactory.CreateLoggerApi();
        _mqttProducer = mqttProducer;
        _algorithmProvider = algorithmProvider;
        _robotState = robotState;
    }

    public async Task SendCommands(List<RobotMoveEnum> moves)
    {
        _logger.LogInformation("Processing message from server with new robot commands");

        RobotCommandDto listRobotCommandDto = new RobotCommandDto(moves);

        string message = JsonSerializer.Serialize(listRobotCommandDto);
        await _mqttProducer.PublishAsync(MqttTopics.RobotCommand, message);
    }

    public async Task StartPicking(List<OrderedProductDto> products)
    {
        _logger.LogInformation("Processing message from server with new products to pick");

        DirectionEnum startDirection = _robotState.Direction;
        Position startPosition = _robotState.Position;
        Position finishPosition = startPosition;

        List<Position> positionsToSee = new List<Position>();

        List<Position> productPositions = products.Select(product => product.Position).ToList();
        positionsToSee.Add(startPosition);
        positionsToSee.AddRange(productPositions);
        positionsToSee.Add(finishPosition);

        List<Position> path = _algorithmProvider.GetAlgorithm().FindPath(positionsToSee);
        List<RobotMoveEnum> moves = GenerateCommands(path, startDirection);

        RobotCommandDto commands = new RobotCommandDto(moves);

        string message = Serializer.Serialize(commands);
        await _mqttProducer.PublishAsync(MqttTopics.RobotCommand, message);
    }

    public async Task SendStop()
    {
        _logger.LogInformation("Processing message from server to stop the robot");
        await _mqttProducer.PublishAsync(MqttTopics.RobotStop, string.Empty);
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