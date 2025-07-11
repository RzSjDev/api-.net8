using api_.net9.Common.Dto;
using api_net9.Application.Dto.UserCrud;

namespace api_net9.Application.Services.UserContact
{
    public interface IUserContactServices
    {
        public Task<ServiceResponseDto<List<GetUserDto>>> getAllUsersContact();
        public Task<ServiceResponseDto<GetUserDto>> getUsersContactById(int id);
        public Task<ServiceResponseDto<List<GetUserDto>>> AddUserContact(AddUserDto addCharacterDto);
        public Task<ServiceResponseDto<GetUserDto>> UpdateUserContact(UpdateUserDto addCharacterDto);
        public Task<ServiceResponseDto<List<GetUserDto>>> DeleteUserContact(int id);
    }
}