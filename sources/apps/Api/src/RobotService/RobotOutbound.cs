using Api.Mqtt;
using Api.WebSockets;

namespace Api.RobotService;

public class RobotOutbound
{
    private readonly MqttConsumer _mqttConsumer;
    private readonly RobotStateHubService _robotStateHubService;

    public RobotOutbound(MqttConsumer mqttConsumer, RobotStateHubService robotStateHubService)
    {
        _mqttConsumer = mqttConsumer;
        _robotStateHubService = robotStateHubService;
    }

    public async Task HandleNewState(string message)
    {
        await _robotStateHubService.SendMessageAsync(message);
    }
}