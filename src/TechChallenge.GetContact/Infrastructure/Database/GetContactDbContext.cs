using Microsoft.EntityFrameworkCore;

namespace TechChallenge.GetContact.Infrastructure.Database
{
    public class GetContactDbContext(DbContextOptions dbContextOptions) : DbContext(dbContextOptions)
    {

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseNpgsql("Host=127.0.0.1;Port=5432;Database=tc-get-contact;Username=postgres;Password=postgres");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(GetContactDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
