using Microsoft.EntityFrameworkCore;

namespace TechChallenge.Infrastructure.Database;
public class TechChallengeDbContext(DbContextOptions dbContextOptions) : DbContext(dbContextOptions)
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
            optionsBuilder.UseNpgsql("Host=127.0.0.1;Port=5432;Database=tech-challenge;Username=postgres;Password=postgres");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
        => modelBuilder.ApplyConfigurationsFromAssembly(typeof(TechChallengeDbContext).Assembly);
}
