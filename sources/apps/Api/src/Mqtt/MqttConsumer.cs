using Api.WebSockets;

namespace Api.Mqtt;

public class MqttConsumer : BackgroundService
{
    private readonly RobotStateHubService _robotHub;

    public MqttConsumer(RobotStateHubService robotHub)
    {
        _robotHub = robotHub;        
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(TimeSpan.FromSeconds(1));
        int counter = 0;

        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            counter++;

            await _robotHub.SendMessage($"Tick: {counter}", cancellationToken: stoppingToken);
        }
    }
}