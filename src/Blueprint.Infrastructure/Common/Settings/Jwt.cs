namespace Blueprint.Infrastructure.Common.Settings;

public sealed class Jwt
{
    public string Key { get; init; } = string.Empty;
    public string Issuer { get; init; } = string.Empty;
    public string Audience { get; init; } = string.Empty;
    public int AccessTokenExpirationInMinutes { get; init; }
    public int RefreshTokenExpirationInDays { get; init; }

}