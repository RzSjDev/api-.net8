using api.net.Data;
using api.net.Dto.AuthDto;
using api.net.Services.AuthUser;
using dotnet_rpg.Dtos.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace AuthUserService_Test
{
    [TestFixture]
    public class AuthUserServiceTest
    {
        private IConfiguration _configuration;
        private UserRegestrationDto _userAuth_one;
        private UserLoginDto _userAuth_one_Login_WrongUserNameOrEmail;
        private UserLoginDto _userAuth_one_Login_WrongPasssword;
        private UserLoginDto _userAuth_one_Login_Success;
        private DbContextOptions<DataContext> options;
        public AuthUserServiceTest()
        {
            var inMemorySettings = new Dictionary<string, string>
           {
               {"AppSettings:Token", "this is my custom Secret key for authentication and is test for Nunit Test"}
           };
            _configuration = new ConfigurationBuilder().AddInMemoryCollection(inMemorySettings).Build();
            
            _userAuth_one = new UserRegestrationDto()
            {
                UserName = "Test",
                UserEmail = "Test",
                Password = "Test",
                ConfirmPassword = "Test",

            };
            _userAuth_one_Login_Success = new UserLoginDto()
            {
                UserNameOrEmail = "Test",
                Password = "Test"
            };
            _userAuth_one_Login_WrongUserNameOrEmail = new UserLoginDto()
            {
                UserNameOrEmail = "WrongUserNameOrEmail",
                Password = "Test"
            };
            _userAuth_one_Login_WrongPasssword = new UserLoginDto()
            {
                UserNameOrEmail = "Test",
                Password = "WrongPasssword"
            };
        }
        [SetUp]
        public void Setup()
        {
            options = new DbContextOptionsBuilder<DataContext>()
               .UseInMemoryDatabase(databaseName: "temp_Api").Options;
        }
        [Test]
        [Order(1)]
        public void Rigester_UserOne_CheckTheUserSuccessfulyRigester()
        {
            //arrange

            //act
            using (var context = new DataContext(options))
            {
                
                var services = new AuthUserService(context, _configuration);
                services.Rigester(_userAuth_one, _userAuth_one.Password);
            }
            //assert
            using (var context = new DataContext(options))
            {

                var ContactFromDb = context.userAuths.FirstOrDefault(u => u.UserName == _userAuth_one.UserName);
                Assert.That(_userAuth_one.UserName, Is.EqualTo(ContactFromDb.UserName));
                Assert.That(_userAuth_one.UserEmail, Is.EqualTo(ContactFromDb.Email));
            }
        }
        [Test]
        [Order(2)]
        public void Login_UserOne_CheckTheUserSuccessfulyLogin()
        {
            //arrange

            //act
            using (var context = new DataContext(options))
            {
                context.Database.EnsureDeleted();
                var services = new AuthUserService(context, _configuration);
                services.Rigester(_userAuth_one, _userAuth_one.Password);

            }
            //assert
            using (var context = new DataContext(options))
            {
                var services = new AuthUserService(context, _configuration);
                var response1 = services.Login(_userAuth_one_Login_WrongUserNameOrEmail);
                var response2 = services.Login(_userAuth_one_Login_WrongPasssword);
                var response3 = services.Login(_userAuth_one_Login_Success);
                Assert.That(response1.Result.Message, Is.EqualTo("User not found"));
                Assert.That(response2.Result.Message, Is.EqualTo("Wrong password"));
                Assert.That(response3.Result.Message, Is.EqualTo("You Successfuly Login"));
            }
        }
        [Test]
        [Order(3)]
        public void IsExist_UserOne_CheckTheUserIsExistOrNot()
        {
            //arrange

            //act
            using (var context = new DataContext(options))
            {
                context.Database.EnsureDeleted();
                var services = new AuthUserService(context, _configuration);
                services.Rigester(_userAuth_one, _userAuth_one.Password);

            }
            //assert
            using (var context = new DataContext(options))
            {
                var services = new AuthUserService(context, _configuration);
                var isExist = services.IsExist(_userAuth_one_Login_Success.UserNameOrEmail, _userAuth_one_Login_Success.Password);
                Assert.That(isExist.Result);
            }
        }

    }

}
