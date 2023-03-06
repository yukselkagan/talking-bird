using AutoMapper;
using IdentityService.Dtos;
using IdentityService.Models;

namespace IdentityService.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserRegisterDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();

        }
    }
}
