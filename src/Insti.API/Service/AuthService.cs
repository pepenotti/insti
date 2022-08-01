using Insti.API.Service.Interfaces;
using Insti.Core.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Insti.API.Service
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        private readonly string jwtSecret;
        private readonly string jwtValidIssuer;
        private readonly string jwtValidAudience;
        private readonly int jwtExpiration;

        public AuthService(
           UserManager<IdentityUser> userManager,
           RoleManager<IdentityRole> roleManager,
           IConfiguration configuration,
           List<string>? roles = null)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;

            jwtSecret = configuration["JWT:Secret"];
            jwtValidIssuer = configuration["JWT:ValidIssuer"];
            jwtValidAudience = configuration["JWT:ValidAudience"];
            jwtExpiration = int.Parse(configuration["JWT:Expiration"]);
                
            // TODO: Check if this works :rolf:
            if(roles == null)
                UserRoles.Roles.ForEach(role => { Task t = InitializeRole(role); });
            else
                roles.ForEach(role => { Task t = InitializeRole(role); });

        }

        public async Task<JwtSecurityToken> Login(string userName, string password)
        {
            var user = await userManager.FindByNameAsync(userName);

            if (user == null || !await userManager.CheckPasswordAsync(user, password))
                throw new UnauthorizedAccessException("Wrong password or wrong user");

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            foreach (var userRole in await userManager.GetRolesAsync(user))
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            return GetToken(authClaims);
        }

        public async Task Register(string userName, string email, string password, List<string> roles)
        {
            if (await userManager.FindByNameAsync(userName) != null)
                throw new BadHttpRequestException("User with that name already exists");

            if(await userManager.FindByEmailAsync(email) != null)
                throw new BadHttpRequestException("User with that email address already exists");

            var user = new IdentityUser()
            {
                Email = email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = userName
            };

            var result = await userManager.CreateAsync(user, password);

            if (!result.Succeeded)
                throw new BadHttpRequestException("User creation failed! Please check user details and try again.");

            await userManager.AddToRolesAsync(user, roles);
        }

        public int? VerifyToken(string token)
        {

            var tokenHandler = new JwtSecurityTokenHandler();
            
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSecret)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var accountId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

                // return account id from JWT token if validation successful
                return accountId;
            }
            catch
            {
                // return null if validation fails
                return null;
            }
        }

        public JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));

            var token = new JwtSecurityToken(
                issuer: jwtValidIssuer,
                audience: jwtValidAudience,
                expires: DateTime.Now.AddHours(jwtExpiration),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }

        private async Task InitializeRole(string role)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}
