using api_net9.Application.Dto.UserCrud;
using api_net9.Domain.Models;
using AutoMapper;

namespace api_net9
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