using AutoMapper;
using ReportingService.Application.Models;
using ReportingService.Persistence.Entities;

namespace ReportingService.Application.Mappings;

public class CustomerMapperProfile : Profile
{
    public CustomerMapperProfile()
    {
        CreateMap<Customer, CustomerModel>()
            .ForMember(x => x.Accounts, opt => opt.MapFrom(y => y.Accounts))
            .ForMember(x => x.Transactions, opt => opt.MapFrom(y => y.Transactions)).ReverseMap();
    }
}
