using System.Reflection;
using Blueprint.API.Common.Abstractions;

namespace Blueprint.API.Extensions;

public static class EndpointExtensions
{
    public static IEndpointRouteBuilder MapEndpoints(
        this IEndpointRouteBuilder app,
        Assembly assembly)
    {
        var endpointTypes = assembly
            .DefinedTypes
            .Where(type =>
                type is { IsAbstract: false, IsInterface: false } &&
                typeof(IEndpoint).IsAssignableFrom(type));

        foreach (var endpointType in endpointTypes)
        {
            if (Activator.CreateInstance(endpointType.AsType())
                is IEndpoint endpoint)
            {
                endpoint.MapEndpoint(app);
            }
        }

        return app;
    }
}