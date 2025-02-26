using ReportingService.Persistence.Entities;

namespace ReportingService.Persistence.Repositories.Interfaces
{
    public interface IAccountRepository : IBaseRepository<Account>
    {
        Task AddAccountRawSqlAsync(Account account);
    }
}
