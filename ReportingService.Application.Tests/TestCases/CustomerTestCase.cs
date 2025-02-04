using ReportingService.Application.Models;
using ReportingService.Persistence.Entities;

namespace ReportingService.Application.Tests.TestCases;

public static class CustomerTestCase
{
    public static Customer GetCustomerEntity(List<Account>? accounts, List<Transaction> transactions)
    {
        return new Customer
        {
            Id = Guid.NewGuid(),
            Role = Core.Configuration.Role.Regular,
            Phone = "test",
            Address = "test",
            Login = "test",
            Password = "test",
            BirthDate = DateTime.Now,
            FirstName = "test",
            LastName = "test",
            Accounts = accounts ?? [],
            IsDeactivated = false,
            Transactions = transactions ?? [],
        };
    }

    public static CustomerModel GetCustomerModel(List<AccountModel>? accountModels, List<TransactionModel> transactionModels)
    {
        return new CustomerModel
        {
            Id = Guid.NewGuid(),
            Role = Core.Configuration.Role.Regular,
            Phone = "test",
            Address = "test",
            Login = "test",
            Password = "test",
            BirthDate = DateTime.Now,
            FirstName = "test",
            LastName = "test",
            Accounts = accountModels ?? [],
            IsDeactivated = false,
            Transactions = transactionModels ?? [],
        };
    }
}
