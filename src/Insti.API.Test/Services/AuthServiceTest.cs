using Bogus;
using Insti.API.Service;
using Insti.API.Test.Helpers;
using Insti.Core.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Insti.API.Test.Services
{
    [TestClass]
    public class AuthServiceTest
    {
        private readonly IConfiguration configuration;
        private IdentityUser user;
        private List<IdentityUser> usersList;

        private const string PASSWORD_OK = "OK";
        private const string PASSWORD_NOT_OK = "NOT_OK";
        private const string USERNAME_NOT_OK = "wrong@email.com";
        private readonly string USERNAME_OK;

        public AuthServiceTest()
        {
            configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    { "JWT:Secret", "Secret" },
                    { "JWT:ValidIssuer", "ValidIssuer" },
                    { "JWT:ValidAudience", "ValidAudience" },
                    { "JWT:Expiration", "100000" }
                }).Build();

            user = new() { Id = "pepe", UserName = "Pepe", Email = "pepe@pepe.com", EmailConfirmed = true };
            usersList = new() { user };
            USERNAME_OK = user.UserName;
        }

        #region Login Tests

        [TestMethod]
        public async Task Login_Success()
        {
            // Arrange
            var userRolesOfUser = UserRoles.Roles.ToList();
            var userManagerMock = MockUserManager(userRolesOfUser);
            var roleManagerMock = MockRoleManager(userRolesOfUser);
            var authService = new AuthService(userManagerMock.Object, roleManagerMock.Object, configuration);

            // Act
            var jwtSecurityToken = await authService.Login(USERNAME_OK, PASSWORD_OK);

            //Assert
            Assert.IsNotNull(jwtSecurityToken);
            userRolesOfUser.ForEach(ur => 
                Assert.IsTrue(
                    jwtSecurityToken.Claims.Any(c =>
                        c.Type == ClaimTypes.Role &&
                        c.Value == ur)));
        }

        [TestMethod]
        [ExpectedException(typeof(UnauthorizedAccessException))]
        public async Task Login_Wrong_Password()
        {
            // Arrange
            var userRolesOfUser = UserRoles.Roles.ToList();
            var userManagerMock = MockUserManager(userRolesOfUser);
            var roleManagerMock = MockRoleManager(userRolesOfUser);
            var authService = new AuthService(userManagerMock.Object, roleManagerMock.Object, configuration);

            // Act
            var jwtSecurityToken = await authService.Login(USERNAME_OK, PASSWORD_NOT_OK);
        }

        [TestMethod]
        [ExpectedException(typeof(UnauthorizedAccessException))]
        public async Task Login_Wrong_UserName()
        {
            // Arrange
            var userRolesOfUser = UserRoles.Roles.ToList();
            var userManagerMock = MockUserManager(userRolesOfUser);
            var roleManagerMock = MockRoleManager(userRolesOfUser);
            var authService = new AuthService(userManagerMock.Object, roleManagerMock.Object, configuration);

            // Act
            var jwtSecurityToken = await authService.Login(USERNAME_NOT_OK, PASSWORD_OK);
        }

        #endregion

        #region Private Methods

        private Mock<UserManager<IdentityUser>> MockUserManager(List<string> userRoles)
        {
            var userManagerMock = UserManagerMocker.MockUserManager<IdentityUser>();

            userManagerMock.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .Returns((string userName) => Task.FromResult(usersList.FirstOrDefault(u => u.UserName == userName))!);

            userManagerMock.Setup(x => x.CheckPasswordAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
                .Returns((IdentityUser user, string password)
                            => Task.FromResult(usersList.Any(u => PASSWORD_OK == password && u.Id == user.Id)));

            userManagerMock.Setup(x => x.GetRolesAsync(It.IsAny<IdentityUser>()))
                .Returns((IdentityUser user) => Task.FromResult((IList<string>)userRoles));

            return userManagerMock;
        }

        private Mock<RoleManager<IdentityRole>> MockRoleManager(List<string> userRoles)
        {
            var store = new Mock<IRoleStore<IdentityRole>>();
            var mock = new Mock<RoleManager<IdentityRole>>(store.Object, null, null, null, null);

            mock.Setup(x => x.RoleExistsAsync(It.IsAny<string>()))
                .Returns((string role) => Task.FromResult(userRoles.Contains(role)));

            mock.Setup(x => x.CreateAsync(It.IsAny<IdentityRole>()))
                .Returns((IdentityRole role) => Task.FromResult(
                    userRoles.Contains(role.Name) ?
                        IdentityResult.Success :
                        IdentityResult.Failed()));

            return mock;
        }

        #endregion
    }
}
