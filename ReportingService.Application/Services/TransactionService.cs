using AutoMapper;
using ReportingService.Application.Models;
using ReportingService.Core;
using ReportingService.Persistence.Repositories.Interfaces;

namespace ReportingService.Application.Services
{
    public class TransactionService(
        ITransactionRepository transactionRepository,
        IMapper mapper)
    {
        public async Task<List<TransactionModel>> SearchTransaction(
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
    }
}
