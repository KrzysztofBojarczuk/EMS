namespace EMS.APPLICATION.Dtos
{
    public class ReservationCreateDto
    {
        public Guid LocalId { get; set; }
        public string Description { get; set; } = null!;
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
    }
}