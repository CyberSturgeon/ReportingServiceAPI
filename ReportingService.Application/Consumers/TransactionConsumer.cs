using AutoMapper;
using MassTransit;
using MYPBackendMicroserviceIntegrations.Messages;
using ReportingService.Application.Models;
using ReportingService.Application.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportingService.Application.Consumers
{
    public class TransactionConsumer(
        ITransactionService transactionService,
        IMapper mapper) : IConsumer<List<TransactionCreatedMessage>>
    {

        public async Task Consume(ConsumeContext<List<TransactionCreatedMessage>> context)
        {
            var transaction = context.Message;
            var transactionModels = mapper.Map<List<TransactionModel>>(transaction);

            await transactionService.TransactionalAddAsync(transactionModels);
        }
    }
}
