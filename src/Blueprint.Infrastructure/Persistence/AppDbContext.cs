using Microsoft.EntityFrameworkCore;

namespace Blueprint.Infrastructure.Persistence;

public class AppDbContext(
    DbContextOptions<AppDbContext> options)
    : DbContext(options)
{

}