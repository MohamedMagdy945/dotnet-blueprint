using Blueprint.Infrastructure.Common.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Blueprint.Infrastructure.Common.Extensions;


public static class JwtAuthenticationExtensions
{
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var settings = configuration.GetRequiredSection(nameof(Jwt))
                         .Get<Jwt>()!;

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.MapInboundClaims = false;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = "sub",
                    RoleClaimType = "role",
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = settings.Issuer,
                    ValidAudience = settings.Audience,

                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.Key)),

                    ClockSkew = TimeSpan.Zero

                };

                options.Events = new JwtBearerEvents
                {
                    OnChallenge = async context =>
                    {
                        context.HandleResponse();

                        if (context.Response.HasStarted) return;

                        var problemDetailsService = context.HttpContext.RequestServices.GetRequiredService<IProblemDetailsService>();

                        context.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;

                        await problemDetailsService.WriteAsync(new ProblemDetailsContext
                        {
                            HttpContext = context.HttpContext,
                            ProblemDetails = new ProblemDetails
                            {
                                Status = StatusCodes.Status401Unauthorized,
                                Title = "Unauthorized",
                                Detail = "Authentication is required to access this resource."
                            }
                        });
                    },

                    OnForbidden = async context =>
                    {
                        var problemDetailsService = context.HttpContext.RequestServices.GetRequiredService<IProblemDetailsService>();

                        context.HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;

                        await problemDetailsService.WriteAsync(new ProblemDetailsContext
                        {
                            HttpContext = context.HttpContext,
                            ProblemDetails = new ProblemDetails
                            {
                                Status = StatusCodes.Status403Forbidden,
                                Title = "Forbidden",
                                Detail = "You do not have permission to access this resource."
                            }
                        });
                    }
                };
            });

        return services;
    }

}
