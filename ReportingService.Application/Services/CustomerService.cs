using AutoMapper;
using ReportingService.Application.Exceptions;
using ReportingService.Application.Models;
using ReportingService.Core.Configuration;
using ReportingService.Persistence.Repositories.Interfaces;

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

    public async Task<CustomerModel> GetCustomerByAccountIdAsync(Guid accountId)
    {
        var account = await accountRepository.GetByIdAsync(accountId) ??
            throw new EntityNotFoundException($"Account {accountId} not found");

        var customersList = await customerRepository.FindAsync(x => x.Accounts.Contains(account));
        if (!customersList.Any())
        {
            throw new EntityNotFoundException($"Customer with account {accountId} not found");

        }
        var customer = customersList.ToList().FirstOrDefault();
        
        var customerModel = mapper.Map<CustomerModel>(customer);

        return customerModel;
    }

    public async Task<CustomerModel> GetCustomerByTransactionIdAsync(Guid transactionId)
    {
        var transaction = await transactionRepository.GetByIdAsync(transactionId) ??
            throw new EntityNotFoundException($"Transaction {transactionId} not found");

        var customersList = await customerRepository.FindAsync(x => x.Transactions.Contains(transaction));
        if (!customersList.Any())
        {
            throw new EntityNotFoundException($"Customer with transaction {transactionId} not found");

        }
        var customer = customersList.ToList().FirstOrDefault();

        var customerModel = mapper.Map<CustomerModel>(customer);

        return customerModel;
    }

    public async Task<IEnumerable<CustomerModel>> GetCustomersAsync(
        int? transactionsCount, int? accountsCount, DateTime? dateStart, DateTime? dateEnd,
        List<Currency>? currencies, DateTime? birth)
    {
        var customers = await customerRepository.FindAsync(x =>
            transactionsCount == null || x.Transactions.Count >= transactionsCount &&
            accountsCount == null || x.Accounts.Count >= accountsCount &&
            dateStart == null || x.Transactions.Where(y => y.Date >= dateStart).Any() &&
            dateEnd == null || x.Transactions.Where(y => y.Date <= dateEnd).Any() &&
            !currencies.Any() || x.Accounts.Where(y => currencies.Contains(y.Currency)).Any() &&
            birth == null || x.BirthDate.Day == birth.Value.Day && x.BirthDate.Month == birth.Value.Month);

        var customerModels = mapper.Map<List<CustomerModel>>(customers);
        
        return customerModels;
    }
    //НУЖНА ПОМОЩЬ

    //public async Task<IEnumerable<CustomerModel>> GetCustomersByBirthAsync(DateTime birth)
    //{
    //    var customers = await customerRepository.FindAsync(x => 
    //        x.BirthDate.Day == birth.Day &&
    //        x.BirthDate.Month == birth.Month);

    //    var customerModels = mapper.Map<List<CustomerModel>>(customers);

    //    return customerModels;
    //}
}
