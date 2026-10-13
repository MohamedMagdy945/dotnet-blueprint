using Blueprint.Application.Common;
using Blueprint.Application.Common.Abstractions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Blueprint.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {

        services.AddValidatorsFromAssembly(typeof(ApplicationAssemblyReference).Assembly);

        services.Scan(scan => scan
            .FromAssemblyOf<IHandler>()
            .AddClasses(classes => classes.Where(type => type.Name.EndsWith("Handler")))
            .AsSelf()
            .WithScopedLifetime());

        return services;
    }
}