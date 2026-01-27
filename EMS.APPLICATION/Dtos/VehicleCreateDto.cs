using EMS.CORE.Enums;
using System.ComponentModel.DataAnnotations;

namespace EMS.APPLICATION.Dtos
{
    public class VehicleCreateDto
    {
        [Required]
        [MinLength(3, ErrorMessage = "Brand must be 3 characters")]
        [MaxLength(280, ErrorMessage = "Brand cannot be over 280 characters")]
        public string Brand { get; set; } = null!;
        [Required]
        [MinLength(3, ErrorMessage = "Model must be 3 characters")]
        [MaxLength(280, ErrorMessage = "Model cannot be over 280 characters")]
        public string Model { get; set; } = null!;
        [Required]
        [MinLength(3, ErrorMessage = "Name must be 3 characters")]
        [MaxLength(280, ErrorMessage = "Name cannot be over 280 characters")]
        public string Name { get; set; } = null!;
        [Required]
        public string RegistrationNumber { get; set; } = null!;
        [Range(0, 1000000)]
        public decimal Mileage { get; set; }
        [Required]
        public VehicleType VehicleType { get; set; }
        [Required]
        public DateTime DateOfProduction { get; set; }
        [Required]
        public DateTime InsuranceOcValidUntil { get; set; }
        [Range(0, 1000000)]
        public decimal InsuranceOcCost { get; set; }
        [Required]
        public DateTime TechnicalInspectionValidUntil { get; set; }
        public bool IsAvailable { get; set; } = true;
    }
}