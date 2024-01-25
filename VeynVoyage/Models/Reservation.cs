using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace VeynVoyage.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int TotalPrice { get; set; }
        public string? Image { get; set; }
        public bool IsActive { get; set; } = true;
        [ForeignKey("UserId")]
        public string? UserId { get; set; }
        public IdentityUser? User { get; set; }
        public List<ReservationDetail>? ReservationDetails { get; set; }
    }
}
