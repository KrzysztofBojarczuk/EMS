namespace EMS.CORE.Entities
{
    public class ReservationEntity
    {
        public Guid Id { get; set; }
        public string Description { get; set; } = null!;
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public Guid LocalId { get; set; }
        public LocalEntity LocalEntity { get; set; } = null!;
        public string? AppUserId { get; set; } = null!;
        public AppUserEntity AppUserEntity { get; set; } = null!;
    }
}