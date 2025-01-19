using Microsoft.EntityFrameworkCore;
using ReportingService.Persistence.Entities;

namespace ReportingService.Persistence.Repositories;

public class ComissionRepository(ReportingContext context)
{
    public async Task<Comission?> GetByIdAsync(Guid id) => context.Comissions
           .FirstOrDefault(x => x.Id == id);

    public async Task<Comission?> GetFullProfileByIdAsync(Guid id) => context.Comissions
        .Include(x => x.Transaction).ThenInclude(y => y.Account)
        .FirstOrDefault(x => x.Id == id);

    public async Task<ICollection<Comission>?> GetAllAsync() =>
        context.Comissions.ToList();

    public async Task<ICollection<Comission>?> GetAllScopedAsync(DateTime DateStart, DateTime DateEnd) =>
        context.Comissions.Where(x => x.Date>DateStart && x.Date<DateEnd).ToList();

    public async Task<Guid> AddAsync(Comission comission)
    {
        context.Comissions.Add(comission);

        await context.SaveChangesAsync();

        return comission.Id;
    }
}
