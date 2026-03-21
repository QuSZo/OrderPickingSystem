namespace Api.Services;

public static class Extensions
{
    public static void AddStatisticsService(this IServiceCollection services)
    {
        services.AddSingleton<StatisticsService>();
    }
}