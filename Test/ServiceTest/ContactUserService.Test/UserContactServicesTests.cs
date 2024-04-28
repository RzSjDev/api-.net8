using api.net;
using api.net.Data;
using api.net.Dto.getCharacter;
using api.net.Models;
using api.net.Services.UserContact;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System.Security.Claims;

namespace Api.net_Test
{
    [TestFixture]
    public class UserContactServicesTests
    {
        private UserAuth _userAuth_one;
        private IMapper _mapper;
        private Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private AddUserDto _usersContact_one;
        private AddUserDto _usersContact_two;
        private UpdateUserDto _usersContact_three;
        private DbContextOptions<DataContext> options;
        public UserContactServicesTests()
        {
            var profile = new AutoMapperProfile();
            var configuration = new MapperConfiguration(c => c.AddProfile(profile));
            _mapper = new Mapper(configuration);

            _usersContact_one = new AddUserDto()
            {
                name = "ali",
                family = "karimi",
                age = 30,
                city = "tehran",
                phone = "9121112233",
                email = "ali@gmail.com"
            };
            _usersContact_two = new AddUserDto()
            {
                name = "abbas",
                family = "mohammadi",
                age = 30,
                city = "tehran",
                phone = "9124412233",
                email = "abbas@gmail.com",
            };
            _usersContact_three = new UpdateUserDto()
            {
                UserId = 1,
                name = "akbar",
                family = "mohammadi",
                age = 33,
                city = "tehran",
                phone = "9124412233",
                email = "akbar@gmail.com",
            };
        }
        [SetUp]
        public void Setup()
        {
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            options = new DbContextOptionsBuilder<DataContext>()
               .UseInMemoryDatabase(databaseName: "temp_Api").Options;
        }
        [Test]
        [Order(1)]
        public async Task AddContact_ContactOne_CheckTheValuesFromDatabase()
        {
            //arrange

            //act
            using (var context = new DataContext(options))
            {
                _httpContextAccessorMock.Setup(h => h.HttpContext.User).Returns(addFakeClaim());
                var services = new UserContactServices(_mapper, context, _httpContextAccessorMock.Object);
                services.AddUserContact(_usersContact_one);
            }
            //assert
            using (var context = new DataContext(options))
            {

                var ContactFromDb = await context.usersContacts.FirstOrDefaultAsync(u => u.name == _usersContact_one.name && u.UserAuthId == 8);
                Assert.That(_usersContact_one.name, Is.EqualTo(ContactFromDb.name));
                Assert.That(_usersContact_one.family, Is.EqualTo(ContactFromDb.family));
                Assert.That(_usersContact_one.email, Is.EqualTo(ContactFromDb.email));
                Assert.That(_usersContact_one.age, Is.EqualTo(ContactFromDb.age));
                Assert.That(_usersContact_one.city, Is.EqualTo(ContactFromDb.city));
                Assert.That(_usersContact_one.phone, Is.EqualTo(ContactFromDb.phone));
            }
        }

        [Test]
        [Order(2)]
        public void GetAllContact_ContactOneAndTwo_CheckBoththeContactFromDatabase()
        {
            //arrange
            var expectedResult = new List<AddUserDto> { _usersContact_one, _usersContact_two };

            using (var context = new DataContext(options))
            {
                context.Database.EnsureDeleted();
                _httpContextAccessorMock.Setup(h => h.HttpContext.User).Returns(addFakeClaim());
                var services = new UserContactServices(_mapper, context, _httpContextAccessorMock.Object);
                services.AddUserContact(_usersContact_one);
                services.AddUserContact(_usersContact_two);
            }
            //act
            using (var context = new DataContext(options))
            {
                _httpContextAccessorMock.Setup(h => h.HttpContext.User).Returns(addFakeClaim());
                var services = new UserContactServices(_mapper, context, _httpContextAccessorMock.Object);
                var actualList = services.getAllUsersContact();
                Assert.That(actualList.Result.Data.Count, Is.EqualTo(expectedResult.Count));
            }
        }

