using AutoMapper;
using ReportingService.Application.Models;
using ReportingService.Presentanion.Models;

namespace ReportingService.Presentanion.Mappings;

public class ComissionMapperProfile : Profile
{
    public ComissionMapperProfile()
    {
        CreateMap<ComissionModel, ComissionResponse>().ReverseMap();
    }
}
