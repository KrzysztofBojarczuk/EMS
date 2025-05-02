namespace EMS.CORE.Entities
{
    public class EmployeeListsEntity
    {
        public Guid Id { get; set; } //unikalny identyfikator
        public string Name { get; set; } = null!;
        public string AppUserId { get; set; } = null!;
        public AppUserEntity AppUserEntity { get; set; } = null!;
        public ICollection<EmployeeEntity> EmployeesEntities { get; set; } = new List<EmployeeEntity>();
        public Guid? TaskId { get; set; }
        public TaskEntity TaskEntities { get; set; }
    }
}