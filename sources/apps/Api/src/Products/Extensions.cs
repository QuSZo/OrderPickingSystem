namespace Api.Products;

public static class Extensions
{
    public static void AddProducts(this IServiceCollection services)
    {
        services.AddSingleton<ProductsRepository>();
    }
}