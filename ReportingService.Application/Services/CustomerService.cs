using AutoMapper;
using ReportingService.Application.Exceptions;
using ReportingService.Application.Models;
using ReportingService.Core.Configuration;
using ReportingService.Persistence.Entities;
using ReportingService.Persistence.Repositories;
using ReportingService.Persistence.Repositories.Interfaces;

namespace ReportingService.Application.Services;

public class CustomerService(
        ICustomerRepository customerRepository,
        ITransactionRepository transactionRepository,
        IAccountRepository accountRepository,
        IMapper mapper) : ICustomerService
{
    public async Task<CustomerModel> AddCustomerAsync(CustomerModel customerModel)
    {
        var customer = await customerRepository.AddAndReturnAsync(mapper.Map<Customer>(customerModel));

        return mapper.Map<CustomerModel>(customer);
    }

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

        var accounts = await accountRepository.FindManyAsync(x => x.CustomerId == id);
        if (!accounts.Any())
        {
            throw new EntityNotFoundException($"Customer {id} have no accounts");
        }

        var transactions = await transactionRepository.FindManyAsync(x => x.CustomerId == id);

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

        var customer = await customerRepository.FindAsync(x => x.Accounts.Contains(account)) ??
            throw new EntityNotFoundException($"Customer with account {accountId} not found");

        var customerModel = mapper.Map<CustomerModel>(customer);

        return customerModel;
    }

    public async Task<CustomerModel> GetCustomerByTransactionIdAsync(Guid transactionId)
    {
        var transaction = await transactionRepository.GetByIdAsync(transactionId) ??
            throw new EntityNotFoundException($"Transaction {transactionId} not found");

        var customer = await customerRepository.FindAsync(x => x.Transactions.Contains(transaction)) ??
            throw new EntityNotFoundException($"Customer with transaction {transactionId} not found");

        var customerModel = mapper.Map<CustomerModel>(customer);

        return customerModel;
    }

    public async Task<IEnumerable<CustomerModel>> GetCustomersByBirthAsync(DateTime dateStart, DateTime dateEnd)
    {
        var customers = await customerRepository.FindManyAsync(x =>
            x.BirthDate.Date >= dateStart.Date &&
            x.BirthDate.Date <= dateEnd);

        var customerModels = mapper.Map<List<CustomerModel>>(customers);

        return customerModels;
    }

    public async Task<IEnumerable<CustomerModel>> GetCustomersAsync(
        int? transactionsCount, int? accountsCount, DateTime? dateStart, DateTime? dateEnd,
        List<Currency>? currencies, DateTime? birth)
    {
        var customers = await customerRepository.FindManyAsync(x =>
            transactionsCount == null || x.Transactions.Count >= transactionsCount &&
            accountsCount == null || x.Accounts.Count >= accountsCount &&
            dateStart == null || x.Transactions.Where(y => y.Date >= dateStart).Any() &&
            dateEnd == null || x.Transactions.Where(y => y.Date <= dateEnd).Any() &&
            currencies == null || x.Accounts.Where(y => currencies.Contains(y.Currency)).Any() &&
            birth == null || x.BirthDate.Day == birth.Value.Day && x.BirthDate.Month == birth.Value.Month);

        var customerModels = mapper.Map<List<CustomerModel>>(customers);

        return customerModels;
    }

    public async Task TransactionalAddCustomersAsync(List<CustomerModel> customerModels)
    {
        CheckAccounts(customerModels);

        var customers = mapper.Map<List<Customer>>(customerModels);

        customerRepository.TransactionalAddRangeAsync(customers);
    }

    //У кого запрашивать текущую базовую валюту?? Бросать экзепшн или удалять плохих клиентов из листа и возвращать только хороших?
    private async Task CheckAccounts(List<CustomerModel> customerModels)
    {
        var customersWithoutAccounts = customerModels.Where(x => !x.Accounts.Where(y => y.Currency == Currency.RUB).Any()).ToList();

        if (customersWithoutAccounts.Any())
        {
            throw new EntityConflictException("Customers with no Accounts in base Currency detected during the adding");
        }
    }
}
