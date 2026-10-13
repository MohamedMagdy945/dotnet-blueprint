using Blueprint.Application.Common.Abstractions;
using Microsoft.Extensions.Logging;

namespace Blueprint.Application.Features.Orders.Create;

public record OrderItemDto(string ProductName, int Quantity, decimal UnitPrice);

public record CreateOrderResponse(Guid OrderId, string Status);

public class CreateOrderHandler(ILogger<CreateOrderHandler> logger) : IHandler
{

    public async Task<CreateOrderResponse> HandleAsync(CreateOrderRequest request, CancellationToken ct = default)
    {

        var orderId = Guid.NewGuid();

        return new CreateOrderResponse(orderId, "Created");
    }
}