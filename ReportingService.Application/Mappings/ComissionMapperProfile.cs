
using AutoMapper;
using ReportingService.Application.Models;
using ReportingService.Persistence.Entities;

namespace ReportingService.Application.Mappings;

public class ComissionMapperProfile : Profile
{
    public ComissionMapperProfile()
    {
        CreateMap<Comission, ComissionModel>().ReverseMap();
    }
}
