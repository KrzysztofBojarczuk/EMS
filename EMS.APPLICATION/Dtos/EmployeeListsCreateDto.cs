namespace EMS.APPLICATION.Dtos
{
    public class EmployeeListsCreateDto
    {
        public string Name { get; set; } = null!;
        public List<Guid> EmployeeIds { get; set; } = new List<Guid>();
    }
}
