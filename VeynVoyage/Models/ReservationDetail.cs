using System.ComponentModel.DataAnnotations.Schema;

namespace VeynVoyage.Models
{
    public class ReservationDetail
    {
        public int Id { get; set; }
        [ForeignKey("ReservationId")]
        public int ReservationId { get; set; }
        public Reservation? Reservation { get; set; }
        [ForeignKey("RoomId")]
        public int RoomId { get; set; }
        public Room? Room { get; set; }
    }
}
