using Blueprint.Application.Common.Models;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;

namespace Blueprint.Infrastructure.Services;

public sealed class RefreshTokenGenerator(IOptions<RefreshTokenSettings> options) : IRefreshTokenGenerator
{

    private readonly RefreshTokenSettings _settings = options.Value;

    public RefreshTokenResult Generate()
    {
        var expiresAt = DateTime.UtcNow.AddDays(_settings.ExpirationInDays);

        Span<byte> bytes = stackalloc byte[RefreshTokenSize];
        RandomNumberGenerator.Fill(bytes);

        var refreshToken = Convert.ToBase64String(bytes);

        Span<byte> hashBytes = stackalloc byte[SHA256.HashSizeInBytes];
        SHA256.HashData(Encoding.UTF8.GetBytes(refreshToken), hashBytes);

        string hashedToken = Convert.ToHexString(hashBytes);

        return new RefreshTokenResult(refreshToken, hashedToken, expiresAt);
    }

}