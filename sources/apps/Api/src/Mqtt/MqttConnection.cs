using MQTTnet;
using MQTTnet.Formatter;

namespace Api.Mqtt;

public class MqttConnection : IDisposable
{
    private readonly SemaphoreSlim _connectLock = new(1, 1);
    private readonly IMqttClient _mqttClient;
    private bool _disposed;

    public MqttConnection(MqttClientFactory mqttClientFactory)
    {
        _mqttClient = mqttClientFactory.CreateMqttClient();
    }

    public bool IsConnected => _mqttClient.IsConnected;
    public event Action? Connected;
    public event Action? Disconnected;

    public async Task<IMqttClient> ConnectAsync()
    {
        await _connectLock.WaitAsync();

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

            _mqttClient.DisconnectedAsync += OnDisconnected;
            _mqttClient.ConnectedAsync += OnConnected;
            await _mqttClient.ConnectAsync(options);
        }
        catch
        {
            throw;
        }
        finally
        {
            _connectLock.Release();
        }
        
        return _mqttClient;
    }

    private Task OnConnected(MqttClientConnectedEventArgs args)
    {
        Connected?.Invoke();
        return Task.CompletedTask;
    }

    private Task OnDisconnected(MqttClientDisconnectedEventArgs args)
    {
        Disconnected?.Invoke();
        return Task.CompletedTask;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
            _mqttClient.ConnectedAsync -= OnConnected;
            _mqttClient.DisconnectedAsync -= OnDisconnected;
            _mqttClient.Dispose();
        }

        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}