
using AutoMapper;
using ReportingService.Application.Models;
using ReportingService.Persistence.Entities;

namespace ReportingService.Application.Mappings;

public class AccountMapperProfile : Profile
{
    public AccountMapperProfile()
    {
        CreateMap<Account, AccountModel>()
            .ForMember(x => x.Customer, opt => opt.MapFrom(y => y.Customer))
            .ForMember(x => x.Transactions, opt => opt.MapFrom(y => y.Transactions));

        CreateMap<AccountModel, Account>()
            .ForMember(x => x.Customer, opt => opt.MapFrom(y => y.Customer))
            .ForMember(x => x.Transactions, opt => opt.MapFrom(y => y.Transactions));
    }
}
