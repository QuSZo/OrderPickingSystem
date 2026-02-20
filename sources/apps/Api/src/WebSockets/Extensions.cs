namespace Api.WebSockets;

public static class Extensions
{
    public static void AddWebSocketServices(this IServiceCollection services)
    {
        services.AddSingleton<RobotStateHubService>();
    }

    public static void MapHubs(this WebApplication app)
    {
        app.MapHub<RobotStateHub>("/api/robot-hub");
    }
}