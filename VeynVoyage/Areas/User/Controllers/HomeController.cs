using Microsoft.AspNetCore.Mvc;

namespace VeynVoyage.Areas.User.Controllers
{
    public class HomeController : Controller
    {
        [Area("User")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
