using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReportingService.Persistence.Repositories;
using ReportingService.Persistence.Repositories.Interfaces;

namespace ReportingService.Persistence.Configuration;

public static class ServicesConfiguration
{
    public static void ConfigurePersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ReportingContext>(options => options.UseNpgsql(configuration.GetConnectionString("ReportingDbConnection")));

        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IComissionRepository, ComissionRepository>();
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();
    }
}
