using api.net.Dto.getCharacter;
using api.net.Models.ServiceResponse;

namespace api.net.Services.UserContact
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