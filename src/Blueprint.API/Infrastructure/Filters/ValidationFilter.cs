using FluentValidation;

namespace Blueprint.API.Infrastructure.Filters;

public sealed class ValidationFilter<TRequest>(
    IValidator<TRequest> validator) : IEndpointFilter
    where TRequest : notnull
{
    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext context,
        EndpointFilterDelegate next)
    {
        var request = context.Arguments
            .OfType<TRequest>()
            .FirstOrDefault();

        if (request is null)
            return await next(context);

        var result = await validator.ValidateAsync(request,
            context.HttpContext.RequestAborted);

        if (result.IsValid)
            return await next(context);

        var errors = result.Errors
            .GroupBy(error => error.PropertyName)
            .ToDictionary(
                group => group.Key,
                group => group
                    .Select(error => error.ErrorMessage)
                    .ToArray());

        return Results.ValidationProblem(
            errors,
            statusCode: StatusCodes.Status400BadRequest);
    }
}