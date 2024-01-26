using VeynVoyage.Data;
using VeynVoyage.Models;
using VeynVoyage.Services.Interfaces;

namespace VeynVoyage.Services.Implementations
{
    public class RoomService : IRoomService
    {
        private readonly ApplicationDbContext _context;
        public RoomService(ApplicationDbContext context)
        {
            _context = context;
        }
        public List<Room> GetRooms()
        {
            return _context.Rooms.ToList();
        }

        public Room GetRoomById(int id)
        {
            return _context.Rooms.FirstOrDefault(x => x.Id == id);
        }

       
    }
}
