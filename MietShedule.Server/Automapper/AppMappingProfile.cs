using AutoMapper;


namespace MietShedule.Server.Automapper
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            AllowNullCollections = true;
        }
    }
}
