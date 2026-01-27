using System.ComponentModel.DataAnnotations;

namespace EMS.APPLICATION.Dtos
{
    public class EmployeeCreateDto
    {
        [Required]
        [MinLength(3, ErrorMessage = "Name must be 3 characters")]
        [MaxLength(280, ErrorMessage = "Name cannot be over 280 characters")]
        public string Name { get; set; } = null!;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
        [Required]
        public string Phone { get; set; } = null!;
        [Range(0, 1000000)]
        public decimal? Salary { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public DateTime EmploymentDate { get; set; }
        [Required]
        public DateTime MedicalCheckValidUntil { get; set; }
        public Guid? EmployeeListId { get; set; }
    }
}