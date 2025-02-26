using AutoMapper;
using MYPBackendMicroserviceIntegrations.Messages;
using ReportingService.Application.Models;
using ReportingService.Persistence.Entities;

namespace ReportingService.Application.Mappings;

public class CustomerMapperProfile : Profile
{
    public CustomerMapperProfile()
    {
        CreateMap<Customer, CustomerModel>().ReverseMap();
        
        CreateMap<CustomerMessage, CustomerModel>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CustomerServiceId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src =>
               DateTime.SpecifyKind( new DateTime(src.BirthDate.Year, src.BirthDate.Month, src.BirthDate.Day, 0, 0, 0), DateTimeKind.Utc)));
            
    }
}
