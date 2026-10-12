namespace Blueprint.Application.Common.Results;

public enum ErrorCode
{
    None = 0,
    NotFound,
    AlreadyExists,
    ValidationFailed,
    Unauthorized,
    Forbidden
}