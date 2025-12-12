namespace EMS.CORE.Entities
{
    public class EmployeeEntity
    {
        public Guid Id { get; set; } //unikalny identyfikator
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public decimal? Salary { get; set; }
        public int? Age { get; set; }
        public DateTime EmploymentDate { get; set; }
        public DateTime MedicalCheckValidUntil { get; set; }
        public string AppUserId { get; set; } = null!;
        public AppUserEntity AppUserEntity { get; set; } = null!;
        public Guid? EmployeeListId { get; set; }
        public EmployeeListsEntity EmployeeListsEntity { get; set; } = null!;
    }
}