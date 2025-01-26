using ReportingService.Persistence.Entities;
using ReportingService.Persistence.Repositories.Interfaces;

namespace ReportingService.Persistence.Repositories
{
    public class AccountRepository(ReportingContext context)
        : BaseRepository<Account>(context), IAccountRepository
    {
    }
}
