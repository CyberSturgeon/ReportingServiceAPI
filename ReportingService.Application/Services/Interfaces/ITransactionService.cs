using ReportingService.Application.Models;
using ReportingService.Core.Configuration.Filters;

namespace ReportingService.Application.Services.Interfaces
{
    public interface ITransactionService
    {
        Task<List<TransactionModel>> SearchAsync(Guid customerId, TransactionSearchFilter dates);
        Task<List<TransactionModel>> SearchByAccountAsync(Guid accountId);
        Task<List<TransactionModel>> GetByPeriodAsync(DateTimeFilter dates);
        Task TransactionalAddAsync(List<TransactionModel> transactionModels);
        Task AddAsync(TransactionModel transactionModel);
    }
}