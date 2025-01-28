using EMS.CORE.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.CORE.Entities
{
    public class TaskEntity
    {
        public Guid Id { get; set; } //unikalny identyfikator
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; } 
        public StatusOfTask Status { get; set; } = StatusOfTask.Active;
        public string AppUserId { get; set; } = null!;
        public AppUserEntity AppUserEntity { get; set; } = null!;
        public Guid? AddressId { get; set; }
        public AddressEntity AddressEntity { get; set; } = null!;
        public ICollection<EmployeeListsEntity> EmployeeListsEntities { get; set; } = new List<EmployeeListsEntity>();
    }
}
