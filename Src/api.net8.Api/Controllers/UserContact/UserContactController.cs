using api_.net9.Common.Dto;
using api_net9.Application.Dto.UserCrud;
using api_net9.Application.Services.UserContact;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace api_net9.Controllers.UserContact
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserContactController : ControllerBase
    {

        private readonly IUserContactServices _charactersServices;
        public UserContactController(IUserContactServices icharactersServices)
        {
            _charactersServices = icharactersServices;
        }


        [HttpGet("GetAll")]
        public async Task<ActionResult<ServiceResponseDto<List<GetUserDto>>>> Get()
        {
            return Ok(await _charactersServices.getAllUsersContact());
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponseDto<GetUserDto>>> GetSingle(int id)
        {
            var response = await _charactersServices.getUsersContactById(id);
            if (response.Data is null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
        [HttpPost]
        public async Task<ActionResult<ServiceResponseDto<List<GetUserDto>>>> AddContact(AddUserDto newcharacters)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(await _charactersServices.AddUserContact(newcharacters));
        }
        [HttpPut]
        public async Task<ActionResult<ServiceResponseDto<GetUserDto>>> UpdateCantact(UpdateUserDto updatecharacters)
        {

            var response = await _charactersServices.UpdateUserContact(updatecharacters);
            if (response.Data is null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResponseDto<List<GetUserDto>>>> DeleteContact(int id)
        {
            return Ok(await _charactersServices.DeleteUserContact(id));
        }

    }
}