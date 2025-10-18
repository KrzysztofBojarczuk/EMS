namespace EMS.APPLICATION.Dtos
{
    public class EmployeeGetDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public decimal? Salary { get; set; }
    }
}