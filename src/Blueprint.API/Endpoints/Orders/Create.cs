using Blueprint.API.Abstractions;
using Blueprint.API.Extensions;
using Blueprint.Application.Features.Orders.Create;
namespace Blueprint.API.Endpoints.Orders;

public sealed class Create : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("orders", HandleAsync)
            .WithName("CreateOrder")
            .WithTags("Orders")
            .RequireAuthorization(policy => policy.RequireRole("User"))
            .WithValidation<CreateOrderRequest>()
            .Produces<CreateOrderResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest);
    }

    public static async Task<IResult> HandleAsync(
        CreateOrderRequest request,
        CreateOrderHandler handler,
        ILogger<Create> logger,
        CancellationToken ct)
    {

        var result = await handler.HandleAsync(request, ct);

        return result.ToHttpResult();
    }
}