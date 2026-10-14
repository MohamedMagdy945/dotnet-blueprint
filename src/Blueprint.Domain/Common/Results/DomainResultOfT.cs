namespace Blueprint.Domain.Common.Results;

public sealed class DomainResult<T> : DomainResult
{
    public T? Value { get; }

    private DomainResult(bool isSuccess, T? value, string? error) : base(isSuccess, error)
    {
        Value = value;
    }

    public static DomainResult<T> Success(T value)
        => new(true, value, null);

    public static new DomainResult<T> Failure(string error)
        => new(false, default, error);
}