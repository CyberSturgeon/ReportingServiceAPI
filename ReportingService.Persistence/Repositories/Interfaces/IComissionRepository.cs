using ReportingService.Persistence.Entities;

namespace ReportingService.Persistence.Repositories.Interfaces;

public interface IComissionRepository
{
    Task<Guid> AddAsync(Comission comission);
    Task<ICollection<Comission>?> GetAllAsync();
    Task<ICollection<Comission>?> GetAllScopedAsync(DateTime DateStart, DateTime DateEnd);
    Task<Comission?> GetByIdAsync(Guid id);
    Task<Comission?> GetFullProfileByIdAsync(Guid id);
}