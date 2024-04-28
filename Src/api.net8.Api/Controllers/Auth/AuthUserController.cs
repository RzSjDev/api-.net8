using api.net.Dto.AuthDto;
using api.net.Models;
using api.net.Models.ServiceResponse;
using api.net.Services.AuthUser;
using dotnet_rpg.Dtos.User;
using Microsoft.AspNetCore.Mvc;

namespace api.net.Controllers.Auth
{
    [ApiController]
    [Route("[controller]")]
    public class AuthUserController : ControllerBase
    {
        private readonly IAuthUserService authUser;
        public AuthUserController(IAuthUserService authUser)
        {
            this.authUser = authUser;
        }
        [HttpPost("Register")]
        public async Task<ActionResult<ServiceResponseDto<int>>> Rigester(UserRegestrationDto userRegestrationDto)
        {
            var response = await authUser.Rigester(userRegestrationDto, userRegestrationDto.Password);
            if (!response.succsess)
            {
                BadRequest(response);
            }
            return Ok(response);
        }
        [HttpPost("Login")]
        public async Task<ActionResult<ServiceResponseDto<int>>> Login(UserLoginDto request)
        {
            var response = await authUser.Login(request);
            if (!response.succsess)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }

}
