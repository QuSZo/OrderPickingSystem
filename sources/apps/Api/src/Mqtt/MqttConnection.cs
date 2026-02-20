using MQTTnet;
using MQTTnet.Formatter;

namespace Api.Mqtt;

public class MqttConnection
{
    private readonly IMqttClient _mqttClient;

    public MqttConnection(MqttClientFactory mqttClientFactory)
    {
        _mqttClient = mqttClientFactory.CreateMqttClient();
    }

    public bool IsConnected => _mqttClient.IsConnected;

    public async Task<IMqttClient> Connect()
    {
        try
        {
            if (IsConnected)
            {
                return _mqttClient;
            }

            var options = new MqttClientOptionsBuilder()
                .WithTcpServer(MqttConfiguration.MqttBrokerUrl, MqttConfiguration.MqttBrokerPort)
                .WithProtocolVersion(MqttProtocolVersion.V500)
                .Build();

            await _mqttClient.ConnectAsync(options);
        }
        catch
        {
            throw;
        }
        
        return _mqttClient;
    }

    public async Task DisconnectAsync()
    {
        if (!IsConnected)
        {
            return;
        }

        await _mqttClient.DisconnectAsync();
    }
}