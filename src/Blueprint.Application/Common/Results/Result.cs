namespace Blueprint.Application.Common.Results;

public sealed class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;

    public string? ErrorCode { get; }
    public string? ErrorMessage { get; }

    private Result(
        bool isSuccess,
        string? errorCode,
        string? errorMessage)
    {
        IsSuccess = isSuccess;
        ErrorCode = errorCode;
        ErrorMessage = errorMessage;
    }

    public static Result Success()
        => new(true, null, null);

    public static Result Failure(
        string errorCode,
        string errorMessage)
        => new(false, errorCode, errorMessage);
}