using AutoMapper;
using Microsoft.Extensions.Logging;
using ReportingService.Application.Exceptions;
using ReportingService.Application.Models;
using ReportingService.Application.Services.Interfaces;
using ReportingService.Persistence.Entities;
using ReportingService.Persistence.Repositories;
using ReportingService.Persistence.Repositories.Interfaces;

namespace ReportingService.Application.Services;

public class AccountService(
    IAccountRepository accountRepository,
    IMapper mapper, ILogger<AccountService> logger) : IAccountService
{
    public async Task<List<AccountModel>> GetAccountsByCustomerIdAsync(
    Guid customerId)
    {
        logger.LogInformation($"Get account by customer {customerId}");

        var accounts = await accountRepository.FindManyAsync(x => x.CustomerId == customerId) ??
            throw new EntityNotFoundException($"Accounts by customer id {customerId} not found");

        var accountModels = mapper.Map<List<AccountModel>>(accounts);
        logger.LogInformation("SUCCESS");
        return accountModels;
    }

    public async Task TransactionalAddAsync(List<AccountModel> accountModels)
    {
        logger.LogInformation($"CREATE {accountModels.Count} account");

        var accounts = mapper.Map<List<Account>>(accountModels);
        accountRepository.TransactionalAddRangeAsync(accounts);
        logger.LogInformation("SUCCESS");
    }

    public async Task AddAsync(AccountModel accountModel)
    {
        logger.LogInformation($"CREATE {accountModel.Id} account");

        var account = mapper.Map<Account>(accountModel);
        accountRepository.AddAccountRawSqlAsync(account);
        logger.LogInformation("SUCCESS");
    }

    public async Task UpdateAsync(AccountModel accountModel)
    {
        logger.LogInformation($"UPDATE {accountModel.Id} account");

        var account = mapper.Map<Account>(accountModel);
        accountRepository.UpdateAccountRawSqlAsync(account);
        logger.LogInformation("SUCCESS");
    }
}
