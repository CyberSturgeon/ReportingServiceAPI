using ReportingService.Application.Models;

namespace ReportingService.Application.Services.Interfaces
{
    public interface IAccountService
    {
        Task<List<AccountModel>> GetAccountsByCustomerIdAsync(Guid customerId);
        Task TransactionalAddAsync(List<AccountModel> accountModels);
    }
}