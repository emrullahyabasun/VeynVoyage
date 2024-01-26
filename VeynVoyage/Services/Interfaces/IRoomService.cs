using VeynVoyage.Models;

namespace VeynVoyage.Services.Interfaces
{
    public interface IRoomService
    {
        List<Room> GetRooms();
        Room GetRoomById(int id);
    }
}
