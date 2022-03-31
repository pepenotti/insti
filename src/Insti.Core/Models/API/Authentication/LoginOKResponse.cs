
namespace Insti.Core.Models.API.Authentication
{
    public class LoginOKResponse
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}