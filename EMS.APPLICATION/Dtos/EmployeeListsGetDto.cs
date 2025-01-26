using EMS.CORE.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.APPLICATION.Dtos
{
    public class EmployeeListsGetDto
    {
        public Guid Id { get; set; } //unikalny identyfikator
        public string Name { get; set; } = null!;
        public ICollection<EmployeeGetDto> Employees { get; set; } = new List<EmployeeGetDto>();
    }
}
