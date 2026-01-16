using EMS.APPLICATION.Dtos;
using Microsoft.AspNetCore.Identity;

namespace EMS.CORE.Entities
{
    public class AppUserEntity : IdentityUser
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<TaskEntity> TaskEntity { get; set; } = new List<TaskEntity>();
        public ICollection<EmployeeEntity> EmployeeEntities { get; set; } = new List<EmployeeEntity>();
        public ICollection<AddressEntity> AddressEntities { get; set; } = new List<AddressEntity>();
        public ICollection<EmployeeListsEntity> EmployeeListsEntities { get; set; } = new List<EmployeeListsEntity>();
        public ICollection<LocalEntity> LocalEntities { get; set; } = new List<LocalEntity>();
        public ICollection<ReservationEntity> ReservationsEntities { get; set; } = new List<ReservationEntity>();
        public ICollection<VehicleEntity> VehicleEntities { get; set; } = new List<VehicleEntity>();
        public BudgetEntity BudgetEntity { get; set; } = null!;
    }
}