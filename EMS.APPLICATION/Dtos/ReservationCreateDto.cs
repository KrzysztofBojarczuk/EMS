namespace EMS.APPLICATION.Dtos
{
    public class ReservationCreateDto
    {
        public Guid LocalId { get; set; }
        public DateTime? CheckInDate { get; set; }
        public DateTime? CheckOutDate { get; set; }
    }
}
