namespace EMS.CORE.Entities
{
    public class AddressEntity
    {
        public Guid Id { get; set; }
        public string City { get; set; } = null!;
        public string Street { get; set; } = null!;
        public string Number { get; set; } = null!;
        public string ZipCode { get; set; } = null!;
        public string AppUserId { get; set; } = null!;
        public AppUserEntity AppUserEntity { get; set; } = null!;
        public ICollection<TaskEntity> TaskEntities { get; set; } = new List<TaskEntity>();
    }
}
