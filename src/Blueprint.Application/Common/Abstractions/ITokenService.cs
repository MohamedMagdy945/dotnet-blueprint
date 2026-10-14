using Blueprint.Application.Common.Models;

namespace Blueprint.Application.Common.Abstractions;

public interface ITokenService
{
    AccessTokenResult GenerateAccessToken(UserClaims userClaims);
    RefreshTokenResult GenerateRefreshToken();
}
