using Microsoft.EntityFrameworkCore;

namespace TechChallenge.UpdateContact.Infrastructure.Database
{
    public class UpdateContactDbContext(DbContextOptions dbContextOptions) : DbContext(dbContextOptions)
    {

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseNpgsql("Host=127.0.0.1;Port=5432;Database=update-contact;Username=postgres;Password=postgres");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UpdateContactDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
