using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.CORE.Entities
{
    public class AppUserEntity : IdentityUser
    {
        public ICollection<TaskEntity> TaskEntity { get; set; } = new List<TaskEntity>();
        public ICollection<EmployeeEntity> EmployeeEntities { get; set; } = new List<EmployeeEntity>();
    }
}
