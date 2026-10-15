using Blueprint.Infrastructure.Common.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Blueprint.Infrastructure.Common.Extensions;


public static class JwtAuthenticationExtensions
{
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var settings = configuration.GetRequiredSection(nameof(JwtSettings))
                         .Get<JwtSettings>()!;


        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.MapInboundClaims = false;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    RoleClaimType = "role",
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = settings.Issuer,
                    ValidAudience = settings.Audience,

                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.SecretKey)),

                    ClockSkew = TimeSpan.Zero

                };


                //options.Events = new JwtBearerEvents
                //{
                //    OnChallenge = context =>
                //    {
                //        context.HandleResponse();

                //        var problem = CreateProblemDetails(context.HttpContext, StatusCodes.Status401Unauthorized,
                //            "Unauthorized", "Authentication is required to access this resource.");

                //        return context.HttpContext.WriteProblemDetailsAsync(problem, context.HttpContext.RequestAborted);
                //    },


                //    OnForbidden = context =>
                //    {
                //        var problem = CreateProblemDetails(context.HttpContext, StatusCodes.Status403Forbidden,
                //            "Forbidden", "You do not have permission to access this resource.");

                //        return context.HttpContext.WriteProblemDetailsAsync(problem, context.HttpContext.RequestAborted);
                //    }
                //};
            });

        return services;
    }

}
