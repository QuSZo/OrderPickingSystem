using Api.WebSockets;

namespace Api.Mqtt;

public class MqttConsumer
{
    private readonly MqttConnection _mqttConnection;

    public MqttConsumer(MqttConnection mqttConnection)
    {
        _mqttConnection = mqttConnection;
    }

    public async Task Subscibe(string topic)
    {
        
    }
}