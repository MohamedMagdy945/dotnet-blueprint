using Blueprint.Application.Common.Models;

namespace Blueprint.Application.Common.Abstractions;

public interface IJwtService
{
    AccessTokenResult GenerateAccessToken(UserClaims userClaims);
    RefreshTokenResult GenerateRefreshToken();
}
