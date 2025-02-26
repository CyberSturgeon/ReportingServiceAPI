using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ReportingService.Application.Consumers;
using ReportingService.Application.Integration;
using ReportingService.Application.Mappings;
using ReportingService.Application.Services;
using ReportingService.Application.Services.Interfaces;

namespace ReportingService.Application;

public static class ServicesConfiguration
{
    public static void ConfigureApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddAutoMapper(typeof(CustomerMapperProfile).Assembly);
        services.AddTransient<IAccountService, AccountService>();
        services.AddScoped<ITransactionService, TransactionService>();

        services.AddOptions<RabbitMQSettings>()
         .Configure<IConfiguration>((options, configuration) =>
         {
             var section = configuration.GetSection("RabbitMq");
             options.Host = section.GetValue<string>("Host") ?? string.Empty;
             options.Username = section.GetValue<string>("Username") ?? string.Empty;
             options.Password = section.GetValue<string>("Password") ?? string.Empty;
             options.CustomerWithAccountQueue = section.GetSection("Consumers").GetValue<string>("CustomerWithAccountQueue") ?? string.Empty;
             options.CustomerMessageQueue = section.GetSection("Consumers").GetValue<string>("CustomerMessageQueue") ?? string.Empty;
             options.RoleUpdateQueue = section.GetSection("Consumers").GetValue<string>("RoleUpdateQueue") ?? string.Empty;
         });

        services.AddMassTransit(x =>
        {
            x.AddConsumer<CustomerWithAccountConsumer>();
            x.AddConsumer<CustomerVipUpdateConsumer>();
            x.AddConsumer<CustomerMessageConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                var settings = context.GetRequiredService<IOptions<RabbitMQSettings>>().Value;

                cfg.Host(settings.Host, h =>
                {
                    h.Username(settings.Username);
                    h.Password(settings.Password);
                });

                cfg.ReceiveEndpoint(settings.CustomerWithAccountQueue, e =>
                {
                    e.ConfigureConsumer<CustomerWithAccountConsumer>(context);
                });

                cfg.ReceiveEndpoint(settings.RoleUpdateQueue, e =>
                {
                    e.ConfigureConsumer<CustomerVipUpdateConsumer>(context);
                });

                cfg.ReceiveEndpoint(settings.CustomerMessageQueue, e =>
                {
                    e.ConfigureConsumer<CustomerMessageConsumer>(context);
                });
            });
        });
    }
}
