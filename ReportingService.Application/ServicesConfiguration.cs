
using Microsoft.Extensions.DependencyInjection;
using ReportingService.Application.Mappings;

namespace ReportingService.Application;

public static class ServicesConfiguration
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(CustomerMapperProfile).Assembly,
                typeof(TransactionMapperProfile).Assembly,
                typeof(ComissionMapperProfile).Assembly,
                typeof(AccountMapperProfile).Assembly);
    }
}
