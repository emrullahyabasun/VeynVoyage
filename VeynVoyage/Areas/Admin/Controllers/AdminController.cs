using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VeynVoyage.Areas.Admin.ViewModel;


namespace VeynVoyage.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
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
            var usersWithRoles = new List<UserRolesViewModel>();
            var roles = await _roleManager.Roles.ToListAsync();

            foreach (var user in _userManager.Users)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                usersWithRoles.Add(new UserRolesViewModel
                {
                    UserId = user.Id,
                    Email = user.Email,
                    Roles = userRoles
                });
            }

            var model = new AdminDashboardViewModel
            {
                UsersWithRoles = usersWithRoles,
                Roles = roles 
            };

            return View(model);
        }











        //ROLLER

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
		public async Task<IActionResult> EditRole(string id)
		{
			var role = await _roleManager.FindByIdAsync(id);
			if (role == null)
			{
				TempData["ErrorMessage"] = "Rol bulunamadı.";
				return RedirectToAction("Index");
			}

			var model = new EditRoleViewModel
			{
				Id = role.Id,
				RoleName = role.Name
			};

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> EditRole(EditRoleViewModel model)
		{
			if (ModelState.IsValid)
			{
				var role = await _roleManager.FindByIdAsync(model.Id);
				if (role == null)
				{
					TempData["ErrorMessage"] = "Rol bulunamadı.";
					return RedirectToAction("Index");
				}

				role.Name = model.RoleName;
				var result = await _roleManager.UpdateAsync(role);

				if (result.Succeeded)
				{
					TempData["SuccessMessage"] = "Rol başarıyla güncellendi.";
					return RedirectToAction("Index");
				}

				AddErrors(result);
			}

			return View(model);
		}



        [HttpGet]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                TempData["ErrorMessage"] = "Rol bulunamadı.";
                return RedirectToAction("Index");
            }

            var model = new DeleteRoleViewModel
            {
                Id = role.Id,
                RoleName = role.Name
            };

            return View(model);
        }

        [HttpPost, ActionName("DeleteRole")]
        public async Task<IActionResult> DeleteRoleConfirmed(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                TempData["ErrorMessage"] = "Rol bulunamadı.";
                return RedirectToAction("Index");
            }

            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Rol başarıyla silindi.";
            }
            else
            {
                TempData["ErrorMessage"] = "Rol silinirken bir hata oluştu: " + string.Join(", ", result.Errors.Select(e => e.Description));
            }

            return RedirectToAction("Index");
        }













        //User
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
                        TempData["ErrorMessage"] = string.Join("; ", addToRoleResult.Errors.Select(e => e.Description));
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = string.Join("; ", createUserResult.Errors.Select(e => e.Description));
                }
            }
            else
            {
                var errors = String.Join("; ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                TempData["ErrorMessage"] = $"Geçersiz form verileri: {errors}";
            }


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

                
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    
                    var currentRoles = await _userManager.GetRolesAsync(user);
                    result = await _userManager.RemoveFromRolesAsync(user, currentRoles);
                    if (!result.Succeeded)
                    {
                        
                        
                        AddErrors(result);
                    }

                   
                    result = await _userManager.AddToRolesAsync(user, model.SelectedRoles);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                       
                        AddErrors(result);
                    }
                }
                else
                {
                   
                    AddErrors(result);
                }
            }

            model.AvailableRoles = new SelectList(await _roleManager.Roles.ToListAsync(), "Name", "Name");
            if (model.SelectedRoles == null)
            {
                var user = await _userManager.FindByIdAsync(model.Id);
                model.SelectedRoles = await _userManager.GetRolesAsync(user);
            }
            return View(model);


        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"Kullanıcı ID'si {id} ile bulunamadı.";
                return View("NotFound");
            }

            var model = new DeleteUserViewModel
            {
                Id = user.Id,
                Email = user.Email
            };

            return View(model);
        }

        
        [HttpPost, ActionName("DeleteUser")]
        public async Task<IActionResult> DeleteUserConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                TempData["ErrorMessage"] = "Kullanıcı bulunamadı.";
                return RedirectToAction("Index");
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = $"{user.Email} başarıyla silindi.";
            }
            else
            {
                TempData["ErrorMessage"] = "Kullanıcı silinemedi: " + string.Join(", ", result.Errors.Select(e => e.Description));
            }

            return RedirectToAction("Index");
        }




    }
}


