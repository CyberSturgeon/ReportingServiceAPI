using AutoMapper;
using ReportingService.Application.Models;
using ReportingService.Application.Services.Interfaces;
using ReportingService.Core.Configuration.Filters;
using ReportingService.Persistence.Entities;
using ReportingService.Persistence.Repositories;
using ReportingService.Persistence.Repositories.Interfaces;

namespace ReportingService.Application.Services
{
    public class TransactionService(
        ITransactionRepository transactionRepository,
        IMapper mapper) : ITransactionService
    {
        public async Task<List<TransactionModel>> SearchTransactionAsync(
            Guid customerId,
            TransactionSearchFilter dates)
        {
            var transactions = await transactionRepository.FindManyAsync(
                x => x.CustomerId == customerId
                && x.Date >= dates.DateFrom
                && x.Date <= dates.DateTo);

            var transactionModels = mapper.Map<List<TransactionModel>>(transactions)
                .OrderByDescending(x => x.Date)
                .ToList();

            return transactionModels;
        }

        public async Task<List<TransactionModel>> SearchTransactionByAccountAsync(Guid accountId)
        {
            var transactions = await transactionRepository.FindManyAsync(
                x => x.AccountId == accountId);

            var transactionModels = mapper.Map<List<TransactionModel>>(transactions)
                .OrderByDescending(x => x.Date)
                .ToList();

            return transactionModels;
        }

        public async Task<List<TransactionModel>> GetTransactionsByPeriodAsync(DateTimeFilter dates)
        {
            var transactions = await transactionRepository.FindManyAsync(x => x.Date >= dates.DateStart && x.Date < dates.DateEnd);
            var transactionModels = mapper.Map<List<TransactionModel>>(transactions).OrderBy(x => x.CustomerId).ToList();
            
            return transactionModels;
        }

        public async Task TransactionalAddAsync(List<TransactionModel> transactionModels)
        {
            var transactions = mapper.Map<List<Transaction>>(transactionModels);
            await transactionRepository.TransactionalAddRangeAsync(transactions);
        }
    }
}
