using Api.Mqtt;
using Api.WebSockets;

namespace Api.RobotService;

public class RobotInbound
{
    private readonly MqttProducer _mqttProducer;
    private readonly RobotStateHubService _robotStateHubService;

    public RobotInbound(MqttProducer mqttProducer, RobotStateHubService robotStateHubService)
    {
        _mqttProducer = mqttProducer;
        _robotStateHubService = robotStateHubService;
    }

    public void HandleCommand()
    {
        throw new NotImplementedException();
    }
}