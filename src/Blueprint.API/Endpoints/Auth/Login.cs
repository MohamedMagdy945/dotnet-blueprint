using Blueprint.API.Abstractions;
using Blueprint.API.Extensions;
using Blueprint.Application.Common.Contracts;
using Blueprint.Application.Features.Auth.Login;

namespace Blueprint.API.Endpoints.Auth;


public sealed class Login : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/authentication/login", HandleAsync)
            .WithValidation<LoginRequest>();
    }

    private static async Task<IResult> HandleAsync(LoginRequest request, LoginHandler handler, HttpContext httpContext,
        CancellationToken ct)
    {
        var result = await handler.HandleAsync(request, ct);

        if (!result.IsSuccess || result.Value is null)
            return result.ToHttpResult();

        httpContext.Response.SetRefreshTokenCookie(
            result.Value.RefreshToken,
            result.Value.RefreshTokenExpiresAt,
            httpContext.Request.IsHttps);

        var authResponse = new AuthResponse(
            result.Value.AccessToken,
            result.Value.AccessTokenExpiresAt,
            result.Value.User
        );

        return Results.Ok(authResponse);

    }
}