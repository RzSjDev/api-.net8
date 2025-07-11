using System.Diagnostics.Contracts;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using api_.net9.Common.Dto;
using api_net9.Application.Context;
using api_net9.Application.Dto.UserCrud;
using api_net9.Domain.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using static api_net9.Application.Services.UserContact.UserContactServices;
namespace api_net9.Application.Services.UserContact
{
    public class UserContactServices : IUserContactServices
    {

        private readonly IMapper _mapper;
        private readonly IDataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserContactServices(IMapper mapper, IDataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _mapper = mapper;
        }

        private int GetUserId()
        {
           int response = int.Parse(_httpContextAccessor.HttpContext!.User
          .FindFirst(ClaimTypes.NameIdentifier)!.Value);
           return response;
        }
        public async Task<ServiceResponseDto<List<GetUserDto>>> AddUserContact(AddUserDto newUser)
        {
            var serviceResponse = new ServiceResponseDto<List<GetUserDto>>();
            var users = _mapper.Map<UsersContact>(newUser);
            users.userAuth = await _context.userAuths.FirstOrDefaultAsync(u => u.UserAuthId == GetUserId());
            users.UserAuthId = GetUserId();
            _context.usersContacts.Add(users);
            await _context.SaveChangesAsync();
            serviceResponse.Data = await _context.usersContacts.Where(c => c.userAuth!.UserAuthId == GetUserId()).Select(c => _mapper.Map<GetUserDto>(c)).ToListAsync();
            serviceResponse.Message = "Contact Successfuly Added!!!";
            return serviceResponse;
        }

        public async Task<ServiceResponseDto<List<GetUserDto>>> DeleteUserContact(int id)
        {
            var serviceResponse = new ServiceResponseDto<List<GetUserDto>>();

            try
            {
                var character = await _context.usersContacts
                    .FirstOrDefaultAsync(c => c.UserId == id && c.UserAuthId == GetUserId());
                if (character is null)
                    throw new Exception($"Character with Id '{id}' not found.");

                _context.usersContacts.Remove(character);

                await _context.SaveChangesAsync();

                serviceResponse.Data =
                    await _context.usersContacts
                        .Where(c => c.UserAuthId == GetUserId())
                        .Select(c => _mapper.Map<GetUserDto>(c)).ToListAsync();
                serviceResponse.Message = $"Contact with Id {id} successfuly Removed!!!";
            }
            catch (Exception ex)
            {
                serviceResponse.succsess = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponseDto<List<GetUserDto>>> getAllUsersContact()
        {
            var serviceResponse = new ServiceResponseDto<List<GetUserDto>>();
            var dbCharacters = await _context.usersContacts.Where(c => c.UserAuthId == GetUserId()).ToListAsync();
            serviceResponse.Data = dbCharacters.Select(c => _mapper.Map<GetUserDto>(c)).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponseDto<GetUserDto>> getUsersContactById(int id)
        {
            var serviceResponse = new ServiceResponseDto<GetUserDto>();
            var dbCharacter = await _context.usersContacts.FirstOrDefaultAsync(c => c.UserId == id && c.UserAuthId == GetUserId());
            serviceResponse.Data = _mapper.Map<GetUserDto>(dbCharacter);
            return serviceResponse;
        }

        public async Task<ServiceResponseDto<GetUserDto>> UpdateUserContact(UpdateUserDto updatedUser)
        {
            var serviceResponse = new ServiceResponseDto<GetUserDto>();

            try
            {
                var users =
                    await _context.usersContacts
                        .FirstOrDefaultAsync(c => c.UserId == updatedUser.UserId && c.UserAuthId == GetUserId());
                if (users is null)
                    throw new Exception($"Character with Id '{updatedUser.UserId}' not found.");

                users.name = updatedUser.name;
                users.email = updatedUser.email;
                users.phone = updatedUser.phone;
                users.city = updatedUser.city;
                users.age = updatedUser.age;
                users.family = updatedUser.family;
                await _context.SaveChangesAsync();
                serviceResponse.Data = _mapper.Map<GetUserDto>(users);
                serviceResponse.Message = $"Contact With Id {updatedUser.UserId} Succesfuly Updated";
            }
            catch (Exception ex)
            {
                serviceResponse.succsess = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }
    }
}

