namespace EMS.APPLICATION.Dtos
{
    public class AddressGetDto
    {
        public Guid Id { get; set; }
        public string City { get; set; } = null!;
        public string Street { get; set; } = null!;
        public string Number { get; set; } = null!;
        public string ZipCode { get; set; } = null!;
    }
}