using Api.Logging;
using Microsoft.AspNetCore.SignalR;

namespace Api.WebSockets;

public class RobotStateHubService
{
    private readonly ILogger _logger;
    private readonly IHubContext<RobotStateHub> _hub;

    public RobotStateHubService(ILoggerFactory loggerFactory, IHubContext<RobotStateHub> hub)
    {
        _logger = loggerFactory.CreateLoggerApi();
        _hub = hub;
    }

    public async Task SendMessageAsync(string message, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Sending new robot state via WebSocket");
        await _hub.Clients.All.SendAsync("ReceiveRobotState", message);
    }
}