using Api.Logging;
using Api.Mqtt;
using Api.WebSockets;

namespace Api.RobotService;

public class RobotOutbound : IHostedService
{
    private readonly ILogger _logger;
    private readonly MqttConsumer _mqttConsumer;
    private readonly RobotStateHubService _robotStateHubService;

    public RobotOutbound(ILoggerFactory loggerFactory, MqttConsumer mqttConsumer, RobotStateHubService robotStateHubService)
    {
        _logger = loggerFactory.CreateLoggerApi();
        _mqttConsumer = mqttConsumer;
        _robotStateHubService = robotStateHubService;

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
        await _robotStateHubService.SendMessageAsync(message);
    }
}