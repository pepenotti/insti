using Bogus;
using Insti.API.Controllers;
using Insti.API.Service.Interfaces;
using Insti.Core.Constants;
using Insti.Core.DTO.API.Authentication;
using Insti.Core.DTO.API.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Insti.API.Test.Controllers
{
    [TestClass]
    public class AuthControllerTest
    {
        private const string USER_NAME = "user";
        private const string PASSWORD = "password";

        private const string JWT_SECRET = "SecretSecretSecret SecretSecretSecret SecretSecretSecret";
        private const string JWT_VALID_ISSUER= "Secret";
        private const string JWT_VALID_AUDIENCE = "Secret";
        private const int JWT_EXPIRATION = 1;
        
        public AuthControllerTest() { }

        #region Login Tests

        [TestMethod]
        public async Task Login_Success()
        {
            //Arrange
            var authServiceMock = new Mock<IAuthService>();

            authServiceMock.Setup(x => x.Login(It.IsAny<string>(), It.IsAny<string>()))
                .Returns((string user, string password)
                    => Task.FromResult(GetToken(new List<Claim>() { new Claim(ClaimTypes.Name, user) })));

            var controller = new AuthController(authServiceMock.Object);

            //Act
            LoginModel loginModel = new() { Username = USER_NAME, Password = PASSWORD };

            var result = await controller!.Login(loginModel) as OkObjectResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);

            var resultValue = result.Value as LoginOKResponse;
            Assert.IsNotNull(resultValue!.Token);
        }

        [TestMethod]
        public async Task Login_Error()
        {
            //Arrange
            var authServiceMock = new Mock<IAuthService>();
            var error = "Unauthorized";

            authServiceMock.Setup(x => x.Login(It.IsAny<string>(), It.IsAny<string>()))
                .Returns((string user, string password) => throw new UnauthorizedAccessException(error));

            var controller = new AuthController(authServiceMock.Object);

            //Act
            LoginModel loginModel = new() { Username = USER_NAME, Password = PASSWORD };

            var result = await controller!.Login(loginModel) as UnauthorizedObjectResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status401Unauthorized, result.StatusCode);

            var resultValue = result.Value as StatusResponse;
            Assert.AreEqual(StatusResponseTypes.Error, resultValue!.Status);
            Assert.AreEqual(error, resultValue!.Message);
        }

        #endregion

        #region Register tests

        [TestMethod]
        public async Task Register_Success()
        {
            //Arrange
            var authServiceMock = new Mock<IAuthService>();
            var controller = new AuthController(authServiceMock.Object);
            var registerModel = GenerateRegisterModel();

            //Act
            var result = await controller!.Register(registerModel) as OkObjectResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);

            var resultValue = result.Value as StatusResponse;
            Assert.AreEqual(StatusResponseTypes.Success, resultValue!.Status);
        }

        [TestMethod]
        public async Task Register_Failure()
        {
            //Arrange
            var authServiceMock = new Mock<IAuthService>();
            var error = "BadHttpRequestException";

            authServiceMock.Setup(x => x.Register(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>()))
                .Returns((string userName, string email, string password, List<string> roles) => throw new BadHttpRequestException(error));

            var controller = new AuthController(authServiceMock.Object);
            var registerModel = GenerateRegisterModel();

            //Act
            var result = await controller!.Register(registerModel) as BadRequestObjectResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status400BadRequest, result.StatusCode);

            var resultValue = result.Value as StatusResponse;
            Assert.AreEqual(StatusResponseTypes.Error, resultValue!.Status);
            Assert.AreEqual(error, resultValue!.Message);
        }

        #endregion

        #region Private Methods

        private JwtSecurityToken GetToken(List<Claim>? claims = null)
        {
            return new JwtSecurityToken(
                issuer: JWT_VALID_ISSUER,
                audience: JWT_VALID_AUDIENCE,
                expires: DateTime.Now.AddHours(JWT_EXPIRATION),
                claims: claims ?? new List<Claim>(),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWT_SECRET)), SecurityAlgorithms.HmacSha256)
                );
        }

        private static RegisterModel GenerateRegisterModel()
        {
            var personModelMocker = new Faker<PersonModel>()
                            .RuleFor(x => x.FirstName, f => f.Person.FirstName)
                            .RuleFor(x => x.LastName, f => f.Person.LastName)
                            .RuleFor(x => x.GenderId, f => "0");
            var person = personModelMocker.Generate(1).First();

            var registerModelMocker = new Faker<RegisterModel>()
                .RuleFor(x => x.Username, f => f.Internet.UserName())
                .RuleFor(x => x.Email, f => f.Internet.Email())
                .RuleFor(x => x.Password, f => f.Internet.Password())
                .RuleFor(x => x.Roles, f => f.PickRandom(UserRoles.Roles, 1).ToList())
                .RuleFor(x => x.PersonData, f => person);

            var registerModel = registerModelMocker.Generate(1).First();
            return registerModel;
        }

        #endregion
    }
}
