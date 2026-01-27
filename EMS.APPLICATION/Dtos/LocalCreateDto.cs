using System.ComponentModel.DataAnnotations;

namespace EMS.APPLICATION.Dtos
{
    public class LocalCreateDto
    {
        [Required]
        [MinLength(3, ErrorMessage = "Description must be 3 characters")]
        [MaxLength(280, ErrorMessage = "Description cannot be over 280 characters")]
        public string Description { get; set; } = null!;
        public int LocalNumber { get; set; }
        public double Surface { get; set; }
        public bool NeedsRepair { get; set; }
    }
}