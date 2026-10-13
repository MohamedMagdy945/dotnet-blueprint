using Blueprint.Application.Common.Abstractions;
using Blueprint.Application.Common.Results;
using Microsoft.Extensions.Logging;

namespace Blueprint.Application.Features.Orders.Create;

public class CreateOrderHandler(ILogger<CreateOrderHandler> logger) : IHandler
{

    public async Task<Result<CreateOrderResponse>> HandleAsync(CreateOrderRequest request, CancellationToken ct = default)
    {

        var orderId = Guid.NewGuid();

        return Result<CreateOrderResponse>.Success(new CreateOrderResponse(orderId));
    }
}