        //assert
        [Test]
        [Order(3)]
        public void GetContactById_Contactone_CheckTheOneValueFromDatabase()
        {
            //arrange

            //act
            using (var context = new DataContext(options))
            {
                context.Database.EnsureDeleted();
                _httpContextAccessorMock.Setup(h => h.HttpContext.User).Returns(addFakeClaim());
                var services = new UserContactServices(_mapper, context, _httpContextAccessorMock.Object);
                services.AddUserContact(_usersContact_one);
                services.AddUserContact(_usersContact_two);

            }

            //assert
            using (var context = new DataContext(options))
            {
                _httpContextAccessorMock.Setup(h => h.HttpContext.User).Returns(addFakeClaim());
                var services = new UserContactServices(_mapper, context, _httpContextAccessorMock.Object);
                var ContactFromDb = services.getUsersContactById(2).Result.Data;
                Assert.That(_usersContact_two.name, Is.EqualTo(ContactFromDb.name));
                Assert.That(_usersContact_two.family, Is.EqualTo(ContactFromDb.family));
                Assert.That(_usersContact_two.email, Is.EqualTo(ContactFromDb.email));
                Assert.That(_usersContact_two.age, Is.EqualTo(ContactFromDb.age));
                Assert.That(_usersContact_two.city, Is.EqualTo(ContactFromDb.city));
                Assert.That(_usersContact_two.phone, Is.EqualTo(ContactFromDb.phone));

            }
        }

        [Test]
        [Order(4)]
        public async Task updateContact_Contactone_CheckTheValuesChangeFromDatabase()
        {
            //arrange

            //act
            using (var context = new DataContext(options))
            {
                context.Database.EnsureDeleted();
                _httpContextAccessorMock.Setup(h => h.HttpContext.User).Returns(addFakeClaim());
                var services = new UserContactServices(_mapper, context, _httpContextAccessorMock.Object);
                services.AddUserContact(_usersContact_one);
                services.AddUserContact(_usersContact_two);
                services.UpdateUserContact(_usersContact_three);
            }

            //assert
            using (var context = new DataContext(options))
            {
                var ContactFromDb = context.usersContacts.FirstOrDefault(u => u.UserId == 1 && u.UserAuthId == 8);
                Assert.That(_usersContact_three.name, Is.EqualTo(ContactFromDb.name));
                Assert.That(_usersContact_three.family, Is.EqualTo(ContactFromDb.family));
                Assert.That(_usersContact_three.email, Is.EqualTo(ContactFromDb.email));
                Assert.That(_usersContact_three.age, Is.EqualTo(ContactFromDb.age));
                Assert.That(_usersContact_three.city, Is.EqualTo(ContactFromDb.city));
                Assert.That(_usersContact_three.phone, Is.EqualTo(ContactFromDb.phone));
            }
        }
        [Test]
        [Order(5)]
        public void DeleteContact_ContactOne_CheckTheValuesDeleteFromDatabase()
        {
            //arrange

            //act
            using (var context = new DataContext(options))
            {
                context.Database.EnsureDeleted();
                _httpContextAccessorMock.Setup(h => h.HttpContext.User).Returns(addFakeClaim());
                var services = new UserContactServices(_mapper, context, _httpContextAccessorMock.Object);
                services.AddUserContact(_usersContact_one);
                services.AddUserContact(_usersContact_two);
                var testId = 2;
                services.DeleteUserContact(testId);
            }

            //assert
            using (var context = new DataContext(options))
            {
                _httpContextAccessorMock.Setup(h => h.HttpContext.User).Returns(addFakeClaim());
                var services = new UserContactServices(_mapper, context, _httpContextAccessorMock.Object);
                var ContactFromDb = services.getAllUsersContact().Result.Data.Count;
                Assert.That(1, Is.EqualTo(ContactFromDb));


            }
        }
        public ClaimsPrincipal addFakeClaim()
        {
            var UserAuthId = "8";
            var claims = new ClaimsPrincipal(
                new ClaimsIdentity(
                    new Claim[] { new Claim(ClaimTypes.NameIdentifier, UserAuthId) }, "Basic")
                );
            return claims;
        }
    }
}
