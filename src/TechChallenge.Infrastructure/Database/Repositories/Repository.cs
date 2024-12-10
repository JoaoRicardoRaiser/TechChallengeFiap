using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TechChallenge.Domain.Entities;
using TechChallenge.Domain.Interfaces.Repositories;

namespace TechChallenge.Infrastructure.Database.Repositories;

public class Repository<TEntity>(DbContext dbContext) : IRepository<TEntity> where TEntity : EntityBase
{
    protected readonly DbSet<TEntity> _dbSet = dbContext.Set<TEntity>();

    public async Task<IEnumerable<TEntity>> GetAllAsync()
        => await _dbSet.ToListAsync();

    public async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate, string[]? includeProperties = null)
    {
        var query = _dbSet.Where(predicate);
        IncludeProperties(ref query, includeProperties);
        return await query.ToListAsync();
    }

    public async Task AddAsync(TEntity entity)
        => await _dbSet.AddAsync(entity);

    public void Delete(TEntity entity)
        => _dbSet.Remove(entity);

    public Task SaveChangesAsync()
        => dbContext.SaveChangesAsync();

    private static IQueryable IncludeProperties(ref IQueryable<TEntity> query, string[]? includeProperties)
    {
        if (includeProperties == null)
            return query;

        foreach (var includeProperty in includeProperties)
            query = query.Include(includeProperty);

        return query;
    }
}
