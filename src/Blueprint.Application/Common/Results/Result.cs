namespace Blueprint.Application.Common.Results;


public sealed class Result<T>
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;

    public T? Value { get; }

    public ErrorCode? ErrorCode { get; }
    public string? ErrorMessage { get; }

    private Result(
        bool isSuccess,
        T? value,
        ErrorCode? errorCode,
        string? errorMessage)
    {
        IsSuccess = isSuccess;
        Value = value;
        ErrorCode = errorCode;
        ErrorMessage = errorMessage;
    }

    public static Result<T> Success(T value)
        => new(true, value, null, null);

    public static Result<T> Failure(
        ErrorCode errorCode,
        string errorMessage)
        => new(false, default, errorCode, errorMessage);
}
