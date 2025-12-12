using EMS.CORE.Enums;

namespace EMS.CORE.Entities
{
    public class VehicleEntity
    {
        public Guid Id { get; set; }
        public string Brand { get; set; } = null!;
        public string Model { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string RegistrationNumber { get; set; } = null!;
        public decimal Mileage { get; set; }
        public VehicleType VehicleType { get; set; }
        public DateTime DateOfProduction { get; set; }
        public DateTime InsuranceOcValidUntil { get; set; }
        public decimal InsuranceOcCost { get; set; }
        public DateTime TechnicalInspectionValidUntil { get; set; }
        public bool IsAvailable { get; set; } = true;
        public string AppUserId { get; set; } = null!;
        public AppUserEntity AppUserEntity { get; set; } = null!;
        public Guid? TaskId { get; set; }
        public TaskEntity TaskEntities { get; set; }
    }
}