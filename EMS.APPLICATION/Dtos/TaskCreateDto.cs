using System.ComponentModel.DataAnnotations;

namespace EMS.APPLICATION.Dtos
{
    public class TaskCreateDto
    {
        [Required]
        [MinLength(3, ErrorMessage = "Name must be 3 characters")]
        [MaxLength(280, ErrorMessage = "Name cannot be over 280 characters")]
        public string Name { get; set; } = null!;
        [Required]
        [MinLength(3, ErrorMessage = "Description must be 3 characters")]
        [MaxLength(2800, ErrorMessage = "Description cannot be over 2800 characters")]
        public string Description { get; set; } = null!;
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        public Guid? AddressId { get; set; }
        [Required]
        public List<Guid> EmployeeListIds { get; set; } = new List<Guid>();
        public List<Guid> VehicleIds { get; set; } = new List<Guid>();
    }
}