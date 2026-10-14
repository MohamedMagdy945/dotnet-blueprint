namespace Blueprint.Application.Common.Models;

public sealed record UserClaims(Guid UserId, string Email, IEnumerable<string> Roles);