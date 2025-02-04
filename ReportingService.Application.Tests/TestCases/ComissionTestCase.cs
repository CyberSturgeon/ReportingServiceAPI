using ReportingService.Application.Models;
using ReportingService.Persistence.Entities;


namespace ReportingService.Application.Tests.TestCases;

public static class ComissionTestCase
{
    public static Comission GetComissionEntity(Guid? transactionId, Transaction? transaction)
    {
        return new Comission
        {
            Id = Guid.NewGuid(),
            TransactionId = transactionId ?? Guid.NewGuid(),
            Transaction = transaction ?? new(),
            Income = 1,
        };
    }

    public static ComissionModel GetComissionModel(Guid? transactionId, TransactionModel? transaction)
    {
        return new ComissionModel
        {
            Id = Guid.NewGuid(),
            TransactionId = transactionId ?? Guid.NewGuid(),
            Transaction = transaction ?? new(),
            Income = 1,
        };
    }
}
