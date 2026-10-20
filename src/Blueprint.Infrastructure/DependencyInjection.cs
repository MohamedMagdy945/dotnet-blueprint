using Blueprint.Application.Common.Abstractions;
using Blueprint.Infrastructure.Common.Extensions;
using Blueprint.Infrastructure.Common.Settings;
using Blueprint.Infrastructure.Persistence;
using Blueprint.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Blueprint.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration
          .GetConnectionString("DefaultConnection");

        services.AddDbContext<IAppDbContext, AppDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });


        services.Configure<Jwt>
            (configuration.GetSection("Jwt"));

        services.AddJwtAuthentication(configuration);


        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        services.AddSingleton<IJwtService, JwtService>();



        return services;
    }

}