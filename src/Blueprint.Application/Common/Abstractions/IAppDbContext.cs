using Blueprint.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Blueprint.Application.Common.Abstractions;

public interface IAppDbContext
{
    DbSet<Product> Products { get; }
    DbSet<User> Users { get; }
    DbSet<Role> Roles { get; }
    DbSet<UserRole> UserRoles { get; }
    DbSet<RefreshToken> RefreshTokens { get; }

    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
