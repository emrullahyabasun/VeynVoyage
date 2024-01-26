using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VeynVoyage.Areas.Admin.ViewModel;


namespace VeynVoyage.Areas.Admin.Controllers
{
	[Area("Admin")]
	//[Authorize(Roles = "Admin")]
	public class AdminController : Controller
	{
		private readonly UserManager<IdentityUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;

		public AdminController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
		{
			_userManager = userManager;
			_roleManager = roleManager;
		}
		public async Task<IActionResult> Index()
		{
			var users = await _userManager.Users.ToListAsync();
			var roles = await _roleManager.Roles.ToListAsync();

			var model = new AdminDashboardViewModel
			{
				Users = users,
				Roles = roles
			};

			return View(model);
		}

		[HttpGet]
		public IActionResult CreateRole()
		{
			return View();


		}

		[HttpPost]
		public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
		{
			if (ModelState.IsValid)
			{
				var createRoleResult = await _roleManager.CreateAsync(new IdentityRole(model.RoleName));

				if (createRoleResult.Succeeded)
				{

					return RedirectToAction("Index");


				}
				else
				{
					foreach (var error in createRoleResult.Errors)
					{

						ModelState.AddModelError(string.Empty, "Role already exists");
					}
				}
			}



			return View(model);
		}


		[HttpGet]
		public async Task<IActionResult> CreateUser()
		{
			var rolesSelectList = new SelectList(await _roleManager.Roles.ToListAsync(), "Name", "Name");
			var model = new CreateUserViewModel
			{
				Roles = rolesSelectList
			};
			return View(model);


		}

		[HttpPost]
		public async Task<IActionResult> CreateUser(CreateUserViewModel model)
		{
			model.Roles = new SelectList(await _roleManager.Roles.ToListAsync(), "Name", "Name");
			if (ModelState.IsValid)
			{
				var user = new IdentityUser { UserName = model.Email, Email = model.Email };
				var createUserResult = await _userManager.CreateAsync(user, model.Password);

				if (createUserResult.Succeeded)
				{
					var addToRoleResult = await _userManager.AddToRoleAsync(user, model.RoleName);
					if (addToRoleResult.Succeeded)
					{
						TempData["SuccessMessage"] = "Kullanıcı başarıyla oluşturuldu.";
						return RedirectToAction("Index");
					}
					else
					{
						foreach (var error in addToRoleResult.Errors)
						{
							ModelState.AddModelError(string.Empty, error.Description);
						}
					}
				}
				else
				{
					foreach (var error in createUserResult.Errors)
					{
						ModelState.AddModelError(string.Empty, error.Description);
					}
				}
			}

			TempData["ErrorMessage"] = "Kullanıcı oluşturulamadı.";
			return View(model);
		}



		[HttpGet]
		public async Task<IActionResult> EditUser(string id)
		{
			var user = await _userManager.FindByIdAsync(id);
			var userRoles = await _userManager.GetRolesAsync(user);

			var model = new EditUserViewModel
			{
				Id = user.Id,
				Email = user.Email,
				Roles = userRoles,
				AvailableRoles = new SelectList(await _roleManager.Roles.ToListAsync(), "Name", "Name")
			};

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> EditUser(EditUserViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByIdAsync(model.Id);
				user.Email = model.Email;
				user.UserName = model.Email;

				// Kullanıcı bilgilerini güncelle
				var result = await _userManager.UpdateAsync(user);
				if (result.Succeeded)
				{
					// Mevcut rolleri sil
					var currentRoles = await _userManager.GetRolesAsync(user);
					result = await _userManager.RemoveFromRolesAsync(user, currentRoles);
					if (!result.Succeeded)
					{
						// Rolleri silme hatası
						// Hataları ModelState'e ekle
						AddErrors(result);
					}

					// Yeni rolleri ekle
					result = await _userManager.AddToRolesAsync(user, model.SelectedRoles);
					if (result.Succeeded)
					{
						return RedirectToAction("Index");
					}
					else
					{
						// Rolleri ekleme hatası
						// Hataları ModelState'e ekle
						AddErrors(result);
					}
				}
				else
				{
					// Kullanıcı güncelleme hatası
					// Hataları ModelState'e ekle
					AddErrors(result);
				}
			}

			// Başarısızlık durumunda veya validation hatası durumunda, formu tekrar göster
			model.AvailableRoles = new SelectList(await _roleManager.Roles.ToListAsync(), "Name", "Name");
			return View(model);
		}

		private void AddErrors(IdentityResult result)
		{
			foreach (var error in result.Errors)
			{
				ModelState.AddModelError(string.Empty, error.Description);
			}
		}




	}
}


