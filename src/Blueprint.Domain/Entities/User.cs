using Blueprint.Domain.Common.BasicEntities;
using System.Data;

namespace Blueprint.Domain.Entities;

public class User : SoftDeletableEntity
{
    public string Email { get; private set; } = string.Empty;
    public string UserName { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public bool IsActive { get; private set; } = true;

    private readonly List<UserRole> _userRoles = [];
    public IReadOnlyCollection<UserRole> UserRoles => _userRoles.AsReadOnly();

    private readonly List<RefreshToken> _refreshTokens = [];
    public IReadOnlyCollection<RefreshToken> RefreshTokens => _refreshTokens.AsReadOnly();

    private User() : base() { }

    public static User Create(string email, string userName, string passwordHash)
    {
        return new User
        {
            Id = Guid.NewGuid(),
            Email = email.Trim().ToLowerInvariant(),
            UserName = userName.Trim(),
            PasswordHash = passwordHash,
            IsActive = true,
            CreatedOnUtc = DateTime.UtcNow
        };
    }

    public void UpdatePassword(string newPasswordHash)
    {
        PasswordHash = newPasswordHash;
        ModifiedOnUtc = DateTime.UtcNow;
        RevokeAllRefreshTokens();
    }

    public void UpdateProfile(string userName)
    {
        UserName = userName.Trim();
        ModifiedOnUtc = DateTime.UtcNow;
    }

    public void AssignRole(Guid roleId)
    {
        if (_userRoles.Any(ur => ur.RoleId == roleId))
            return;

        _userRoles.Add(new UserRole(Id, roleId));
        ModifiedOnUtc = DateTime.UtcNow;
    }

    public void RemoveRole(Guid roleId)
    {
        var existingRole = _userRoles.FirstOrDefault(ur => ur.RoleId == roleId);
        if (existingRole is not null)
        {
            _userRoles.Remove(existingRole);
            ModifiedOnUtc = DateTime.UtcNow;
        }
    }

    public void Deactivate()
    {
        IsActive = false;
        ModifiedOnUtc = DateTime.UtcNow;
        RevokeAllRefreshTokens();
    }

    public void Activate()
    {
        IsActive = true;
        ModifiedOnUtc = DateTime.UtcNow;
    }

    public override void SoftDelete()
    {
        base.SoftDelete();
        RevokeAllRefreshTokens();
    }

    public RefreshToken AddRefreshToken(string token, DateTime expiresOnUtc)
    {
        var refreshToken = RefreshToken.Create(token, expiresOnUtc, Id);
        _refreshTokens.Add(refreshToken);
        return refreshToken;
    }

    public void RevokeAllRefreshTokens()
    {
        foreach (var token in _refreshTokens.Where(t => t.IsActive))
        {
            token.Revoke();
        }
    }
}