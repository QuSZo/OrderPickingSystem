using Api.Logging;
using MQTTnet;

namespace Api.Mqtt;

public class MqttProducer
{
    private readonly ILogger _logger;
    private readonly MqttConnection _mqttConnection;
    private readonly MqttClientFactory _mqttClientFactory;

    public MqttProducer(ILoggerFactory loggerFactory, MqttConnection mqttConnection, MqttClientFactory mqttClientFactory)
    {
        _logger = loggerFactory.CreateLoggerApi();
        _mqttConnection = mqttConnection;
        _mqttClientFactory = mqttClientFactory;
    }

    public async Task PublishAsync(string topic, string message)
    {
        try
        {

            if (string.IsNullOrEmpty(topic))
            {
                return;
            }

            _logger.LogDebug($"Publishing message {message} on mqtt topic: {topic}");

            IMqttClient mqttClient = await _mqttConnection.ConnectAsync();

            MqttApplicationMessage mqttMessage = _mqttClientFactory
                .CreateApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(message)
                .Build();

            await mqttClient.PublishAsync(mqttMessage);
        }
        catch (Exception exception)
        {
            _logger.LogError($"Error during mqtt publishing message, error message {exception.Message}");
            throw;
        }

    }
}