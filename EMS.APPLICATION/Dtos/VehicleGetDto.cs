using EMS.CORE.Enums;

namespace EMS.APPLICATION.Dtos
{
    public  class VehicleGetDto
    {
        public Guid Id { get; set; }
        public string Brand { get; set; } = null!;
        public string Model { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string RegistrationNumber { get; set; } = null!;
        public decimal Mileage { get; set; }
        public VehicleType VehicleType { get; set; }
        public DateTime DateOfProduction { get; set; }
        public bool IsAvailable { get; set; } = true;
    }
}