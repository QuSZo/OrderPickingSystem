using MQTTnet;

namespace Api.Mqtt;

public class MqttProducer
{
    private readonly MqttConnection _mqttConnection;
    private readonly MqttClientFactory _mqttClientFactory;

    public MqttProducer(MqttConnection mqttConnection, MqttClientFactory mqttClientFactory)
    {
        _mqttConnection = mqttConnection;
        _mqttClientFactory = mqttClientFactory;
    }

    public async Task PublishAsync(string topic, string message)
    {
        if (string.IsNullOrEmpty(topic))
        {
            return;
        }

        IMqttClient mqttClient = await _mqttConnection.ConnectAsync();

        MqttApplicationMessage mqttMessage = _mqttClientFactory
            .CreateApplicationMessageBuilder()
            .WithTopic(topic)
            .WithPayload(message)
            .Build();

        await mqttClient.PublishAsync(mqttMessage);
    }
}