using AutoMapper;
using MassTransit;
using Microsoft.Extensions.Logging;
using MYPBackendMicroserviceIntegrations.Messages;
using ReportingService.Application.Models;
using ReportingService.Application.Services;
using ReportingService.Application.Services.Interfaces;

namespace ReportingService.Application.Consumers
{
    public class TransactionConsumer(
        ITransactionService transactionService,
        ILogger<TransactionConsumer> logger,
        IMapper mapper) : IConsumer<TransactionCreatedMessage>
    {

        public async Task Consume(ConsumeContext<TransactionCreatedMessage> context)
        {
            logger.LogInformation($"CONSUME {context.Message.Id} transaction");
            var transaction = context.Message;
            var transactionModel = mapper.Map<TransactionModel>(transaction);
            
            await transactionService.AddAsync(transactionModel);

            //await transactionService.TransactionalAddAsync(transactionModel);
            logger.LogInformation($"UPDATE {context.Message.Id} transaction SUCCESS");
        }
    }
}
