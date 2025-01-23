using System.Linq.Expressions;

namespace ReportingService.Persistence.Repositories.Interfaces
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        Task<TEntity> AddAndReturnAsync(TEntity entity);
        Task AddAsync(TEntity entity);
        Task AddRangeAsync(IEnumerable<TEntity> entities);
        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity?> GetByIdAsync(object id);
        Task RemoveAsync(TEntity entity);
        Task TransactionalAddRangeAsync(IEnumerable<TEntity> entities);
        Task UpdateAsync(TEntity entity);
    }
}