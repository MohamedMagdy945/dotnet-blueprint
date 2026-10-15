
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

        var statusCode = result.ErrorCode switch
        {
            ErrorCode.NotFound => StatusCodes.Status404NotFound,
            ErrorCode.AlreadyExists => StatusCodes.Status409Conflict,
            ErrorCode.ValidationFailed => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status400BadRequest
        };

        return Results.Problem(
            statusCode: statusCode,
            detail: result.ErrorMessage);
    }

}