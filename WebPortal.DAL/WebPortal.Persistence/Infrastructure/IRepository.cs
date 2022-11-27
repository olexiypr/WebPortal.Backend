using System.Linq.Expressions;

namespace WebPortal.Persistence.Infrastructure;

public interface IRepository<TEntity>
    where TEntity : class
{
    IQueryable<TEntity> Query(params Expression<Func<TEntity, object>>[] includes);

    Task<TEntity> AddAsync(TEntity entity);

    Task AddRangeAsync(IEnumerable<TEntity> entities);

    Task DeleteRangeAsync(IEnumerable<TEntity> entities);

    Task<TEntity> UpdateAsync(TEntity entity);

    Task UpdateRangeAsync(IEnumerable<TEntity> entities);

    Task<int> SaveChangesAsync();

    ValueTask<TEntity> GetByIdAsync(params object[] keys);

    void Delete(TEntity entity);

    void Detach(TEntity entity);
}