namespace EMS.APPLICATION.Dtos
{
    public class EmployeeCreateDto
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public decimal? Salary { get; set; }
        public Guid? EmployeeListId { get; set; }
    }
}