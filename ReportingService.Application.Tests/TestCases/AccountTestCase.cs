using MYPBackendMicroserviceIntegrations.Enums;
using ReportingService.Application.Models;
using ReportingService.Persistence.Entities;

namespace ReportingService.Application.Tests.TestCases;

public static class AccountTestCase
{
    public static Account GetAccountEntity(Guid? customerId = null, List<Transaction>? transactions = null, Customer? customer = null)
    {
        return new Account
        {
            Id = Guid.NewGuid(),
            CustomerId = customerId ?? Guid.NewGuid(),
            DateCreated = DateTime.UtcNow,
            IsDeactivated = true,
            Currency = Currency.RUB,
            Transactions = transactions ?? [],
            Customer = customer ?? new(),
        };
    }

    public static AccountModel GetAccountModel(Guid? customerId = null, List<TransactionModel>? transactions = null, CustomerModel? customer = null)
    {
        return new AccountModel
        {
            Id = Guid.NewGuid(),
            CustomerId = customerId ?? Guid.NewGuid(),
            DateCreated = DateTime.UtcNow,
            IsDeactivated = true,
            Currency = Currency.RUB,
        };
    }
}
