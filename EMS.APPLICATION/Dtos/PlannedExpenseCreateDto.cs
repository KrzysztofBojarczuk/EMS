using System.ComponentModel.DataAnnotations;

namespace EMS.APPLICATION.Dtos
{
    public class PlannedExpenseCreateDto
    {

        [Required]
        [MinLength(3, ErrorMessage = "Name must be 3 characters")]
        [MaxLength(280, ErrorMessage = "Name cannot be over 280 characters")]
        public string Name { get; set; } = null!;
        [Range(0, 1000000)]
        public decimal Amount { get; set; }
        [Required]
        public DateTime DueDate { get; set; }
    }
}