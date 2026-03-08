using Api.Logging;
using MQTTnet;
using MQTTnet.Formatter;

namespace Api.Mqtt;

public class MqttConnection : IDisposable
{
    private readonly ILogger _logger;
    private readonly SemaphoreSlim _connectLock = new(1, 1);
    private readonly IMqttClient _mqttClient;
    private bool _disposed;

    public MqttConnection(ILoggerFactory loggerFactory, MqttClientFactory mqttClientFactory)
    {
        _logger = loggerFactory.CreateLoggerApi();
        _mqttClient = mqttClientFactory.CreateMqttClient();
    }

    public async Task<IMqttClient> ConnectAsync()
    {
        await _connectLock.WaitAsync();

        try
        {
            if (_mqttClient.IsConnected)
            {
                return _mqttClient;
            }

            _logger.LogInformation("Connecting to mqtt broker");

            var options = new MqttClientOptionsBuilder()
                .WithTcpServer(MqttConfiguration.MqttBrokerUrl, MqttConfiguration.MqttBrokerPort)
                .WithProtocolVersion(MqttProtocolVersion.V500)
                .Build();

            await _mqttClient.ConnectAsync(options);
        }
        catch (Exception exception)
        {
            _logger.LogError($"Error during connecting to mqtt broker, error message: {exception.Message}");
        }
        finally
        {
            _connectLock.Release();
        }
        
        return _mqttClient;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
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