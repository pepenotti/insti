using Microsoft.AspNetCore.Identity;

namespace Insti.Data.Models
{
    public class Student : Person
    {
        public string UserId { get; set; }
        public IdentityUser User { get; set; }
    }
}
