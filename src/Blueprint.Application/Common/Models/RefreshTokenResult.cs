namespace Blueprint.Application.Common.Models;

public sealed record RefreshTokenResult(
    string Token,
    string TokenHash,
    DateTime ExpiresAt
);