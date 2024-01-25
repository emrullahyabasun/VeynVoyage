using Microsoft.AspNetCore.Mvc;
using VeynVoyage.Data;
using VeynVoyage.Services.Interfaces;

namespace VeynVoyage.Areas.User.Controllers
{
    [Area("User")]
    public class RoomController : Controller
    {
        private readonly IRoomService _roomService;
        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        public IActionResult Index()
        {
            var rooms = _roomService.GetRooms();
            return View(rooms);

        }
    }
}
