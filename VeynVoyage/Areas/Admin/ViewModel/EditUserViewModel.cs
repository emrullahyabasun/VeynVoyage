using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace VeynVoyage.Areas.Admin.ViewModel
{
    public class EditUserViewModel
    {
        public string Id { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

      
        public IEnumerable<string>? Roles { get; set; }

       
        public SelectList? AvailableRoles { get; set; }

       
        [Display(Name = "Roller")]
        public IEnumerable<string>? SelectedRoles { get; set; }
    }
}
