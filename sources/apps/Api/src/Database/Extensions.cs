using Microsoft.EntityFrameworkCore;

namespace Api.Database;

public static class Extensions
{
    public static void AddPostgres(this IServiceCollection services)
    {
        services.AddDbContext<OrderPickingDbContext>(options => options.UseNpgsql(PostgresOptions.ConnectionString));
    }
}
