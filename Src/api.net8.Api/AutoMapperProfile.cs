using api.net.Dto.getCharacter;
using api.net.Models;
using AutoMapper;

namespace api.net
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
           
            CreateMap<UsersContact, GetUserDto>();
            CreateMap< AddUserDto,UsersContact > ();
           
        }
    }
}