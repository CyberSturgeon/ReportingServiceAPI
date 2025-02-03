using ReportingService.Application.Models;
using ReportingService.Persistence.Entities;

namespace ReportingService.Application.Tests.TestCases;

public static class AccountTestCase
{
    public static Account GetAccountEntity(Guid? customerId, List<Transaction>? transactions, Customer? customer)
    {
        return new Account
        {
            Id = Guid.NewGuid(),
            CustomerId = customerId ?? Guid.NewGuid(),
            DateCreated = DateTime.UtcNow,
            Status = true,
            Currency = Core.Configuration.Currency.RUB,
            Transactions = transactions ?? [],
            Customer = customer ?? new(),
        };
    }

    public static AccountModel GetAccountModel(Guid? customerId, List<TransactionModel>? transactions, CustomerModel? customer)
    {
        return new AccountModel
        {
            Id = Guid.NewGuid(),
            CustomerId = customerId ?? Guid.NewGuid(),
            DateCreated = DateTime.UtcNow,
            Status = true,
            Currency = Core.Configuration.Currency.RUB,
            Transactions = transactions ?? [],
            Customer = customer ?? new(),
        };
    }
}
