using Microsoft.AspNetCore.Identity;


namespace VeynVoyage.Areas.Admin.ViewModel
{
    public class AdminDashboardViewModel
    {
        public IEnumerable<IdentityUser> Users { get; set; }
        public IEnumerable<IdentityRole> Roles { get; set; }

		public IEnumerable<UserRolesViewModel> UsersWithRoles { get; set; }
	}
}
