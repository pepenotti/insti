using Insti.Core.Models.API.Authentication;
using Insti.Data.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

/// <summary>

/// </summary>
namespace Insti.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration configuration;

        public AuthController(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.configuration = configuration;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await userManager.FindByNameAsync(model.Username);

            if(user == null || !await userManager.CheckPasswordAsync(user, model.Password))
                return Unauthorized();

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            foreach (var userRole in await userManager.GetRolesAsync(user))
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var token = GetToken(authClaims);

            return Ok(new LoginOKResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo
            });
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var userExists = await userManager.FindByNameAsync(model.Username) != null;

            if (userExists)
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new AuthResponse { Status = AuthResponseStatus.Error, Message = "User already exists!" });

            if (!await roleManager.RoleExistsAsync(model.Role))
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new AuthResponse { Status = AuthResponseStatus.Error, Message = "Invalid role, check user details and try again." });

            var result = await userManager.CreateAsync(new IdentityUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            }, model.Password);

            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new AuthResponse { Status = AuthResponseStatus.Error, Message = "User creation failed! Please check user details and try again." });


            return Ok(new AuthResponse { Status = AuthResponseStatus.Success, Message = "User created successfully!" });
        }

        [HttpPost]
        [Route("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
        {
            var userExists = await userManager.FindByNameAsync(model.Username);

            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new AuthResponse { Status = AuthResponseStatus.Error, Message = "User already exists!" });

            var user = new IdentityUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };

            var result = await userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new AuthResponse { Status = AuthResponseStatus.Error, Message = $"User creation failed! {string.Join(" | ", result.Errors.Select(e => e.Description))}" });
            
            await InitializeRoles();

            await userManager.AddToRoleAsync(user, UserRoles.Admin);
            await userManager.AddToRoleAsync(user, UserRoles.Student);
            await userManager.AddToRoleAsync(user, UserRoles.Professor);
            await userManager.AddToRoleAsync(user, UserRoles.Monitor);
            
            return Ok(new AuthResponse { Status = AuthResponseStatus.Success, Message = "User created successfully!" });
        }

        private async Task InitializeRoles()
        {
            if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));

            if (!await roleManager.RoleExistsAsync(UserRoles.Student))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.Student));

            if (!await roleManager.RoleExistsAsync(UserRoles.Professor))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.Professor));

            if (!await roleManager.RoleExistsAsync(UserRoles.Monitor))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.Monitor));
        }

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: configuration["JWT:ValidIssuer"],
                audience: configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }
    }
}
