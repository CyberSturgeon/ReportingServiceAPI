using Microsoft.EntityFrameworkCore;
using ReportingService.Persistence.Repositories.Interfaces;
using System.Linq.Expressions;


namespace ReportingService.Persistence.Repositories;

public class BaseRepository<TEntity>(ReportingContext Context) : IBaseRepository<TEntity>
    where TEntity : class
{
    protected readonly DbSet<TEntity> DbSet = Context.Set<TEntity>();
    public virtual async Task<TEntity?> GetByIdAsync(object id)
    {
        return await DbSet.FindAsync(id);
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await DbSet.ToListAsync();
    }

    public virtual async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await DbSet.Where(predicate).ToListAsync();
    }

    public virtual async Task AddAsync(TEntity entity)
    {
        await DbSet.AddAsync(entity);
        await Context.SaveChangesAsync();
    }

    public virtual async Task<TEntity> AddAndReturnAsync(TEntity entity)
    {
        await DbSet.AddAsync(entity);
        await Context.SaveChangesAsync();
        return entity;
    }

    public virtual async Task AddRangeAsync(IEnumerable<TEntity> entities)
    {
        await DbSet.AddRangeAsync(entities);
        await Context.SaveChangesAsync();
    }
    public async Task TransactionalAddRangeAsync(IEnumerable<TEntity> entities)
    {
        using var transaction = await Context.Database.BeginTransactionAsync();
        try
        {
            await DbSet.AddRangeAsync(entities);
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
        DbSet.Update(entity);
        await Context.SaveChangesAsync();
    }

    public virtual async Task RemoveAsync(TEntity entity)
    {
        DbSet.Remove(entity);
        await Context.SaveChangesAsync();
    }
}
