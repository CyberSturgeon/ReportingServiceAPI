using ReportingService.Persistence.Entities;
using ReportingService.Persistence.Repositories.Interfaces;

namespace ReportingService.Persistence.Repositories
{
    public class TransactionRepository(ReportingContext context): BaseRepository<Comission>(context), ITransactionRepository
    {
    }
}
