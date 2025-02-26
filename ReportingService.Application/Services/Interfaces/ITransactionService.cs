using ReportingService.Application.Models;
using ReportingService.Core.Configuration.Filters;

namespace ReportingService.Application.Services.Interfaces
{
    public interface ITransactionService
    {
        Task<List<TransactionModel>> SearchTransaction(Guid customerId, TransactionSearchFilter dates);
        Task<List<TransactionModel>> SearchTransactionByAccount(Guid accountId);
        Task<List<TransactionModel>> GetTransactionsByPeriodAsync(DateTimeFilter dates);
    }
}