using System.ComponentModel.DataAnnotations;

namespace EMS.APPLICATION.Dtos
{
    public class ReservationCreateDto
    {
        [Required]
        public Guid LocalId { get; set; }
        [Required]
        [MinLength(3, ErrorMessage = "Description must be 3 characters")]
        [MaxLength(280, ErrorMessage = "Description cannot be over 280 characters")]
        public string Description { get; set; } = null!;
        [Required]
        public DateTime CheckInDate { get; set; }
        [Required]
        public DateTime CheckOutDate { get; set; }
    }
}