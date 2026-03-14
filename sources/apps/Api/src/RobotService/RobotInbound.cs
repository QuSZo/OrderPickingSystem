using System.Text.Json;
using System.Text.Json.Serialization;
using Api.Logging;
using Api.Mqtt;
using Api.Products;
using Api.RobotOperations;
using Api.TravelingSalesmanAlgorithms;

namespace Api.RobotService;

public class RobotInbound
{
    private readonly ILogger _logger;
    private readonly MqttProducer _mqttProducer;
    private readonly TravelingSalesmanAlgorithmProvider _algorithmProvider;
    private readonly RobotOperation _robotOperation;

    public RobotInbound(
        ILoggerFactory loggerFactory, 
        MqttProducer mqttProducer, 
        TravelingSalesmanAlgorithmProvider algorithmProvider,
        RobotOperation robotOperation)
    {
        _logger = loggerFactory.CreateLoggerApi();
        _mqttProducer = mqttProducer;
        _algorithmProvider = algorithmProvider;
        _robotOperation = robotOperation;
    }

    public async Task SendCommands(string message)
    {
        _logger.LogInformation("Processing message from server with new robot commands");
        await _mqttProducer.PublishAsync(MqttTopics.RobotCommand, message);
    }

    public async Task StartPicking(List<Position> positions)
    {
        _logger.LogInformation("Processing message from server with new products to pick");

        List<Position> path = _algorithmProvider.GetAlgorithm().FindPath(positions);
        List<RobotMoveEnum> moves = _robotOperation.GenerateMoves(path, DirectionEnum.South);

        JsonSerializerOptions options = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
        };

        string message = JsonSerializer.Serialize(moves, options);

        await _mqttProducer.PublishAsync(MqttTopics.RobotCommand, message);
    }

    public async Task SendStop()
    {
        _logger.LogInformation("Processing message from server to stop the robot");
        await _mqttProducer.PublishAsync(MqttTopics.RobotStop, string.Empty);
    }
}