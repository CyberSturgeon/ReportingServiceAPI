using ReportingService.Presentanion.Mappings;

namespace ReportingService.Presentanion;

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
