using AutoMapper;
using MassTransit;
using Microsoft.Extensions.Logging;
using MYPBackendMicroserviceIntegrations.Messages;
using ReportingService.Application.Models;
using ReportingService.Application.Services.Interfaces;

namespace ReportingService.Application.Consumers
{
    public class AccountConsumer(
        IAccountService accountService,
        ILogger<TransactionConsumer> logger,
        IMapper mapper) : IConsumer<AccountMessage>
    {

        public async Task Consume(ConsumeContext<AccountMessage> context)
        {
            logger.LogInformation($"CONSUME {context.Message.Id} account");
            var account = context.Message;
            var accountModel = mapper.Map<AccountModel>(account);

            //await accountService.AddAsync(accountModel);
            logger.LogInformation($"UPDATE {context.Message.Id} account SUCCESS");
        }
    }
}
