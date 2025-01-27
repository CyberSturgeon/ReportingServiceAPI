
using AutoMapper;
using ReportingService.Application.Exceptions;
using ReportingService.Application.Models;
using ReportingService.Core.Configuration;
using ReportingService.Persistence.Repositories.Interfaces;
using System.Linq;

namespace ReportingService.Application.Services;

public class CustomerService (
        ICustomerRepository customerRepository,
        ITransactionRepository transactionRepository,
        IAccountRepository accountRepository,
        IMapper mapper)
{
    public async Task<CustomerModel> GetCustomerByIdAsync(Guid id)
    {
        var customer = await customerRepository.GetByIdAsync(id) ??
            throw new EntityNotFoundException($"Customer {id} not found");

        var customerModel = mapper.Map<CustomerModel>(customer);

        return customerModel;
    }

    public async Task<CustomerModel> GetFullCustomerByIdAsync(Guid id)
    {
        var customer = await customerRepository.GetByIdAsync(id) ??
            throw new EntityNotFoundException($"Customer {id} not found");

        var accounts = await accountRepository.FindAsync(x => x.CustomerId == id);
        if(!accounts.Any())
        {
            throw new EntityNotFoundException($"Customer {id} have no accounts");
        }

        var transactions = await transactionRepository.FindAsync(x => x.CustomerId == id);

        var customerModel = mapper.Map<CustomerModel>(customer);

        var accountModels = mapper.Map<List<AccountModel>>(accounts)
                .OrderByDescending(x => x.DateCreated)
                .ToList();

        var transactionModels = mapper.Map<List<TransactionModel>>(transactions)
            .OrderByDescending(x => x.Date)
            .ToList();

        customerModel.Accounts = accountModels;
        customerModel.Transactions = transactionModels;

        return customerModel;
    }

    public async Task<CustomerModel> GetCustomerByAccountIdAsync(Guid id)
    {
        var account = await accountRepository.GetByIdAsync(id) ??
            throw new EntityNotFoundException($"Account {id} not found");

        var customersList = await customerRepository.FindAsync(x => x.Accounts.Contains(account));
        if (!customersList.Any())
        {
            throw new EntityNotFoundException($"Customer with account {id} not found");

        }
        var customer = customersList.ToList().FirstOrDefault();
        
        var customerModel = mapper.Map<CustomerModel>(customer);

        return customerModel;
    }
    //public async Task<IEnumerable<CustomerModel>> GetCustomers(
    //    int? transactionsCount, int? accountsCount, DateTime? dateStart, DateTime? dateEnd,
    //    List<Currency>? currencies)
    //{
    //    var customers = await customerRepository.FindAsync()
    //}
    //НУЖНА ПОМОЩЬ
}
