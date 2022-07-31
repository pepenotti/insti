using System.ComponentModel.DataAnnotations;

namespace Insti.Core.DTO.API.Authentication
{
    public class PersonModel
    {
        [Required(ErrorMessage = "First Name is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        public string LastName { get; set; }

        public string GenderId { get; set; }
    }
}
