using System.Linq.Expressions;
using TechChallenge.Domain.Entities;

namespace TechChallenge.Domain.Interfaces.Repositories;

public interface IRepository<TEntity> where TEntity : EntityBase
{
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate, string[]? includeProperties = null);
    Task<TEntity?> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, string[]? includeProperties = null);
    Task AddAsync(TEntity entity);
    void Delete(TEntity entity);
    Task SaveChangesAsync();
}
