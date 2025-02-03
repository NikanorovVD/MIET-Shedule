using AutoMapper;
using DataLayer.Entities;
using ServiceLayer.Constants;
using ServiceLayer.Models;


namespace MietShedule.Server.Automapper
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            AllowNullCollections = true;

            // Data - Service
            CreateMap<Couple, CoupleDto>()
                .ForMember(dest => dest.Time, opt => opt.MapFrom(src => CoupleTime.GetTime(src.Order)));
        }
    }
}
