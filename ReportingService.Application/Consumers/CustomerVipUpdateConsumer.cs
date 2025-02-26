
using AutoMapper;
using MassTransit;
using Microsoft.Extensions.Logging;
using MYPBackendMicroserviceIntegrations.Messages;
using ReportingService.Application.Services.Interfaces;

namespace ReportingService.Application.Consumers;

public class CustomerVipUpdateConsumer(ICustomerService customerService,
        IAccountService accountService,
        ILogger<CustomerWithAccountConsumer> logger,
        IMapper mapper) : IConsumer<CustomerRoleUpdateIdsReportingMessage>
{
    public async Task Consume(ConsumeContext<CustomerRoleUpdateIdsReportingMessage> context)
    {
        logger.LogInformation($"CONSUME {context.Message.VipCustomerIds.Count} ids");
        var ids = context.Message.VipCustomerIds;
        //await customerService.BatchUpdateRoleAync(ids);
        logger.LogInformation($"UPDATE {context.Message.VipCustomerIds.Count} ids SUCCESS");
    }
}
