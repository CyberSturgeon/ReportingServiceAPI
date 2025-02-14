
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ReportingService.Application.Consumers;
using ReportingService.Application.Integration;
using ReportingService.Application.Mappings;

namespace ReportingService.Application;

public static class ServicesConfiguration
{
    public static void ConfigureApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(typeof(CustomerMapperProfile).Assembly);

        services.Configure<RabbitMQSettings>(configuration.GetSection("RabbitMQSettings"));//HELP

        services.AddMassTransit(x =>
        {
            x.AddConsumer<CustomerConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                var settings = context.GetRequiredService<IOptions<RabbitMQSettings>>().Value;

                cfg.Host(settings.Host, settings.VirtualHost, h =>
                {
                    h.Username(settings.Username);
                    h.Password(settings.Password);
                });

                cfg.ReceiveEndpoint(settings.QueueName, e =>
                {
                    e.ConfigureConsumer<CustomerConsumer>(context);
                });
            });
        });
    }
}
