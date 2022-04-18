using System.ComponentModel.DataAnnotations;

namespace Insti.Core.DTO.API.Roles
{
    public class SetRoles
    {
        [Required(ErrorMessage = "User Name is required")]
        public List<string> Roles { get; set; }
    }
}