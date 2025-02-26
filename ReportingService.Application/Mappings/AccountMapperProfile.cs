using AutoMapper;
using MYPBackendMicroserviceIntegrations.Messages;
using ReportingService.Application.Models;
using ReportingService.Persistence.Entities;

namespace ReportingService.Application.Mappings;

public class AccountMapperProfile : Profile
{
    public AccountMapperProfile()
    {
        CreateMap<Account, AccountModel>().ReverseMap();
        CreateMap<AccountMessage, AccountModel>();
    }
}
