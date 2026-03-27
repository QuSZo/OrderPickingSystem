namespace Api.Orders;

public static class Extensions
{
    public static void AddOrders(this IServiceCollection services)
    {
        services.AddSingleton<IOrdersRepository, InMemoryOrdersRepository>();
    }
}