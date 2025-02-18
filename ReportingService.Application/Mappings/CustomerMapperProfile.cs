using AutoMapper;
using Integration.Messages;
using ReportingService.Application.Models;
using ReportingService.Persistence.Entities;

namespace ReportingService.Application.Mappings;

public class CustomerMapperProfile : Profile
{
    public CustomerMapperProfile()
    {
        CreateMap<Customer, CustomerModel>().ReverseMap();
        CreateMap<CustomerModel, CustomerIntegrationModel>().ReverseMap();
    }
}
