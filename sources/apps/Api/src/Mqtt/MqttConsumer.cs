using System.Text;
using MQTTnet;

namespace Api.Mqtt;

public class MqttConsumer
{
    private readonly MqttConnection _mqttConnection;
    private readonly MqttClientFactory _mqttClientFactory;
    private IMqttClient? _mqttClient;
    private string? _topic = null;

    public MqttConsumer(
        MqttConnection mqttConnection, 
        MqttClientFactory mqttClientFactory)
    {
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

            _mqttClient = await _mqttConnection.ConnectAsync();

            MqttClientSubscribeOptions mqttSubscribeOptions = _mqttClientFactory
                .CreateSubscribeOptionsBuilder()
                .WithTopicFilter(topic)
                .Build();

            _mqttClient.ApplicationMessageReceivedAsync += HandleMessage;
            await _mqttClient.SubscribeAsync(mqttSubscribeOptions);
            _topic = topic;
        }
        catch
        {
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

            MqttClientUnsubscribeOptions mqttUnubscribeOptions = _mqttClientFactory
                .CreateUnsubscribeOptionsBuilder()
                .WithTopicFilter(_topic)
                .Build();

            _mqttClient.ApplicationMessageReceivedAsync -= HandleMessage;
            await _mqttClient.UnsubscribeAsync(mqttUnubscribeOptions);
            _topic = null;
        }
        catch
        {
            throw;
        }
    }

    private Task HandleMessage(MqttApplicationMessageReceivedEventArgs receivedEventArgs)
    {
        if (receivedEventArgs.ApplicationMessage.Topic == _topic)
        {
            var byteMessage = receivedEventArgs.ApplicationMessage.Payload;
            string message = Encoding.UTF8.GetString(byteMessage);
            ReceivedMessage?.Invoke(message);
        }
        
        return Task.CompletedTask;
    }
}