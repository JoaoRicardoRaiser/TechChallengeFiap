using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using TechChallenge.DeleteContact.Domain.Entities;
using TechChallenge.DeleteContact.Domain.Interfaces;

namespace TechChallenge.DeleteContact.Infrastructure.Database.Repositories;

[ExcludeFromCodeCoverage]
public class Repository<TEntity>(DbContext dbContext) : IRepository<TEntity> where TEntity : EntityBase
{
    protected readonly DbSet<TEntity> _dbSet = dbContext.Set<TEntity>();

    public async Task<TEntity?> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, string[]? includeProperties = null)
    {
        var query = _dbSet.Where(predicate);
        IncludeProperties(ref query, includeProperties);
        return await query.SingleOrDefaultAsync();
    }

    public async Task AddAsync(TEntity entity)
        => await _dbSet.AddAsync(entity);

    public Task SaveChangesAsync()
        => dbContext.SaveChangesAsync();

    public void Delete(TEntity entity)
    => _dbSet.Remove(entity);

    private static IQueryable IncludeProperties(ref IQueryable<TEntity> query, string[]? includeProperties)
    {
        if (includeProperties == null)
            return query;

        foreach (var includeProperty in includeProperties)
            query = query.Include(includeProperty);

        return query;
    }
}
