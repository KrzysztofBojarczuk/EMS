using System.ComponentModel.DataAnnotations;

namespace EMS.APPLICATION.Dtos
{
    public class EmployeeListsCreateDto
    {

        [Required]
        [MinLength(3, ErrorMessage = "Name must be 3 characters")]
        [MaxLength(280, ErrorMessage = "Name cannot be over 280 characters")]
        public string Name { get; set; } = null!;
        [Required]
        public List<Guid> EmployeeIds { get; set; } = new List<Guid>();
    }
}