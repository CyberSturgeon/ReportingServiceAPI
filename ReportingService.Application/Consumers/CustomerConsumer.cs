using AutoMapper;
using Integration.Messages;
using MassTransit;
using ReportingService.Application.Models;
using ReportingService.Application.Services.Interfaces;


namespace ReportingService.Application.Consumers
{
    public class CustomerConsumer(
        ICustomerService customerService,
        IMapper mapper) : IConsumer<List<CustomerIntegrationModel>>
    {

        public async Task Consume(ConsumeContext<List<CustomerIntegrationModel>> context)
        {
            var customers = context.Message;
            var customerModels = mapper.Map<List<CustomerModel>>(customers);

            await customerService.TransactionalAddCustomersAsync(customerModels);
        }

        public async Task Consume(ConsumeContext<List<Guid>> context)
        {
            var ids = context.Message;

            //await customerService.Вызов хранимой SQL функции
        }
    }
}
