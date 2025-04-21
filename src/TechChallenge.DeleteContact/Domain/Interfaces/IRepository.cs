using System.Linq.Expressions;
using TechChallenge.DeleteContact.Domain.Entities;

namespace TechChallenge.DeleteContact.Domain.Interfaces;

public interface IRepository<TEntity> where TEntity : EntityBase
{
    Task<TEntity?> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, string[]? includeProperties = null);
    Task AddAsync(TEntity entity);
    void Delete(TEntity entity);
    Task SaveChangesAsync();
}

