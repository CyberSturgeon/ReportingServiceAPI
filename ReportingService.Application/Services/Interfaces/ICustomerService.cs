using ReportingService.Application.Models;
using ReportingService.Core.Configuration;

namespace ReportingService.Application.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<CustomerModel> AddCustomerAsync(CustomerModel customerModel);
        Task<CustomerModel> GetCustomerByAccountIdAsync(Guid accountId);
        Task<CustomerModel> GetCustomerByIdAsync(Guid id);
        Task<CustomerModel> GetCustomerByTransactionIdAsync(Guid transactionId);
        Task<IEnumerable<CustomerModel>> GetCustomersAsync(CustomerFilter? filter);
        Task<IEnumerable<CustomerModel>> GetCustomersByBirthAsync(DateTime dateStart, DateTime dateEnd);
        Task<CustomerModel> GetFullCustomerByIdAsync(Guid id);
    }
}