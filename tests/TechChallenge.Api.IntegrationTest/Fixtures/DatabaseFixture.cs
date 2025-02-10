using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TechChallenge.Domain.Entities;
using TechChallenge.Infrastructure.Database;
using Testcontainers.PostgreSql;


namespace TechChallenge.Api.IntegrationTest.Fixtures;
public class DatabaseFixture : IDisposable
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
            .WithImage("postgres:16.4-alpine3.20")
            .WithDatabase("tech-challenge-test")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .WithPortBinding(5432, 5432)
            .Build();

    public DatabaseFixture()
    {
        _dbContainer.StartAsync().Wait();
        

        var dbContext = GetDbContext();
        dbContext.Database.MigrateAsync().Wait();

        SeedAsync().Wait();
    }

    public async Task AddAsync<Tentity>(Tentity[] entities)
    {
        var dbContext = GetDbContext();
        await dbContext.AddRangeAsync(entities);
        await dbContext.SaveChangesAsync();
    }

    public async Task AddAsync<Tentity>(Tentity entity)
    {
        var dbContext = GetDbContext();
        await dbContext.AddAsync(entity!);
        await dbContext.SaveChangesAsync();
    }

    public async Task<TEntity?> SingleOrDefaultAsync<TEntity>(Expression<Func<TEntity, bool>> predicate, string[]? includeProperties = null) where TEntity : EntityBase
    {
        var set = GetDbContext().Set<TEntity>();
        var query = set.Where(predicate);
        query = IncludeProperties(query, includeProperties);
        return await query.SingleOrDefaultAsync();
    }

    public async Task SeedAsync()
    {
        var dbContext = GetDbContext();
        await dbContext.AddRangeAsync(ContactsSeed());
        await dbContext.SaveChangesAsync();
    }

    public static Contact[] ContactsSeed()
        => 
        [
            new Contact
            {
                Id = Guid.Parse("2464c839-95fb-42f2-8e09-14a17112576b"),
                Name = "John Doe",
                Email = "johndoe@mail.com",
                Phone = "47123456789",
                PhoneAreaCode = 47
            },
            new Contact
            {
                Id = Guid.Parse("e0ae41a8-0589-4fae-8183-bf7e160b3382"),
                Name = "Billy Joe",
                Email = "billyjoe@mail.com",
                Phone = "11987654321",
                PhoneAreaCode = 11
            }
        ];

    public DbContext GetDbContext()
    {
        var dbContextOptionsBuilder = new DbContextOptionsBuilder()
            .UseNpgsql(GetConnectionString());

        return new TechChallengeDbContext(dbContextOptionsBuilder.Options);
    }

    public string GetConnectionString()
        => _dbContainer.GetConnectionString();

    public void Dispose()
    {
        _dbContainer.DisposeAsync().GetAwaiter().GetResult();
        GC.SuppressFinalize(this);
    }

    private static IQueryable<TEntity> IncludeProperties<TEntity>(IQueryable<TEntity> query, string[]? includeProperties = null) where TEntity: EntityBase
    {
        if (includeProperties == null || includeProperties.Length == 0)
            return query;

        foreach (var property in includeProperties)
            query = query.Include(property);

        return query;
    }
}
