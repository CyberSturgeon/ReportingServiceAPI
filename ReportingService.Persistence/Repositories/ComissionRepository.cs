
using ReportingService.Persistence.Entities;
using ReportingService.Persistence.Repositories.Interfaces;

namespace ReportingService.Persistence.Repositories;

public class ComissionRepository(ReportingContext context) 
        : BaseRepository<Comission>(context), IComissionRepository
{
}
