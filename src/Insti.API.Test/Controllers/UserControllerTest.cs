using Insti.API.Controllers;
using Insti.API.Test.Helpers;
using Insti.Core.Constants;
using Insti.Core.DTO.API.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Insti.API.Test.Controllers
{
    [TestClass]
    public class UserControllerTest
    {

        private const string WRONG_ROLE = "wrongRole";
        private const string WRONG_ROLE_CODE = "test";
        private IdentityUser user;

        public UserControllerTest()
        {
            user = new() { Id = "pepe", UserName = "Pepe" , Email = "pepe@pepe.com", EmailConfirmed = true };
        }

        [TestMethod]
        public async Task SetRoles_ValidRoles_Success()
        {
            //Arrange
            var repositoryMocks = new RepositoryMocker();
            repositoryMocks.Init();

            var userManagerMock = UserManagerMocker.MockUserManager(new List<IdentityUser> { user });
            var userRoles = UserRoles.Roles;
            var userRolesWereCleared = false;

            var getUserAsyncReturn = (ClaimsPrincipal x) => Task.FromResult(user);

            var removeFromRolesAsyncReturn = (IdentityUser identityUser, IEnumerable<string> roles) =>
            {
                userRoles = new List<string>();
                userRolesWereCleared = true;
                return Task.FromResult(new IdentityResult());
            };

            var addToRolesAsyncReturn = (IdentityUser user, IEnumerable<string> addedUserRoles) =>
            {
                var errors = new List<IdentityError>();

                foreach (var role in addedUserRoles)
                {
                    if (UserRoles.Roles.Contains(role))
                        userRoles.Add(role);
                    else
                        errors.Add(new IdentityError() { Code = WRONG_ROLE_CODE, Description = role });
                }

                return Task.FromResult(errors.Any() ? IdentityResult.Failed(errors.ToArray()) : IdentityResult.Success);
            };

            UserManagerMocker.InitUserManagerMock(ref userManagerMock, getUserAsyncReturn!, removeFromRolesAsyncReturn!, addToRolesAsyncReturn!);

            var userManager = userManagerMock.Object!;

            //Act
            var userController = new UserController(userManager, repositoryMocks.PersonRepository);
            var result = await userController.SetRoles(new SetRolesRequest { Roles = UserRoles.Roles }) as OkObjectResult;
            var resultValue = result!.Value as IdentityResult;
            
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 200);
            Assert.IsTrue(resultValue!.Succeeded);
            Assert.IsTrue(userRolesWereCleared);
            Assert.AreEqual(userRoles.Count, UserRoles.Roles.Count);
        }

        [TestMethod]
        public async Task SetRoles_InvalidRoles_Error()
        {
            //Arrange
            var repositoryMocks = new RepositoryMocker();
            repositoryMocks.Init();
            var userManagerMock = UserManagerMocker.MockUserManager(new List<IdentityUser> { user });
            var userRoles = UserRoles.Roles;
            var userRolesWereCleared = false;

            var roles = UserRoles.Roles.ToList();
            roles.Add(WRONG_ROLE);

            var getUserAsyncReturn = (ClaimsPrincipal x) => Task.FromResult(user);

            var removeFromRolesAsyncReturn = (IdentityUser identityUser, IEnumerable<string> roles) =>
            {
                userRoles = new List<string>();
                userRolesWereCleared = true;
                return Task.FromResult(new IdentityResult());
            };

            var addToRolesAsyncReturn = (IdentityUser user, IEnumerable<string> addedUserRoles) =>
            {
                var errors = new List<IdentityError>();

                foreach (var role in addedUserRoles)
                {
                    if (UserRoles.Roles.Contains(role))
                        userRoles.Add(role);
                    else
                        errors.Add(new IdentityError() { Code = WRONG_ROLE_CODE,Description = role });
                }

                return Task.FromResult(errors.Any() ? IdentityResult.Failed(errors.ToArray()) : IdentityResult.Success);
            };

            UserManagerMocker.InitUserManagerMock(ref userManagerMock, getUserAsyncReturn, removeFromRolesAsyncReturn!, addToRolesAsyncReturn!);

            var userManager = userManagerMock.Object!;

            //Act
            var userController = new UserController(userManager, repositoryMocks.PersonRepository);
            var result = await userController.SetRoles(new SetRolesRequest { Roles = roles }) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 200);

            var resultValue = result!.Value as IdentityResult; 
            Assert.IsTrue(!resultValue!.Succeeded);
            Assert.IsTrue(userRolesWereCleared);
            Assert.AreEqual(userRoles.Count, UserRoles.Roles.Count);

            var error = resultValue.Errors.First();
            Assert.AreEqual(error.Code, WRONG_ROLE_CODE);
            Assert.AreEqual(error.Description, WRONG_ROLE);
        }
    }
}
