using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Insti.Data.Models
{
    public class Professor : Person
    {
        public string UserId { get; set; }
        public virtual IdentityUser User { get; set; }

        public List<Module> Modules { get; set; }
    }
}
