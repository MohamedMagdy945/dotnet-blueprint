namespace Blueprint.Domain.Common.Results;

public class DomainResult
{
    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public string? Error { get; }

    protected DomainResult(
        bool isSuccess,
        string? error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public static DomainResult Success()
        => new(true, null);

    public static DomainResult Failure(string error)
        => new(false, error);
}