using Blueprint.Application.Common.Abstractions;
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

        services.AddSingleton<IPasswordHasher, PasswordHasher>();


        return services;
    }

}