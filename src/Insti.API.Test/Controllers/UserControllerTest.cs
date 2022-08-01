using Bogus;
using Insti.API.Controllers;
using Insti.API.Test.Helpers;
using Insti.Core.Constants;
using Insti.Core.DTO.API.Authentication;
using Insti.Core.DTO.API.User;
using Insti.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Person = Insti.Data.Models.Person;

namespace Insti.API.Test.Controllers
{
    [TestClass]
    public class UserControllerTest
    {
        private const string WRONG_ROLE = "wrongRole";
        private const string WRONG_ROLE_CODE = "test";
        private IdentityUser user;
        private List<IdentityUser> usersList;

        public UserControllerTest()
        {
            user = new() { Id = "pepe", UserName = "Pepe" , Email = "pepe@pepe.com", EmailConfirmed = true };
            usersList  = new() { user };
        }

        #region Set Roles

        [TestMethod]
        public async Task SetRoles_ValidRoles_Success()
        {
            //Arrange
            var repositoryMocks = new RepositoryMocker();
            repositoryMocks.Init();

            var userManagerMock = UserManagerMocker.MockUserManager<IdentityUser>();
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
            var userManagerMock = UserManagerMocker.MockUserManager<IdentityUser>();
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

        #endregion

        #region Person

        [TestMethod]
        public async Task Get_Person()
        {
            //Arrange
            RepositoryMocker repositoryMocks;
            UserManager<IdentityUser> userManager;
            PersonArrange(out repositoryMocks, out userManager);

            //Act
            var userController = new UserController(userManager, repositoryMocks.PersonRepository);
            var result = await userController.GetPerson() as OkObjectResult;
            var resultValue = result!.Value as Person;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);

            Assert.IsNotNull(resultValue);
            Assert.AreEqual(user.Id, resultValue.IdentityUserId);
        }

        [TestMethod]
        public async Task Put_Person()
        {
            //Arrange
            PersonArrange(out RepositoryMocker repositoryMocks, out UserManager<IdentityUser> userManager);
            var personModelMocker = new Faker<PersonModel>()
                            .RuleFor(x => x.FirstName, f => f.Person.FirstName)
                            .RuleFor(x => x.LastName, f => f.Person.LastName)
                            .RuleFor(x => x.GenderId, f => "1");

            var newPersonData = personModelMocker.Generate(1).First();
            var storedPersonData = repositoryMocks.PersonRepository.GetByUserId(user.Id)!;
            var oldFirstName = storedPersonData.FirstName;
            var oldLastName = storedPersonData.LastName;
            var oldGenderId = storedPersonData.GenderId;

            //Act
            var userController = new UserController(userManager, repositoryMocks.PersonRepository);
            var result = await userController.UpdatePerson(newPersonData) as OkResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);

            Assert.AreNotEqual(oldFirstName, newPersonData.FirstName);
            Assert.AreNotEqual(oldLastName, newPersonData.LastName);

            Assert.AreEqual(storedPersonData.FirstName, newPersonData.FirstName);
            Assert.AreEqual(storedPersonData.LastName, newPersonData.LastName);
        }

        [TestMethod]
        public async Task Delete_Person()
        {
            //Arrange
            PersonArrange(out RepositoryMocker repositoryMocks, out UserManager<IdentityUser> userManager);
            
            var storedPersonData = repositoryMocks.PersonRepository.GetByUserId(user.Id)!;

            //Act
            var userController = new UserController(userManager, repositoryMocks.PersonRepository);
            var result = await userController.DeletePerson() as OkResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);

            var deletedPersonData = repositoryMocks.PersonRepository.GetByUserId(user.Id);

            Assert.IsNotNull(storedPersonData);
            Assert.IsNull(deletedPersonData);
        }

        private void PersonArrange(out RepositoryMocker repositoryMocks, out UserManager<IdentityUser> userManager)
        {
            repositoryMocks = new RepositoryMocker();
            repositoryMocks.Init(usersList);

            var userManagerMock = UserManagerMocker.MockUserManager<IdentityUser>();

            var getUserAsyncReturn = (ClaimsPrincipal x) => Task.FromResult(user);

            UserManagerMocker.InitUserManagerMock(ref userManagerMock, getUserAsyncReturn!, null, null);

            userManager = userManagerMock.Object!;
        }

        #endregion
    }
}
