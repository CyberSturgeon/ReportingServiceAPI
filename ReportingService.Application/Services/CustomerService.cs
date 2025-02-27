using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MYPBackendMicroserviceIntegrations.Enums;
using ReportingService.Application.Exceptions;
using ReportingService.Application.Models;
using ReportingService.Application.Services.Interfaces;
using ReportingService.Core.Configuration.Filters;
using ReportingService.Persistence.Entities;
using ReportingService.Persistence.Repositories.Interfaces;

namespace ReportingService.Application.Services;

public class CustomerService(
        ICustomerRepository customerRepository,
        ITransactionRepository transactionRepository,
        IAccountRepository accountRepository,
        IMapper mapper, ILogger<CustomerService> logger) : ICustomerService
{
    public async Task AddAsync(CustomerModel customerModel)
    {
        try
        {
            logger.LogInformation($"CREATE customer {LogHelper.HideEmail(customerModel.Email)}");
            await customerRepository.AddCustomerRawSqlAsync(mapper.Map<Customer>(customerModel));
            logger.LogInformation("SUCCESS");
        }
        catch (Exception ex)
        {
            throw new BadRabbitDataException($"ERROR {customerModel.CustomerServiceId} problem");
        }
    }

    public async Task<CustomerModel> GetByIdAsync(Guid id)
    {
        logger.LogInformation($"GET customer {id}");
        var customer = await customerRepository.GetByIdAsync(id) ??
            throw new EntityNotFoundException($"Customer {id} not found");

        var customerModel = mapper.Map<CustomerModel>(customer);
        logger.LogInformation("SUCCESS");
        return customerModel;
    }

    public async Task UpdateAsync(CustomerModel customerModel)
    {
        try
        {
            logger.LogInformation($"UPDATE customer {LogHelper.HideEmail(customerModel.Email)}");
            await customerRepository.UpdateCustomerRawSqlAsync(mapper.Map<Customer>(customerModel));
            logger.LogInformation("SUCCESS");
        }
        catch (Exception ex)
        {
            throw new BadRabbitDataException($"ERROR {customerModel.Id} problem");
        }
    }

    public async Task<CustomerModel> GetFullByIdAsync(Guid id)
    {
        logger.LogInformation($"GET full customer {id}");
        var customer = await customerRepository.FindAsync(x => x.Id == id,
                        y => y.Include(x => x.Accounts).Include(x => x.Transactions)) ??
            throw new EntityNotFoundException($"Customer {id} not found");

        if(!customer.Accounts.Any())
        {
            throw new EntityNotFoundException($"No Accounts related to Customer {id}");
        }

        var customerModel = mapper.Map<CustomerModel>(customer);
        logger.LogInformation("SUCCESS");
        return customerModel;
    }

    public async Task<CustomerModel> GetByAccountIdAsync(Guid accountId)
    {
        logger.LogInformation($"Get customer by account {accountId}");
        var account = await accountRepository.GetByIdAsync(accountId) ??
            throw new EntityNotFoundException($"Account {accountId} not found");

        var customer = await customerRepository.FindAsync(x => x.Accounts.Contains(account)) ??
            throw new EntityNotFoundException($"Customer with account {accountId} not found");

        var customerModel = mapper.Map<CustomerModel>(customer);
        logger.LogInformation("SUCCESS");
        return customerModel;
    }

    public async Task<CustomerModel> GetByTransactionIdAsync(Guid transactionId)
    {
        logger.LogInformation($"GET customer by transaction {transactionId}");
        var transaction = await transactionRepository.GetByIdAsync(transactionId) ??
            throw new EntityNotFoundException($"Transaction {transactionId} not found");

        var customer = await customerRepository.FindAsync(x => x.Transactions.Contains(transaction)) ??
            throw new EntityNotFoundException($"Customer with transaction {transactionId} not found");

        var customerModel = mapper.Map<CustomerModel>(customer);
        logger.LogInformation("SUCCESS");
        return customerModel;
    }

    public async Task<List<CustomerModel>> GetByBirthAsync(DateFilter dates)
    {
        logger.LogInformation($"GET customers by birth {dates.DateStart} - {dates.DateEnd}");
        var customers = await customerRepository.FindManyAsync(x =>
            (dates.DateStart.Month<dates.DateEnd.Month && x.BirthDate.Month == dates.DateStart.Month && x.BirthDate.Day >= dates.DateStart.Day) ||
            (dates.DateStart.Month < dates.DateEnd.Month && x.BirthDate.Month == dates.DateEnd.Month && x.BirthDate.Day <= dates.DateEnd.Day) ||
            (dates.DateStart.Month == dates.DateEnd.Month && x.BirthDate.Month == dates.DateStart.Month && x.BirthDate.Day >= dates.DateStart.Day && x.BirthDate.Day <= dates.DateEnd.Day) ||
            (x.BirthDate.Month>dates.DateStart.Month && x.BirthDate.Month<dates.DateEnd.Month));

        var customerModels = mapper.Map<List<CustomerModel>>(customers);
        logger.LogInformation($"SUCCESS {customerModels.Count} returned");
        return customerModels;
    }

    public async Task TransactionalAddAsync(List<CustomerModel> customerModels)
    {
        logger.LogInformation($"CREATE {customerModels.Count} customers ");
        CheckAccounts(customerModels);

        var customers = mapper.Map<List<Customer>>(customerModels);
        customerRepository.TransactionalAddRangeAsync(customers);
        logger.LogInformation("SUCCESS");
    }

    public async Task BatchUpdateRoleAync(List<Guid> customerIds)
    {
        try
        {
            logger.LogInformation($"UPDATE role for {customerIds.Count} customers ");
            await customerRepository.BatchUpdateRoleAsync(customerIds);
            logger.LogInformation("SUCCESS");
        }
        catch (Exception ex)
        {
            throw new BadRabbitDataException($"ERROR problem");
        }
    }

    private async Task CheckAccounts(List<CustomerModel> customerModels)
    {
        logger.LogInformation($"CHECK accounts for {customerModels.Count} customers");
        var customersWithoutAccounts = customerModels.Where(x => !x.Accounts.Where(y => y.Currency == Currency.RUB).Any()).ToList();

        if (customersWithoutAccounts.Any())
        {
            throw new EntityConflictException("Customers with no RUB Accounts detected during the adding");
        }
        logger.LogInformation($"SUCESS passed {customersWithoutAccounts.Count} customers");
    }
}
