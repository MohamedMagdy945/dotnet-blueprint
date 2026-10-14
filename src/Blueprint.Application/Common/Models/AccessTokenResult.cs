namespace Blueprint.Application.Common.Models;

public sealed record AccessTokenResult(
    string Token,
    DateTime ExpiresAt
);