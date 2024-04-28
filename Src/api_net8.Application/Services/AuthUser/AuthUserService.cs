using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using api.net.Dto.AuthDto;
using api.net.Models;
using api.net.Models.ServiceResponse;
using api.net.Services;
using api_net8.Application.Context;
using api_net8.Domain.Models.Enum;
using dotnet_rpg.Dtos.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace api.net.Services.AuthUser
{
    public class AuthUserService : IAuthUserService
    {
        private readonly IDataContext _context;
        private readonly IConfiguration _configuration;

        public AuthUserService(IDataContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public async Task<bool> IsExist(string userName, string email)
        {
            if (await _context.userAuths.AnyAsync(u => u.UserName.ToLower() == userName.ToLower() || u.Email.ToLower() == email.ToLower()))
            {
                return true;
            }
            return false;
        }

        public async Task<ServiceResponseDto<string>> Login(UserLoginDto userLogin)
        {
            var response = new ServiceResponseDto<string>();
            var user = await _context.userAuths.FirstOrDefaultAsync(u => u.UserName.ToLower().Equals(userLogin.UserNameOrEmail.ToLower()) || u.Email.ToLower().Equals(userLogin.UserNameOrEmail.ToLower()));
            if (user is null)
            {
                response.succsess = false;
                response.Message = "User not found";
            }
            else if (!VerifyPasswordHash(userLogin.Password, user.Passwordhash, user.PasswordSalt))
            {
                response.succsess = false;
                response.Message = "Wrong password";
            }
            else
            {
                response.Message = "You Successfuly Login";
                response.Data = CreateToken(user);
            }

            return response;
        }

        public async Task<ServiceResponseDto<int>> Rigester(UserRegestrationDto userRegestration, string Password)
        {
            var response = new ServiceResponseDto<int>();
            if (await IsExist(userRegestration.UserName, userRegestration.UserEmail))
            {
                response.succsess = false;
                response.Message = "User already exist";
                return response;
            }

            CreatePasswordHash(Password, out byte[] passwordHash, out byte[] passwordSalt);
            var user=new UserAuth() 
            {
                UserName=userRegestration.UserName,      
                Email=userRegestration.UserEmail,
                role=Role.subscriber,
                PasswordSalt=passwordSalt,
                Passwordhash=passwordHash
            };

            _context.userAuths.Add(user);
            await _context.SaveChangesAsync();
            response.Message = "You Successfuly Rigester";
            response.Data = user.UserAuthId;
            return response;

        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        private string CreateToken(UserAuth user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserAuthId.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            var appSettingsToken = _configuration.GetSection("AppSettings:Token").Value;
            if (appSettingsToken is null)
                throw new Exception("AppSettings Token is null!");

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(appSettingsToken));

            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}


