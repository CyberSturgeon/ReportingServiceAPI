using ReportingService.Application.Models;
using ReportingService.Persistence.Entities;

namespace ReportingService.Application.Tests.TestCases;

public static class TransactionTestCase
{
    public static Transaction GetTransactionEntity(Guid? accountId = null, Guid? customerId = null)
    {
        return new Transaction
        {
            Id = Guid.NewGuid(),
            AccountId = accountId ?? Guid.NewGuid(),
            CustomerId = customerId ?? Guid.NewGuid(),
            Amount = 1,
            AmountRUB = 1,
            TransactionType = Core.Configuration.TransactionType.Deposit,
            Currency = Core.Configuration.Currency.RUB,
            Date = DateTime.UtcNow,
        };
    }

    public static TransactionModel GetTransactionModel(Guid? accountId = null, Guid? customerId = null)
    {
        return new TransactionModel
        {
            Id = Guid.NewGuid(),
            AccountId = accountId ?? Guid.NewGuid(),
            CustomerId = customerId ?? Guid.NewGuid(),
            Amount = 1,
            AmountRUB = 1,
            TransactionType = Core.Configuration.TransactionType.Deposit,
            Currency = Core.Configuration.Currency.RUB,
            Date = DateTime.UtcNow,
        };
    }
}
