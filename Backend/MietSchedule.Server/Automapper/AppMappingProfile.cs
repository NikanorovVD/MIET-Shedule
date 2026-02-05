using AutoMapper;
using DataLayer.Entities;
using ServiceLayer.Models;

namespace MietSchedule.Server.Automapper
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            // Data - Service
            CreateMap<Pair, PairDto>()
                .ForMember(dest => dest.Group, opt => opt.MapFrom(src => src.Group.Name))
                .ForMember(dest => dest.Teacher, opt => opt.MapFrom(src => src.Teacher.Name));

            CreateMap<TimePair, TimeDto>();

            CreateMap<Pair, PairExportDto>()
                .ForMember(dest => dest.WeekType, opt => opt.MapFrom(src => src.WeekType.ToString()))
                .ForMember(dest => dest.Group, opt => opt.MapFrom(src => src.Group.Name))
                .ForMember(dest => dest.Teacher, opt => opt.MapFrom(src => src.Teacher.Name));
        }
    }
}
