
using AutoMapper;
using MassTransit;
using Microsoft.Extensions.Logging;
using MYPBackendMicroserviceIntegrations.Messages;
using ReportingService.Application.Models;
using ReportingService.Application.Services.Interfaces;

namespace ReportingService.Application.Consumers;

public class CustomerMessageConsumer(ICustomerService customerService,
        ILogger<CustomerMessageConsumer> logger,
        IMapper mapper) : IConsumer<CustomerMessage>
{
    public async Task Consume(ConsumeContext<CustomerMessage> context)
    {
        logger.LogInformation($"CONSUME {context.Message.Id} customer");
        var customer = context.Message;
        var customerModel = mapper.Map<CustomerModel>(customer);
        await customerService.AddAsync(customerModel);
        logger.LogInformation($"ADD {context.Message.Id} customer SUCCESS");
    }
}
