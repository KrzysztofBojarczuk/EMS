using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.APPLICATION.Dtos
{
    public class BudgetGetDto
    {
        public Guid Id { get; set; }
        public decimal Budget { get; set; }
    }
}
