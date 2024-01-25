using Microsoft.AspNetCore.Mvc;

namespace VeynVoyage.Areas.User.Controllers
{
    [Area("User")]
    public class HomeController : Controller
    {
        
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AboutUs()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }
    }
}
