namespace VeynVoyage.Models
{
    public class Room
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Space { get; set; }
        public int Capacity { get; set; }
        public int Price { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public List<ReservationDetail>? ReservationDetails { get; set; }
    }
}
