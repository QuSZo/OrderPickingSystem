using Microsoft.AspNetCore.SignalR;

namespace Api.WebSockets;

public class RobotStateHubService
{
    private readonly IHubContext<RobotStateHub> _hub;

    public RobotStateHubService(IHubContext<RobotStateHub> hub)
    {
        _hub = hub;
    }

    public async Task SendMessageAsync(string message, CancellationToken cancellationToken = default)
    {
        await _hub.Clients.All.SendAsync("ReceiveRobotState", message);
    }
}