using ReportingService.Application.Models;
using ReportingService.Core.Configuration.Filters;

namespace ReportingService.Application.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<CustomerModel> AddCustomerAsync(CustomerModel customerModel);
        Task<CustomerModel> GetCustomerByAccountIdAsync(Guid accountId);
        Task<CustomerModel> GetCustomerByIdAsync(Guid id);
        Task<CustomerModel> GetCustomerByTransactionIdAsync(Guid transactionId);
        Task<List<CustomerModel>> GetCustomersAsync(CustomerFilter? filter);
        Task<List<CustomerModel>> GetCustomersByBirthAsync(DateTime dateStart, DateTime dateEnd);
        Task<CustomerModel> GetFullCustomerByIdAsync(Guid id);
    }
}