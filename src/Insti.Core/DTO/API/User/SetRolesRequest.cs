using System.ComponentModel.DataAnnotations;

namespace Insti.Core.DTO.API.User
{
    public class SetRolesRequest
    {
        [Required(ErrorMessage = "User Name is required")]
        public List<string> Roles { get; set; }
    }
}