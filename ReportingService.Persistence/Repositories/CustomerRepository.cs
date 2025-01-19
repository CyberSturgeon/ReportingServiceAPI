using Microsoft.EntityFrameworkCore;
using ReportingService.Persistence.Entities;
using ReportingService.Persistence.Repositories.Interfaces;
using ReportingService.Core.Configuration;

namespace ReportingService.Persistence.Repositories;

public class CustomerRepository(ReportingContext context) : ICustomerRepository
{
    public async Task<Customer?> GetByIdAsync(Guid id) => context.Customers
           .FirstOrDefault(x => x.Id == id);

    public async Task<Customer?> GetByLoginAsync(string login) => context.Customers
           .FirstOrDefault(x => x.Login == login);

    public async Task<Customer?> GetFullProfileByIdAsync(Guid id) => context.Customers
        .Include(x => x.Transactions).ThenInclude(y => y.Account)
        .Include(x => x.Accounts).ThenInclude(y => y.Customer)
        .FirstOrDefault(x => x.Id == id);

    public async Task<ICollection<Customer>?> GetAllAsync() =>
        context.Customers.Where(x => x.IsDeactivated == false).ToList();

    public async Task DeactivateAsync(Customer customer)
    {
        customer.IsDeactivated = true;

        await context.SaveChangesAsync();
    }

    public async Task ActivateAsync(Customer customer)
    {
        customer.IsDeactivated = false;

        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Customer customer, Customer newCustomer)
    {
        customer.Phone = newCustomer.Phone;
        customer.BirthDate = newCustomer.BirthDate;
        customer.Address = newCustomer.Address;
        customer.FirstName = newCustomer.FirstName;
        customer.LastName = newCustomer.LastName;

        await context.SaveChangesAsync();
    }

    public async Task UpdateLoginAsync(Customer customer, string login)
    {
        customer.Login = login;

        await context.SaveChangesAsync();
    }

    public async Task UpdatePasswordAsync(Customer customer, string password)
    {
        customer.Password = password;

        await context.SaveChangesAsync();
    }

    public async Task<Guid> AddAsync(Customer customer)
    {
        context.Customers.Add(customer);

        await context.SaveChangesAsync();

        return customer.Id;
    }

    public async Task<ICollection<Customer>> GetByBirthdayAsync(DateTime day) => context.Customers
        .Where(x => x.BirthDate == day).ToList();

    public async Task<ICollection<Customer>> GetByTransactionInPeriodCountAsync(
        int transactionsCount, DateTime DateStart, DateTime DateEnd) => context.Customers
            .Where(x => x.Transactions
                .Where(y => y.TransactionType != TransactionType.Withdrawal &&
                y.Date > DateStart &&
                y.Date < DateEnd)
            .ToList().Count>=transactionsCount).ToList();
    
    public async Task<ICollection<Customer>> GetByAccountDepositeDifferenceInPeriodAsync(
        decimal depDifference, DateTime DateStart, DateTime DateEnd) => context.Customers
            .Where(x => x.AmountWithoutDrawalRUB - x.AmountWithDrawalRUB > depDifference).ToList();

    public async Task UpdateAmountWithoutDrawalRUBAsync(Customer customer, decimal newAmount)
    {
        customer.AmountWithoutDrawalRUB = newAmount;

        await context.SaveChangesAsync();
    }

    public async Task UpdateAmountWithDrawalRUBAsync(Customer customer, decimal newAmount)
    {
        customer.AmountWithDrawalRUB = newAmount;

        await context.SaveChangesAsync();
    }
}
