using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TechChallenge.DeleteContact.Domain.Entities;
using TechChallenge.DeleteContact.Infrastructure.Database;
using Testcontainers.PostgreSql;

namespace TechChallenge.DeleteContact.IntegrationTest.Fixtures;

public class DatabaseFixture : IDisposable
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
            .WithImage("postgres:16.4-alpine3.20")
            .WithDatabase("tc-delete-contact-test")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .WithPortBinding(6543, 5432)
            .Build();

    public DatabaseFixture()
    {
        _dbContainer.StartAsync().Wait();


        var dbContext = GetDbContext();
        dbContext.Database.MigrateAsync().Wait();
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

    public DbContext GetDbContext()
    {
        var dbContextOptionsBuilder = new DbContextOptionsBuilder()
            .UseNpgsql(GetConnectionString());

        return new DeleteContactDbContext(dbContextOptionsBuilder.Options);
    }

    public string GetConnectionString()
        => _dbContainer.GetConnectionString();

    public void Dispose()
    {
        _dbContainer.DisposeAsync().GetAwaiter().GetResult();
        GC.SuppressFinalize(this);
    }

    private static IQueryable<TEntity> IncludeProperties<TEntity>(IQueryable<TEntity> query, string[]? includeProperties = null) where TEntity : EntityBase
    {
        if (includeProperties == null || includeProperties.Length == 0)
            return query;

        foreach (var property in includeProperties)
            query = query.Include(property);

        return query;
    }
}
