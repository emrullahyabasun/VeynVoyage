using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace VeynVoyage.Areas.Admin.ViewModel
{
    public class CreateUserViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public string RoleName { get; set; }
        public IEnumerable<SelectListItem>? Roles { get; set; }
    }
}
