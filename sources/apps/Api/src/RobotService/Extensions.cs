namespace Api.RobotService;

public static class Extensions
{
    public static void AddRobotService(this IServiceCollection services)
    {
        services.AddSingleton<RobotInbound>();
        services.AddHostedService<RobotOutbound>();
    }
}