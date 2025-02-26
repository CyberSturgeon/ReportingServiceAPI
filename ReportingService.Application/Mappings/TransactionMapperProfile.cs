using AutoMapper;
using MYPBackendMicroserviceIntegrations.Messages;
using ReportingService.Application.Models;
using ReportingService.Persistence.Entities;

namespace ReportingService.Application.Mappings;

public class TransactionMapperProfile : Profile
{
    public TransactionMapperProfile()
    {
        CreateMap<Transaction, TransactionModel>().ReverseMap();
        CreateMap<TransactionCreatedMessage, TransactionModel>();
    }
}
