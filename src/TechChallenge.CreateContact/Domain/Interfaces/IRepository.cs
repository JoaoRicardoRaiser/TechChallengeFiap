using System.Linq.Expressions;
using TechChallenge.CreateContact.Domain.Entities;

namespace TechChallenge.CreateContact.Domain.Interfaces;

public interface IRepository<TEntity> where TEntity : EntityBase
{
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<TEntity?> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, string[]? includeProperties = null);
    Task AddAsync(TEntity entity);
    Task SaveChangesAsync();
}
