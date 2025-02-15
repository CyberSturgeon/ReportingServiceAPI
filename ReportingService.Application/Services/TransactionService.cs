using AutoMapper;
using ReportingService.Application.Models;
using ReportingService.Persistence.Repositories.Interfaces;

namespace ReportingService.Application.Services
{
    public class TransactionService(
        ITransactionRepository transactionRepository,
        IMapper mapper)
    {
        public async Task<List<TransactionModel>> GetTransactionsByCustomerId(
            Guid customerId,
            DateTime dateFrom,
            DateTime dateTo)
        {
            var transactions = await transactionRepository.FindManyAsync(
                x => x.CustomerId == customerId
                && x.Date >= dateFrom
                && x.Date <= dateTo);

            var transactionModels = mapper.Map<List<TransactionModel>>(transactions)
                .OrderByDescending(x => x.Date)
                .ToList();

            return transactionModels;
        }
    }
}
