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
        Task<IEnumerable<CustomerModel>> GetCustomersAsync(int? transactionsCount, int? accountsCount, DateTime? dateStart, DateTime? dateEnd, List<Currency>? currencies, DateTime? birth);
        Task<IEnumerable<CustomerModel>> GetCustomersByBirthAsync(DateTime dateStart, DateTime dateEnd);
        Task<CustomerModel> GetFullCustomerByIdAsync(Guid id);
    }
}