using Blueprint.API.Abstractions;
using System.Reflection;

namespace Blueprint.API.Extensions;

public static class EndpointRegistrationExtensions
{
    public static IEndpointRouteBuilder MapEndpoints(
        this IEndpointRouteBuilder app,
        Assembly assembly)
    {
        // 1. Create the base /api group internally
        var apiGroup = app.MapGroup("/api")
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        // 2. Discover all classes implementing IEndpoint
        var endpointTypes = assembly
            .DefinedTypes
            .Where(type =>
                type is { IsAbstract: false, IsInterface: false } &&
                typeof(IEndpoint).IsAssignableFrom(type));

        // 3. Register each endpoint against apiGroup instead of app
        foreach (var endpointType in endpointTypes)
        {
            if (Activator.CreateInstance(endpointType.AsType()) is IEndpoint endpoint)
            {
                endpoint.MapEndpoint(apiGroup);
            }
        }

        return app;
    }
}