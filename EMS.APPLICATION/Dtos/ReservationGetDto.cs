namespace EMS.APPLICATION.Dtos
{
    public class ReservationGetDto
    {
        public Guid Id { get; set; }
        public DateTime? CheckInDate { get; set; }
        public DateTime? CheckOutDate { get; set; }
    }
}