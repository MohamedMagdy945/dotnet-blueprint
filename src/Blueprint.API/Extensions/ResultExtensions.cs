
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

        var (statusCode, title) = result.ErrorCode switch
        {
            ErrorCode.NotFound => (StatusCodes.Status404NotFound, "Not Found"),
            ErrorCode.AlreadyExists => (StatusCodes.Status409Conflict, "Conflict"),
            ErrorCode.ValidationFailed => (StatusCodes.Status400BadRequest, "Bad Request"),
            _ => (StatusCodes.Status400BadRequest, "Bad Request")
        };

        return Results.Problem(statusCode: statusCode,
            title: title,
            detail: result.ErrorMessage);
    }

}