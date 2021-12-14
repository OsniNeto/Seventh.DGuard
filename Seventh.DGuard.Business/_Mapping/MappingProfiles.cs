using AutoMapper;
using Seventh.DGuard.Database;
using Seventh.DGuard.DTO;

namespace Seventh.DGuard.Business
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Server, ServerDTO_In>().ReverseMap();
            CreateMap<Server, ServerDTO_Out>().ReverseMap();

            CreateMap<Video, VideoDTO_In>().ReverseMap();
            CreateMap<Video, VideoDTO_Out>().ReverseMap();

            CreateMap<RecyclerStatus, RecyclerStatusDTO_In>().ReverseMap();
            CreateMap<RecyclerStatus, RecyclerStatusDTO_Out>().ReverseMap();
            CreateMap<RecyclerStatusDTO_In, RecyclerStatusDTO_Out>().ReverseMap();
        }
    }
}