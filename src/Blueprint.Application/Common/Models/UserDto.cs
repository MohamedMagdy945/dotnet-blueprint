namespace Blueprint.Application.Common.Models;

public sealed record UserDto(
    Guid Id,
    string Email,
    IReadOnlyList<string> Roles
);