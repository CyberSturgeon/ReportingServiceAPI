﻿
using Microsoft.Extensions.DependencyInjection;
using ReportingService.Application.Mappings;

namespace ReportingService.Application;

public static class ServicesConfiguration
{
    public static void ConfigureApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(CustomerMapperProfile).Assembly);
    }
}
