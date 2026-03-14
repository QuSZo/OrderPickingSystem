namespace Api.RobotOperations;

public static class Extensions
{
    public static void AddRobotOperation(this IServiceCollection services)
    {
        services.AddSingleton<RobotOperation>();
    }
}