using System.Text;
using Api.Logging;
using MQTTnet;

namespace Api.Mqtt;

public class MqttConsumer
{
    private readonly ILogger _logger;
    private readonly MqttConnection _mqttConnection;
    private readonly MqttClientFactory _mqttClientFactory;
    private IMqttClient? _mqttClient;
    private string? _topic = null;

    public MqttConsumer(
        ILoggerFactory loggerFactory,
        MqttConnection mqttConnection, 
        MqttClientFactory mqttClientFactory)
    {
        _logger = loggerFactory.CreateLoggerApi();
        _mqttConnection = mqttConnection;
        _mqttClientFactory = mqttClientFactory;
    }

    public event Func<string, Task>? ReceivedMessage;

    public async Task SubscribeAsync(string topic)
    {
        try
        {
            if (string.IsNullOrEmpty(topic) || _topic != null)
            {
                return;
            }

            _logger.LogInformation($"Subscribing to mqtt topic: {topic}");
            _mqttClient = await _mqttConnection.ConnectAsync();

            MqttClientSubscribeOptions mqttSubscribeOptions = _mqttClientFactory
                .CreateSubscribeOptionsBuilder()
                .WithTopicFilter(topic)
                .Build();

            _mqttClient.ApplicationMessageReceivedAsync += HandleMessage;
            await _mqttClient.SubscribeAsync(mqttSubscribeOptions);
            _topic = topic;
        }
        catch (Exception exception)
        {
            _logger.LogError($"Error during subscribing to mqtt topic {_topic}, error message {exception.Message}");
            throw;
        }
    }

    public async Task UnsubscribeAsync()
    {
        try
        {
            if(_topic == null || _mqttClient == null || !_mqttClient.IsConnected)
            {
                return;
            }

            _logger.LogInformation($"Unsubscribing mqtt topic: {_topic}");

            MqttClientUnsubscribeOptions mqttUnubscribeOptions = _mqttClientFactory
                .CreateUnsubscribeOptionsBuilder()
                .WithTopicFilter(_topic)
                .Build();

            _mqttClient.ApplicationMessageReceivedAsync -= HandleMessage;
            await _mqttClient.UnsubscribeAsync(mqttUnubscribeOptions);
            _topic = null;
        }
        catch (Exception exception)
        {
            _logger.LogError($"Error during unsubscribing mqtt topic {_topic}, error message {exception.Message}");
            throw;
        }
    }

    private Task HandleMessage(MqttApplicationMessageReceivedEventArgs receivedEventArgs)
    {
        if (receivedEventArgs.ApplicationMessage.Topic == _topic)
        {
            var byteMessage = receivedEventArgs.ApplicationMessage.Payload;
            string message = Encoding.UTF8.GetString(byteMessage);
            _logger.LogDebug($"Consumed message on mqtt topic: {_topic}, message {message}");
            ReceivedMessage?.Invoke(message);
        }
        
        return Task.CompletedTask;
    }
}