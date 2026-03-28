namespace Api.Orders;

public static class Extensions
{
    public static void AddOrders(this IServiceCollection services)
    {
        services.AddScoped<IOrdersRepository, OrdersRepository>();
    }
}