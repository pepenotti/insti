using Microsoft.AspNetCore.Identity;

namespace Insti.Data.Models
{
    public abstract class Person : BaseEntity
    {
        public string IdentityUserId { get; set; }
        public IdentityUser IdentityUser { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int GenderId{ get; set; }
        public Gender Gender { get; set; }
    }
}
