namespace EMS.APPLICATION.Dtos
{
    public class AddressGetDto
    {
        public Guid Id { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public string ZipCode { get; set; }
    }
}
