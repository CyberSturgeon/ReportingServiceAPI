
using Microsoft.Extensions.DependencyInjection;
using ReportingService.Application.Mappings;
using ReportingService.Application.Services;
using ReportingService.Application.Services.Interfaces;

namespace ReportingService.Application;

public static class ServicesConfiguration
{
    public static void ConfigureApplication(this IServiceCollection services)
    {
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddAutoMapper(typeof(CustomerMapperProfile).Assembly);
    }
}
