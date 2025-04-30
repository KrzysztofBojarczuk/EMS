namespace EMS.APPLICATION.Dtos
{
    public class EmployeeListsGetDto
    {
        public Guid Id { get; set; } //unikalny identyfikator
        public string Name { get; set; } = null!;
        public ICollection<EmployeeGetDto> Employees { get; set; } = new List<EmployeeGetDto>();
    }
}
