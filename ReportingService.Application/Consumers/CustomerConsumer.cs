using AutoMapper;
using MassTransit;
using MYPBackendMicroserviceIntegrations.Messages;
using ReportingService.Application.Models;
using ReportingService.Application.Services;
using ReportingService.Application.Services.Interfaces;


namespace ReportingService.Application.Consumers
{
    public class CustomerConsumer(
        ICustomerService customerService,
        IAccountService accountService,
        IMapper mapper) : IConsumer<List<CustomerWithAccountMessage>>, IConsumer<CustomerRoleUpdateIdsReportingMessage>
    {

        public async Task Consume(ConsumeContext<List<CustomerWithAccountMessage>> context)
        {
            var customers = context.Message.Select(x => x.Customer).ToList();
            var accounts = context.Message.Select(x => x.Account).ToList();

            var customerModels = mapper.Map<List<CustomerModel>>(customers);
            //var accountModels = mapper.Map<List<AccountModel>>(accounts);

            //await accountService.TransactionalAddAccountsAsync(accountModels);
            await customerService.TransactionalAddCustomersAsync(customerModels);
        }

        public async Task Consume(ConsumeContext<CustomerRoleUpdateIdsReportingMessage> context)
        {
            var ids = context.Message.VipCustomerIds;
            await customerService.BatchUpdateRoleAync(ids);
        }
    }
}
