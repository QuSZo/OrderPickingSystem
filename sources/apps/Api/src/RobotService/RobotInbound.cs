using Api.Logging;
using Api.Mqtt;

namespace Api.RobotService;

public class RobotInbound
{
    private readonly ILogger _logger;
    private readonly MqttProducer _mqttProducer;

    public RobotInbound(ILoggerFactory loggerFactory, MqttProducer mqttProducer)
    {
        _logger = loggerFactory.CreateLoggerApi();
        _mqttProducer = mqttProducer;
    }

    public async Task HandleCommand(string message)
    {
        _logger.LogInformation("Processing message from server with new robot commands");
        await _mqttProducer.PublishAsync(MqttTopics.RobotCommand, message);
    }

    public async Task HandleStop()
    {
        _logger.LogInformation("Processing message from server to stop the robot");
        await _mqttProducer.PublishAsync(MqttTopics.RobotStop, string.Empty);
    }
}