using Blueprint.API.Abstractions;
using Blueprint.API.Extensions;
using Blueprint.Application.Features.Orders.Create;

namespace Blueprint.API.Endpoints.Orders;

public sealed class CreateOrderEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("orders", HandleAsync)
            .WithName("CreateOrder")
            .WithTags("Orders")
            .WithValidation<CreateOrderCommand>()
            .Produces<CreateOrderResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest);
    }

    public static async Task<IResult> HandleAsync(
        CreateOrderRequest request,
        CreateOrderHandler handler,
        ILogger<CreateOrderEndpoint> logger,
        CancellationToken ct)
    {
        logger.LogInformation("Handling CreateOrder request");

        var command = new CreateOrderCommand();
        var result = await handler.HandleAsync(command, ct);

        return Results.Created($"/api/orders/{result.OrderId}", result);
    }
}