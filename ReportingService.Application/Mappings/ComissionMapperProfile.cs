
using AutoMapper;
using ReportingService.Application.Models;
using ReportingService.Persistence.Entities;

namespace ReportingService.Application.Mappings;

public class ComissionMapperProfile : Profile
{
    public ComissionMapperProfile()
    {
        CreateMap<Comission, ComissionModel>()
            .ForMember(x => x.Transaction, opt => opt.MapFrom(y => y.Transaction));

        CreateMap<ComissionModel, Comission>()
            .ForMember(x => x.Transaction, opt => opt.MapFrom(y => y.Transaction));
    }
}
