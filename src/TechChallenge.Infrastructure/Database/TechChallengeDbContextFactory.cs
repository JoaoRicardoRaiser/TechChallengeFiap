using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace TechChallenge.Infrastructure.Database;

public class TechChallengeDbContextFactory : IDesignTimeDbContextFactory<TechChallengeDbContext>
{
    private const string DefaultConnectionString = "Host=127.0.0.1;Port=5432;Database=tech-challenge;Username=postgres;Password=postgres";

    TechChallengeDbContext IDesignTimeDbContextFactory<TechChallengeDbContext>.CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<TechChallengeDbContext>();
        optionsBuilder.UseNpgsql(DefaultConnectionString);

        return new TechChallengeDbContext(optionsBuilder.Options);
    }
}
