using Api.Mqtt;

namespace Api.RobotService;

public class RobotInbound
{
    private readonly MqttProducer _mqttProducer;

    public RobotInbound(MqttProducer mqttProducer)
    {
        _mqttProducer = mqttProducer;
    }

    public async Task HandleCommand(string message)
    {
        await _mqttProducer.PublishAsync(MqttTopics.RobotCommand, message);
    }
}