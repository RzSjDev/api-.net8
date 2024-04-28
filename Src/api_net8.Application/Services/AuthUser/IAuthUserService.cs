using api.net.Dto.AuthDto;
using api.net.Models;
using api.net.Models.ServiceResponse;
using api.net.Services;
using dotnet_rpg.Dtos.User;

namespace api.net.Services.AuthUser
{
    public interface IAuthUserService
    {
        Task<ServiceResponseDto<int>> Rigester(UserRegestrationDto userRegestration, string Password);
        Task<ServiceResponseDto<string>> Login(UserLoginDto userLogin);
        Task<bool> IsExist(string user, string email);
    }
}