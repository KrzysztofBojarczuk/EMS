namespace EMS.APPLICATION.Dtos
{
    public class EmployeeGetDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public decimal? Salary { get; set; }
        public int? Age { get; set; }
        public DateTime EmploymentDate { get; set; }
        public DateTime MedicalCheckValidUntil { get; set; }
    }
}