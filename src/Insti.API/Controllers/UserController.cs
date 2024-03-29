﻿using Insti.Core.Constants;
using Insti.Core.DTO.API.User;
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

        public UserController(UserManager<IdentityUser> userManager)
        {
            this.userManager = userManager;
        }

        [HttpPut]
        public async Task<IActionResult> SetRoles([FromBody] SetRolesRequest request)
        {
            var user = await GetUserIdentity();

            await userManager.RemoveFromRolesAsync(user, UserRoles.Roles);
            return new OkObjectResult(await userManager.AddToRolesAsync(user, request.Roles));
        }

        private Task<IdentityUser> GetUserIdentity() => userManager.GetUserAsync(User);
    }
}
