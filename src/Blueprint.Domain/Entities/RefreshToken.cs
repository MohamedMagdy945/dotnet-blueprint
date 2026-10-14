using Blueprint.Domain.Common.BasicEntities;

namespace Blueprint.Domain.Entities;

public class RefreshToken : BaseEntity
{
    public string TokenHash { get; private set; } = string.Empty;
    public DateTime ExpiresOnUtc { get; private set; }
    public DateTime CreatedOnUtc { get; private set; }
    public DateTime? RevokedOnUtc { get; private set; }
    public string? ReplacedByToken { get; private set; }

    public Guid UserId { get; private set; }
    public User User { get; private set; } = null!;

    public bool IsExpired => DateTime.UtcNow >= ExpiresOnUtc;
    public bool IsRevoked => RevokedOnUtc is not null;
    public bool IsActive => !IsRevoked && !IsExpired;

    private RefreshToken() : base() { }

    public static RefreshToken Create(string tokenHash, DateTime expiresOnUtc, Guid userId)
    {
        return new RefreshToken
        {
            Id = Guid.NewGuid(),
            TokenHash = tokenHash,
            ExpiresOnUtc = expiresOnUtc,
            CreatedOnUtc = DateTime.UtcNow,
            UserId = userId
        };
    }

    public void Revoke(string? replacedByToken = null)
    {
        RevokedOnUtc = DateTime.UtcNow;
        ReplacedByToken = replacedByToken;
    }
}