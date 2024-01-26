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
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = model.Email, Email = model.Email };
                var createUserResult = await _userManager.CreateAsync(user, model.Password);

                if (createUserResult.Succeeded)
                {
                    var addToRoleResult = await _userManager.AddToRoleAsync(user, model.RoleName);
                    if (addToRoleResult.Succeeded)
                    {
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

            
            model.Roles = new SelectList(await _roleManager.Roles.ToListAsync(), "Name", "Name", model.RoleName);
            return View(model);
        }


    }
}


