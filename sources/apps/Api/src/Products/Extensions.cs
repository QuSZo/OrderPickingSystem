namespace Api.Products;

public static class Extensions
{
    public static void AddProductsService(this IServiceCollection services)
    {
        services.AddSingleton<ProductsService>();
    }
}