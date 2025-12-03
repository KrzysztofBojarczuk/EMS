namespace EMS.APPLICATION.Dtos
{
    public class ReservationGetDto
    {
        public Guid Id { get; set; }
        public string Description { get; set; } = null!;
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
    }
}