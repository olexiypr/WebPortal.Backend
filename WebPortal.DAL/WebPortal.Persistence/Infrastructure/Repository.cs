using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using WebPortal.Persistence.Context;
using WebPortal.Persistence.Exceptions;

namespace WebPortal.Persistence.Infrastructure;

public class Repository<TEntity> : IRepository<TEntity>
    where TEntity : class
{
    private readonly WebPortalDbContext _dbContext;
    private readonly DbSet<TEntity> _dbEntities;

    public Repository(WebPortalDbContext dbContext)
    {
        _dbContext = dbContext;
        _dbEntities = _dbContext.Set<TEntity>();
    }
    public IQueryable<TEntity> Query(params Expression<Func<TEntity, object>>[] includes)
    {
        var dbSet = _dbContext.Set<TEntity>();
        var query = includes
            .Aggregate<Expression<Func<TEntity, object>>, IQueryable<TEntity>>(dbSet, (current, include) => current.Include(include));

        return query ?? dbSet;
    }

    public async Task<TEntity> AddAsync(TEntity entity)
    {
        CheckEntityForNull(entity);
        return (await _dbEntities.AddAsync(entity)).Entity;
    }

    public async Task AddRangeAsync(IEnumerable<TEntity> entities) => await _dbEntities.AddRangeAsync(entities);

    public async Task DeleteRangeAsync(IEnumerable<TEntity> entities) =>
        await Task.Run(() => entities.ToList().ForEach(item => _dbContext.Entry(item).State = EntityState.Deleted));

    public async Task<TEntity> UpdateAsync(TEntity entity) =>
        await Task.Run(() => _dbEntities.Update(entity).Entity);

    public async Task UpdateRangeAsync(IEnumerable<TEntity> entities) =>
        await Task.Run(() => _dbEntities.UpdateRange(entities));

    public async Task<int> SaveChangesAsync()
    {
        try
        {
            return await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            if (_dbContext.Database.CurrentTransaction != null)
            {
                await _dbContext.Database.CurrentTransaction.RollbackAsync();
            }

            throw;
        }
    }

    public async ValueTask<TEntity> GetByIdAsync(params object[] keys) => 
        await _dbEntities.FindAsync(keys) ?? throw new NotFoundException(_dbEntities.EntityType.ToString(), keys.ToString());
    
    public void Delete(TEntity entity) => _dbContext.Entry(entity).State = EntityState.Deleted;

    public void Detach(TEntity entity) => _dbContext.Entry(entity).State = EntityState.Detached;

    private static void CheckEntityForNull(TEntity entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity), "The entity to add cannot be null.");
        }
    }
}