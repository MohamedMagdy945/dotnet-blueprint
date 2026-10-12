
using Blueprint.Application;
using Blueprint.Infrastructure;

namespace Blueprint.API;

public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var allowedOrigins = configuration
            .GetSection("Cors:AllowedOrigins")
            .Get<string[]>() ?? [];

        services.AddApplicationServices();
        services.AddInfrastructureServices(configuration);
        return services;
    }
}