using Microsoft.EntityFrameworkCore;

namespace TechChallenge.Infrastructure.Database;
public class TechChallengeDbContext(DbContextOptions<TechChallengeDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
        => modelBuilder.ApplyConfigurationsFromAssembly(typeof(TechChallengeDbContext).Assembly);
}
