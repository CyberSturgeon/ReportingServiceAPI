using ReportingService.Presentanion.Mappings;

namespace ReportingService.Presentanion.Configuration;

public static class ServicesConfiguration
{
    public static void ConfigurePresentation(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddAutoMapper(typeof(CustomerMapperProfile).Assembly);
    }
}
