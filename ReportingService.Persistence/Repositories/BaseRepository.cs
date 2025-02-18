using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using ReportingService.Persistence.Repositories.Interfaces;
using System.Linq.Expressions;


namespace ReportingService.Persistence.Repositories;

public class BaseRepository<TEntity>(ReportingContext Context) : IBaseRepository<TEntity>
    where TEntity : class
{
    protected readonly DbSet<TEntity> _dbSet = Context.Set<TEntity>();
    public virtual async Task<TEntity?> GetByIdAsync(object id)
    {
        return await _dbSet.FindAsync(id);
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public virtual async Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null)
    {
        IQueryable<TEntity> query = _dbSet;

        if (include != null)
        {
            query = include(query);
        }

        return await query.SingleOrDefaultAsync(predicate);
    }

    public virtual async Task<IEnumerable<TEntity>> FindManyAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _dbSet.Where(predicate).ToListAsync();
    }

    public virtual async Task AddAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
        await Context.SaveChangesAsync();
    }

    public virtual async Task<TEntity> AddAndReturnAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
        await Context.SaveChangesAsync();
        return entity;
    }

    public virtual async Task AddRangeAsync(IEnumerable<TEntity> entities)
    {
        await _dbSet.AddRangeAsync(entities);
        await Context.SaveChangesAsync();
    }
    public async Task TransactionalAddRangeAsync(IEnumerable<TEntity> entities)
    {
        using var transaction = await Context.Database.BeginTransactionAsync();
        try
        {
            await _dbSet.AddRangeAsync(entities);
            await Context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public virtual async Task UpdateAsync(TEntity entity)
    {
        _dbSet.Update(entity);
        await Context.SaveChangesAsync();
    }

    public virtual async Task RemoveAsync(TEntity entity)
    {
        _dbSet.Remove(entity);
        await Context.SaveChangesAsync();
    }
}
