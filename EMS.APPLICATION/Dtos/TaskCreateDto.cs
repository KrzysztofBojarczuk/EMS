namespace EMS.APPLICATION.Dtos
{
    public class TaskCreateDto
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid? AddressId { get; set; }
        public List<Guid> EmployeeListIds { get; set; } = new List<Guid>();
        public List<Guid> VehicleIds { get; set; } = new List<Guid>();
    }
}