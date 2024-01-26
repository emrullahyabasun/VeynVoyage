using System.ComponentModel.DataAnnotations;

namespace VeynVoyage.Areas.Admin.ViewModel
{
    public class CreateRoleViewModel
    {
        [Required]
        public string RoleName { get; set; }


    }
}
