using System.Linq.Expressions;
using TechChallenge.UpdateContact.Domain.Entities;

namespace TechChallenge.UpdateContact.Domain.Interfaces;

public interface IRepository<TEntity> where TEntity : EntityBase
{
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<TEntity?> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, string[]? includeProperties = null);
    Task AddAsync(TEntity entity);
    void Delete(TEntity entity);
    Task SaveChangesAsync();
}
