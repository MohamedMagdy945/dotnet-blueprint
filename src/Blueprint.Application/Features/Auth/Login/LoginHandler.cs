using Blueprint.Application.Common.Abstractions;
using Blueprint.Application.Common.Extensions;
using Blueprint.Application.Common.Models;
using Blueprint.Application.Common.Results;
using Blueprint.Application.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Blueprint.Application.Features.Auth.Login;

public class LoginHandler(
    IAppDbContext dbContext,
    IPasswordHasher passwordHasher,
    SessionService sessionService,
    ILogger<LoginHandler> logger
    ) : IHandler
{
    public async Task<Result<AuthResult>> HandleAsync(LoginRequest request, CancellationToken ct)
    {
        var email = request.Email.NormalizeEmail();

        var credentials = await dbContext.Users
            .AsNoTracking()
            .Where(x => x.Email == email)
            .SelectLoginCredentials()
            .SingleOrDefaultAsync(ct);

        if (credentials is null || !credentials.IsActive || !passwordHasher.Verify(request.Password, credentials.PasswordHash))
        {
            logger.LogWarning("Failed login attempt for email {Email}.", email);
            return Result<AuthResult>.Failure(ErrorCode.Unauthorized, "Invalid email or password.");
        }

        var userClaims = new UserClaims(credentials.Id, credentials.Email, credentials.Roles);

        var tokens = await sessionService.CreateAsync(userClaims, ct);

        var authResult = new AuthResult(
            tokens.AccessToken,
            tokens.AccessTokenExpiresAt,
            tokens.RefreshToken,
            tokens.RefreshTokenExpiresAt,
            new UserDto(credentials.Id, credentials.Email, credentials.Roles)
        );

        return Result<AuthResult>.Success(authResult);
    }

}
