using ReportingService.Application.Models;
using ReportingService.Core.Configuration.Filters;

namespace ReportingService.Application.Services.Interfaces
{
    public interface ITransactionService
    {
        Task<List<TransactionModel>> SearchTransactionAsync(Guid customerId, TransactionSearchFilter dates);
        Task<List<TransactionModel>> SearchTransactionByAccountAsync(Guid accountId);
        Task<List<TransactionModel>> GetTransactionsByPeriodAsync(DateTimeFilter dates);
        Task TransactionalAddAsync(List<TransactionModel> transactionModels);
        Task AddAsync(TransactionModel transactionModel);
    }
}