using AutoMapper;

namespace ReportingService.Application.Mappings;

public static class MapperHelper
{
    public static MapperConfiguration ConfigureMapper()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new AccountMapperProfile());
            cfg.AddProfile(new CustomerMapperProfile());
            cfg.AddProfile(new TransactionMapperProfile());
            cfg.AddProfile(new ComissionMapperProfile());
        });
        return config;
    }
}
