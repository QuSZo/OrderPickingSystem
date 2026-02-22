using Api.Mqtt;
using Api.WebSockets;

namespace Api.RobotService;

public class RobotOutbound : IHostedService
{
    private readonly MqttConsumer _mqttConsumer;
    private readonly RobotStateHubService _robotStateHubService;

    public RobotOutbound(MqttConsumer mqttConsumer, RobotStateHubService robotStateHubService)
    {
        _mqttConsumer = mqttConsumer;
        _robotStateHubService = robotStateHubService;

        _mqttConsumer.ReceivedMessage += ProcessMessage;
    }

    public async Task HandleNewState(string message)
    {
        await _robotStateHubService.SendMessageAsync(message);
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
        await _robotStateHubService.SendMessageAsync(message);
    }
}