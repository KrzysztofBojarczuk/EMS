using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.APPLICATION.Dtos
{
    public class EmployeeListsCreateDto
    {
        public string Name { get; set; } = null!;
        public List<Guid> EmployeeIds { get; set; } = new List<Guid>();
    }
}
