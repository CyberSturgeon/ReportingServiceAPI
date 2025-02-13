using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ReportingService.Application.Exceptions;
using ReportingService.Application.Models;
using ReportingService.Application.Services.Interfaces;
using ReportingService.Core.Configuration;
using ReportingService.Persistence.Entities;
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
        var customer = await customerRepository.FindAsync(x => x.Id == id,
                        y => y.Include(x => x.Accounts).Include(x => x.Transactions)) ??
            throw new EntityNotFoundException($"Customer {id} not found");

        if(!customer.Accounts.Any())
        {
            throw new EntityNotFoundException($"No Accounts related to Customer {id}");
        }

        var customerModel = mapper.Map<CustomerModel>(customer);

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
    //НУЖНА ПОМОЩЬ

    //public async Task<IEnumerable<CustomerModel>> GetCustomersByBirthAsync(DateTime birth)
    //{
    //    var customers = await customerRepository.FindManyAsync(x => 
    //        x.BirthDate.Day == birth.Day &&
    //        x.BirthDate.Month == birth.Month);

    //    var customerModels = mapper.Map<List<CustomerModel>>(customers);

    //    return customerModels;
    //}
}
