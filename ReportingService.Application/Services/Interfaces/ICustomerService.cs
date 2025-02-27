using ReportingService.Application.Models;
using ReportingService.Core.Configuration.Filters;
using ReportingService.Persistence.Entities;

namespace ReportingService.Application.Services.Interfaces
{
    public interface ICustomerService
    {
        Task BatchUpdateRoleAync(List<Guid> customerIds);
        Task AddAsync(CustomerModel customerModel);
        Task<CustomerModel> GetByAccountIdAsync(Guid accountId);
        Task<CustomerModel> GetByIdAsync(Guid id);
        Task<CustomerModel> GetByTransactionIdAsync(Guid transactionId);
        Task<List<CustomerModel>> GetByBirthAsync(DateFilter dates);
        Task<CustomerModel> GetFullByIdAsync(Guid id);
        Task TransactionalAddAsync(List<CustomerModel> customerModels);
        Task UpdateAsync(CustomerModel customerModel);
    }
}