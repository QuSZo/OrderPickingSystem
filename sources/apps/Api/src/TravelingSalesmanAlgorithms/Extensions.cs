namespace Api.TravelingSalesmanAlgorithms;

public static class Extensions
{
    public static void AddTspAlgorithm(this IServiceCollection services)
    {
        services.AddSingleton<TravelingSalesmanAlgorithmProvider>();
    }
}