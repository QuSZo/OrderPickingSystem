namespace Api.Policies;

public static class CorsPolicy
{
    public static void AddCorsPolicy(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
                policy.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod());
        });
    }
}