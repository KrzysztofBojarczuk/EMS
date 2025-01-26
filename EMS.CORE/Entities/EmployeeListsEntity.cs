﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.CORE.Entities
{
    public class EmployeeListsEntity
    {
        public Guid Id { get; set; } //unikalny identyfikator
        public string Name { get; set; } = null!;
        public string AppUserId { get; set; } = null!;
        public AppUserEntity AppUserEntity { get; set; } = null!;
        public ICollection<EmployeeEntity> EmployeesEntities { get; set; } = new List<EmployeeEntity>();
    }
}
