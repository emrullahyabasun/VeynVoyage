using System.ComponentModel.DataAnnotations;

namespace VeynVoyage.Areas.Admin.ViewModel
{
	public class EditRoleViewModel
	{
		public string Id { get; set; }

		[Required(ErrorMessage = "Rol adı gereklidir.")]
		public string RoleName { get; set; }
	}
}
