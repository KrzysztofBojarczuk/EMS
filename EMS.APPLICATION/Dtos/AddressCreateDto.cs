using System.ComponentModel.DataAnnotations;

namespace EMS.APPLICATION.Dtos
{
    public class AddressCreateDto
    {
        [Required]
        [MinLength(3, ErrorMessage = "City must be 3 characters")]
        [MaxLength(280, ErrorMessage = "City cannot be over 280 characters")]
        public string City { get; set; } = null!;
        [Required]
        [MinLength(3, ErrorMessage = "Street must be 3 characters")]
        [MaxLength(280, ErrorMessage = "Street cannot be over 280 characters")]
        public string Street { get; set; } = null!;
        [Required]
        public string Number { get; set; } = null!;
        [Required]
        [RegularExpression(@"^\d{2}-\d{3}$", ErrorMessage = "ZipCode must be in format XX-XXX")]
        public string ZipCode { get; set; } = null!;
    }
}