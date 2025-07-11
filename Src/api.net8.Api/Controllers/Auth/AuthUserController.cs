using api_.net9.Common.Dto;
using api_net9.Application.Dto.AuthDto;
using api_net9.Application.Services.AuthUser;
using Microsoft.AspNetCore.Mvc;

namespace api_net9.Controllers.Auth
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
