using Blueprint.Application.Common.Abstractions;
using Blueprint.Application.Common.Models;
using Blueprint.Domain.Entities;

namespace Blueprint.Application.Services;

public class SessionService(
    IAppDbContext dbContext,
    ITokenService tokenService
)
{
    public async Task<TokenResult> CreateAsync(UserClaims userClaims, CancellationToken ct = default)
    {
        var accessTokenResult = tokenService.GenerateAccessToken(userClaims);
        var refreshTokenResult = tokenService.GenerateRefreshToken();

        var refreshToken = RefreshToken.Create(
            refreshTokenResult.TokenHash,
            refreshTokenResult.ExpiresAt,
            userClaims.UserId);

        dbContext.RefreshTokens.Add(refreshToken);
        await dbContext.SaveChangesAsync(ct);

        return new TokenResult(
            accessTokenResult.Token,
            accessTokenResult.ExpiresAt,
            refreshTokenResult.Token,
            refreshTokenResult.ExpiresAt
        );
    }
}
