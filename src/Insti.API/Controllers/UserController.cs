using Insti.Core.Constants;
using Insti.Core.DTO.API.Authentication;
using Insti.Core.DTO.API.User;
using Insti.Data.Models;
using Insti.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Insti.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = UserRoles.Admin)]
    public class UserController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IPersonRepository personRepository;

        public UserController(
            UserManager<IdentityUser> userManager,
            IPersonRepository personRepository)
        {
            this.userManager = userManager;
            this.personRepository = personRepository;
        }

        [HttpPut]
        public async Task<IActionResult> SetRoles([FromBody] SetRolesRequest request)
        {
            var user = await GetUserIdentity();

            await userManager.RemoveFromRolesAsync(user, UserRoles.Roles);
            return new OkObjectResult(await userManager.AddToRolesAsync(user, request.Roles));
        }

        [HttpGet("/person/")]
        public async Task<IActionResult> GetPerson()
        {
            var user = await GetUserIdentity();

            return Ok(personRepository.GetByUserId(user.Id));
        }

        [HttpPut("/person/")]
        public async Task<IActionResult> UpdatePerson([FromBody] PersonModel personModel)
        {
            var user = await GetUserIdentity();
            var person = personRepository.GetByUserId(user.Id);

            personRepository.Update(UpdatePerson(person!, personModel));

            return Ok();
        }
        
        [HttpDelete("/person/")]
        public async Task<IActionResult> DeletePerson()
        {
            var user = await GetUserIdentity();
            personRepository.DeleteByUserId(user.Id);
            return Ok();
        }

        private Person UpdatePerson(Person oldPerson, PersonModel newPerson) 
        {
            oldPerson.FirstName = newPerson.FirstName;
            oldPerson.LastName = newPerson.LastName;
            oldPerson.GenderId = newPerson.GenderId;
            return oldPerson;
        }

        private Task<IdentityUser> GetUserIdentity() => userManager.GetUserAsync(User);
    }
}
