using AutoMapper;
using ReportingService.Application.Models;
using ReportingService.Application.Services.Interfaces;
using ReportingService.Core.Configuration.Filters;
using ReportingService.Persistence.Entities;
using ReportingService.Persistence.Repositories.Interfaces;
using ReportingService.Application.Exceptions;

namespace ReportingService.Application.Services
{
    public class TransactionService(
        ITransactionRepository transactionRepository,
        IMapper mapper) : ITransactionService
    {
        public async Task<List<TransactionModel>> SearchAsync(
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

        public async Task<List<TransactionModel>> SearchByAccountAsync(Guid accountId)
        {
            var transactions = await transactionRepository.FindManyAsync(
                x => x.AccountId == accountId);

            var transactionModels = mapper.Map<List<TransactionModel>>(transactions)
                .OrderByDescending(x => x.Date)
                .ToList();

            return transactionModels;
        }

        public async Task<List<TransactionModel>> GetByPeriodAsync(DateTimeFilter dates)
        {
            dates.DateStart = DateTime.SpecifyKind(dates.DateStart, DateTimeKind.Utc);
            dates.DateEnd = DateTime.SpecifyKind(dates.DateEnd, DateTimeKind.Utc);

            var transactions = await transactionRepository.FindManyAsync(x => x.Date >= dates.DateStart && x.Date < dates.DateEnd);
            var transactionModels = mapper.Map<List<TransactionModel>>(transactions).OrderBy(x => x.CustomerId).ToList();
            
            return transactionModels;
        }

        public async Task TransactionalAddAsync(List<TransactionModel> transactionModels)
        {
            var transactions = mapper.Map<List<Transaction>>(transactionModels);
            await transactionRepository.TransactionalAddRangeAsync(transactions);
        }

        public async Task AddAsync(TransactionModel transactionModel)
        {
            try
            {
                var transaction = mapper.Map<Transaction>(transactionModel);
                await transactionRepository.AddTransactionRawSqlAsync(transaction);
            }
            catch (Exception ex)
            {
                throw new BadRabbitDataException($"ERROR {transactionModel.Id} problem");
            }
        }
    }
}
