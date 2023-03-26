using AutoMapper;
using PostService.Dtos;
using PostService.Model;

namespace PostService.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Post, PostDto>().ReverseMap();
        }
    }
}
