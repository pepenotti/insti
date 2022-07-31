using Microsoft.AspNetCore.Identity;

namespace Insti.Data.Models
{
    public class Person : BaseEntity
    {
        public string IdentityUserId { get; set; }
        public IdentityUser IdentityUser { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string GenderId { get; set; }
        public Gender Gender { get; set; }
    }
}
