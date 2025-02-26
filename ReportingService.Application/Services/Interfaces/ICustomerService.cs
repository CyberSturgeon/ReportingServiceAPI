using ReportingService.Application.Models;
using ReportingService.Core.Configuration.Filters;
using ReportingService.Persistence.Entities;

namespace ReportingService.Application.Services.Interfaces
{
    public interface ICustomerService
    {
        Task BatchUpdateRoleAync(List<Guid> customerIds);
        Task AddAsync(CustomerModel customerModel);
        Task<CustomerModel> GetCustomerByAccountIdAsync(Guid accountId);
        Task<CustomerModel> GetCustomerByIdAsync(Guid id);
        Task<CustomerModel> GetCustomerByTransactionIdAsync(Guid transactionId);
        Task<List<CustomerModel>> GetCustomersByBirthAsync(DateFilter dates);
        Task<CustomerModel> GetFullCustomerByIdAsync(Guid id);
        Task TransactionalAddCustomersAsync(List<CustomerModel> customerModels);
    }
}