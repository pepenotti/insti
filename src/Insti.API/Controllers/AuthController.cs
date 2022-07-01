using Insti.API.Service.Interfaces;
using Insti.Core.DTO.API.Authentication;
using Insti.Core.DTO.API.Common;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace Insti.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;

        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            try
            {
                var token = await authService.Login(model.Username, model.Password);

                return Ok(new LoginOKResponse
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    Expiration = token.ValidTo
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return new UnauthorizedObjectResult(new StatusResponse(StatusResponseTypes.Error, ex.Message));
            }
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            try
            {
                await authService.Register(model.Username, model.Email, model.Password, model.Roles);

                return Ok(new StatusResponse(StatusResponseTypes.Success, "User created successfully!"));

            }
            catch (BadHttpRequestException ex)
            {
                return BadRequest(new StatusResponse(StatusResponseTypes.Error, ex.Message));
            }
        }
    }
}
