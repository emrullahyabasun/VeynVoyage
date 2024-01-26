using System.ComponentModel.DataAnnotations;

namespace VeynVoyage.Areas.User.ViewModels
{
    public class ReservationViewModel
    {
        public int RoomId { get; set; }
        public string RoomName { get; set; }
        public string ImageUrl { get; set; }
        public int Price { get; set; }

        [Required(ErrorMessage = "Giriş tarihi zorunludur.")]
        [DataType(DataType.Date)]
        [Display(Name = "Giriş Tarihi")]
        public DateTime CheckInDate { get; set; }

        [Required(ErrorMessage = "Çıkış tarihi zorunludur.")]
        [DataType(DataType.Date)]
        [Display(Name = "Çıkış Tarihi")]
        public DateTime CheckOutDate { get; set; }

        [Required(ErrorMessage = "Misafir sayısı zorunludur.")]
        [Range(1, 10, ErrorMessage = "Misafir sayısı en az 1 en fazla 10 olabilir.")]
        [Display(Name = "Misafir Sayısı")]
        public int Guests { get; set; }
    }
}
