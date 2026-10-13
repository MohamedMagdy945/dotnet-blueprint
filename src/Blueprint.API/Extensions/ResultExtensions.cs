
using Blueprint.Application.Common.Results;

namespace Blueprint.API.Extensions;

public static class ResultExtensions
{
    public static IResult ToHttpResult<T>(this Result<T> result)
    {
        if (result.IsSuccess)
        {
            return Results.Ok(result.Value);
        }

        return result.ErrorCode switch
        {
            ErrorCode.NotFound =>
                Results.Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    title: "Not Found",
                    detail: "The requested resource was not found."),

            ErrorCode.AlreadyExists =>
                Results.Problem(
                    statusCode: StatusCodes.Status409Conflict,
                    title: "Conflict",
                    detail: "The resource already exists."),

            ErrorCode.ValidationFailed =>
                Results.Problem(
                    statusCode: StatusCodes.Status400BadRequest,
                    title: "Validation Failed",
                    detail: "The request contains invalid data."),

            ErrorCode.Unauthorized =>
                Results.Problem(
                    statusCode: StatusCodes.Status401Unauthorized,
                    title: "Unauthorized",
                    detail: "Authentication is required."),

            ErrorCode.Forbidden =>
                Results.Problem(
                    statusCode: StatusCodes.Status403Forbidden,
                    title: "Forbidden",
                    detail: "You are not allowed to perform this operation."),

            _ =>
                Results.Problem(
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Internal Server Error",
                    detail: "An unexpected error occurred.")
        };
    }
}