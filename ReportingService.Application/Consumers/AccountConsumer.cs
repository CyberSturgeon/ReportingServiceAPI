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
    public class AccountConsumer(
        IAccountService accountService,
        IMapper mapper) : IConsumer<List<AccountMessage>>
    {

        public async Task Consume(ConsumeContext<List<AccountMessage>> context)
        {
            var accounts = context.Message;
            var accountModels = mapper.Map<List<AccountModel>>(accounts);

            await accountService.TransactionalAddAsync(accountModels);
        }
    }
}
