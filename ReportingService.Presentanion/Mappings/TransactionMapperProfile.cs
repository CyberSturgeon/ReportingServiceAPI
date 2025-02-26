using AutoMapper;
using ReportingService.Application.Models;
using ReportingService.Presentanion.Models;

namespace ReportingService.Presentanion.Mappings;

public class TransactionMapperProfile : Profile
{
    public TransactionMapperProfile()
    {
        CreateMap<TransactionModel, TransactionResponse>().ReverseMap();
    }
}
