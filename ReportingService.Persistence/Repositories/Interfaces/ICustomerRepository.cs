using ReportingService.Persistence.Entities;

namespace ReportingService.Persistence.Repositories.Interfaces;

public interface ICustomerRepository
{
    Task ActivateAsync(Customer customer);
    Task<Guid> AddAsync(Customer customer);
    Task DeactivateAsync(Customer customer);
    Task<ICollection<Customer>?> GetAllAsync();
    Task<Customer?> GetByIdAsync(Guid id);
    Task<Customer?> GetByLoginAsync(string login);
    Task<Customer?> GetFullProfileByIdAsync(Guid id);
    Task UpdateAsync(Customer customer, Customer newCustomer);
    Task UpdateLoginAsync(Customer customer, string login);
    Task UpdatePasswordAsync(Customer customer, string password);
    Task<ICollection<Customer>> GetByBirthdayAsync(DateTime day);
    Task<ICollection<Customer>> GetByTransactionInPeriodCountAsync(
            int transactionsCount, DateTime DateStart, DateTime DateEnd);
    Task UpdateAmountWithoutDrawalRUBAsync(Customer customer, decimal newAmount);
    Task UpdateAmountWithDrawalRUBAsync(Customer customer, decimal newAmount);

}