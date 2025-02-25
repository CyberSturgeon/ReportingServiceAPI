using AutoMapper;
using MassTransit;
using MYPBackendMicroserviceIntegrations.Messages;
using ReportingService.Application.Models;
using ReportingService.Application.Services.Interfaces;


namespace ReportingService.Application.Consumers
{
    public class CustomerConsumer(
        ICustomerService customerService,
        IMapper mapper) : IConsumer<List<CustomerMessage>>
    {

        public async Task Consume(ConsumeContext<List<CustomerMessage>> context)
        {
            var customers = context.Message;
            var customerModels = mapper.Map<List<CustomerModel>>(customers);

            await customerService.TransactionalAddCustomersAsync(customerModels);
        }

        public async Task Consume(ConsumeContext<List<Guid>> context)
        {
            var ids = context.Message;

            await customerService.BatchUpdateRoleAync(ids);
        }
    }
}
