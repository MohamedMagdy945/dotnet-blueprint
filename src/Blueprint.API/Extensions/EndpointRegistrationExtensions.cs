using Asp.Versioning;
using Blueprint.API.Abstractions;
using System.Reflection;

namespace Blueprint.API.Extensions;

public static class EndpointRegistrationExtensions
{
    public static IEndpointRouteBuilder MapEndpoints(
        this IEndpointRouteBuilder app,
        Assembly assembly)
    {
        var apiVersionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1, 0))
            .HasApiVersion(new ApiVersion(2, 0))
            .ReportApiVersions()
            .Build();

        var apiGroup = app.MapGroup("/api/v{version:apiVersion}")
            .WithApiVersionSet(apiVersionSet)
            .MapToApiVersion(1, 0)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        var endpointTypes = assembly
            .DefinedTypes
            .Where(type =>
                type is { IsAbstract: false, IsInterface: false } &&
                typeof(IEndpoint).IsAssignableFrom(type));

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