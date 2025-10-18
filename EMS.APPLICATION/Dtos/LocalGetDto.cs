namespace EMS.APPLICATION.Dtos
{
    public class LocalGetDto
    {
        public Guid Id { get; set; } //unikalny identyfikator
        public string Description { get; set; } = null!;
        public int LocalNumber { get; set; }
        public double Surface { get; set; }
        public bool NeedsRepair { get; set; }
        public ICollection<ReservationGetDto> Reservations{ get; set; } = new List<ReservationGetDto>();
    }
}