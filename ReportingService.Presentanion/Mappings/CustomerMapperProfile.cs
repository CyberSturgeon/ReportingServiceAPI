using AutoMapper;
using ReportingService.Application.Models;
using ReportingService.Presentanion.Models;

namespace ReportingService.Presentanion.Mappings;

public class CustomerMapperProfile : Profile
{
    public CustomerMapperProfile()
    {
        CreateMap<CustomerResponse, CustomerModel>().ReverseMap();
    }
}
