using Blueprint.Application.Common.Models;

namespace Blueprint.Application.Common.Contracts;

public sealed record AuthResponse(
    string AccessToken,
    DateTime AccessTokenExpiresAt,
    UserDto User
);


