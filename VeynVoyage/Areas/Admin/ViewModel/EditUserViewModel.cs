using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace VeynVoyage.Areas.Admin.ViewModel
{
    public class EditUserViewModel
    {
        public string Id { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        // Kullanıcının mevcut rolleri (string listesi olarak)
        public IEnumerable<string>? Roles { get; set; }

        // Tüm mevcut rolleri seçim listesi olarak tutar
        public SelectList? AvailableRoles { get; set; }

        // Kullanıcıya atamak için seçilen roller
        [Display(Name = "Roller")]
        public IEnumerable<string>? SelectedRoles { get; set; }
    }
}
