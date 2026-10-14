using Blueprint.Application.Common.Abstractions;
using Blueprint.Application.Common.Models;
using Blueprint.Infrastructure.Common.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Text;

namespace Blueprint.Infrastructure.Services;

public class TokenService(IOptions<TokenSettings> options) : ITokenService
{
    private const string Roles = "roles";
    private const int RefreshTokenSize = 32;

    private readonly TokenSettings _settings = options.Value;

    private static readonly JsonWebTokenHandler JwtHandler = new();

    private readonly SigningCredentials _signingCredentials = new(
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Value.SecretKey)), SecurityAlgorithms.HmacSha256);


    public AccessTokenResult GenerateAccessToken(UserClaims userClaims)
    {
        var expiresAt = DateTime.UtcNow.AddMinutes(_settings.AccessTokenExpirationInMinutes);

        var claims = new Dictionary<string, object>
        {
            [JwtRegisteredClaimNames.Sub] = userClaims.UserId.ToString(),
            [JwtRegisteredClaimNames.Email] = userClaims.Email,
            [JwtRegisteredClaimNames.Jti] = Guid.NewGuid().ToString(),
            [Roles] = userClaims.Roles.ToArray()
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = _settings.Issuer,
            Audience = _settings.Audience,
            Claims = claims,
            Expires = expiresAt,
            SigningCredentials = _signingCredentials
        };

        var handler = new JsonWebTokenHandler();
        string token = handler.CreateToken(tokenDescriptor);

        return new AccessTokenResult(token, expiresAt);
    }

    public RefreshTokenResult GenerateRefreshToken()
    {
        var expiresAt = DateTime.UtcNow.AddDays(_settings.RefreshTokenExpirationInDays);

        Span<byte> bytes = stackalloc byte[RefreshTokenSize];
        RandomNumberGenerator.Fill(bytes);

        var refreshToken = Convert.ToBase64String(bytes);

        Span<byte> hashBytes = stackalloc byte[SHA256.HashSizeInBytes];
        SHA256.HashData(Encoding.UTF8.GetBytes(refreshToken), hashBytes);

        string hashedToken = Convert.ToHexString(hashBytes);

        return new RefreshTokenResult(refreshToken, hashedToken, expiresAt);
    }
}
