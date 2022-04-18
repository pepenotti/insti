using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Insti.API.Service.Interfaces
{
    public interface IAuthService
    {
        Task<JwtSecurityToken> Login(string userName, string password);
        Task Register(string userName, string email, string password, List<string> roles);
        int? VerifyToken(string token);
        JwtSecurityToken GetToken(List<Claim> authClaims);
    }
}
