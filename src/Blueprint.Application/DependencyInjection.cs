using Microsoft.Extensions.DependencyInjection;

namespace Blueprint.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        return services;
    }

    public static IServiceCollection AddApplication(this IServiceCollection services)
        => services.AddApplicationServices();
}