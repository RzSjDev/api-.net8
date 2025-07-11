using api_.net9.Common.Dto;
using api_net9.Application.Dto.AuthDto;

namespace api_net9.Application.Services.AuthUser
{
    public interface IAuthUserService
    {
        Task<ServiceResponseDto<int>> Rigester(UserRegestrationDto userRegestration, string Password);
        Task<ServiceResponseDto<string>> Login(UserLoginDto userLogin);
        Task<bool> IsExist(string user, string email);
    }
}