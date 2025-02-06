using AutoMapper;
using ReportingService.Application.Exceptions;
using ReportingService.Application.Models;
using ReportingService.Persistence.Entities;
using ReportingService.Persistence.Repositories;
using ReportingService.Persistence.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportingService.Application.Services
{
    public class TransactionService(
        ITransactionRepository transactionRepository,
        IMapper mapper)
    {
        public async Task<List<TransactionModel>> GetAllTransactionsByCustomerId(Guid customerId)
        {
            var transactions = await transactionRepository.FindManyAsync(x => x.CustomerId == customerId);
            
            var transactionModel = mapper.Map<List<TransactionModel>>(transactions)
            .OrderByDescending(x => x.Date)
            .ToList();

            return transactionModel;
        }
    }
}
