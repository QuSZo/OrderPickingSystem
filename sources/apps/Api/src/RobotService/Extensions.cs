namespace Api.RobotService;

public static class Extensions
{
    public static void AddRobotService(this IServiceCollection services)
    {
        services.AddHostedService<RobotInbound>();
        services.AddHostedService<RobotOutbound>();
    }
}