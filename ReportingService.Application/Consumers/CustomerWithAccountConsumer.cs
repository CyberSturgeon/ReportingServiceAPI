using AutoMapper;
using MassTransit;
using Microsoft.Extensions.Logging;
using MYPBackendMicroserviceIntegrations.Messages;
using ReportingService.Application.Models;
using ReportingService.Application.Services.Interfaces;


namespace ReportingService.Application.Consumers
{
    public class CustomerWithAccountConsumer(
        ICustomerService customerService,
        IAccountService accountService,
        ILogger<CustomerWithAccountConsumer> logger,
        IMapper mapper) : IConsumer<CustomerWithAccountMessage>
    {

        public async Task Consume(ConsumeContext<CustomerWithAccountMessage> context)
        {
            logger.LogInformation($"CONSUME Customer {context.Message.Customer.Id} WithAccount {context.Message.Account.Id} Message");
            var customer = context.Message.Customer;
            var account = context.Message.Account;

            var customerModel = mapper.Map<CustomerModel>(customer);
            var accountModel = mapper.Map<AccountModel>(account);

            await customerService.AddAsync(customerModel);
            await accountService.AddAsync(accountModel);
            logger.LogInformation($"ADD Customer {context.Message.Customer.Id} WithAccount {context.Message.Account.Id} SUCCESS");
            
        }
    }
}
