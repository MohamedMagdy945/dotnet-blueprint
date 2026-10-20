using Blueprint.Domain.Entities;

namespace Blueprint.Application.Features.Auth.Login;

public static class LoginProjection
{
    public static IQueryable<LoginUserData> SelectLoginUserData(this IQueryable<User> query)
    {
        return query.Select(x => new LoginUserData
        {
            Id = x.Id,
            Email = x.Email,
            PasswordHash = x.PasswordHash,
            IsActive = x.IsActive,
            Roles = x.UserRoles.Select(ur => ur.Role.Name).ToList()
        });
    }
}