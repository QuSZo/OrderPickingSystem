namespace Api.RobotServices;

public static class Extensions
{
    public static void AddRobotService(this IServiceCollection services)
    {
        services.AddSingleton<RobotInbound>();
        services.AddSingleton<RobotState>();
        services.AddHostedService<RobotOutbound>();
    }
}