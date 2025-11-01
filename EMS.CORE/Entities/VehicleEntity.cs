using EMS.CORE.Enums;

namespace EMS.CORE.Entities
{
    public class VehicleEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string RegistrationNumber { get; set; } = null!;
        public VehicleType VehicleType { get; set; }
        public DateTime LastServiceDate { get; set; }
        public bool IsAvailable { get; set; } = true;
        public string AppUserId { get; set; } = null!;
        public AppUserEntity AppUserEntity { get; set; } = null!;
        public Guid? TaskId { get; set; }
        public TaskEntity TaskEntities { get; set; }
    }
}