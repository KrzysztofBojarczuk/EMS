using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.APPLICATION.Dtos
{
    public class PlannedExpenseCreateDto
    {
        public string Name { get; set; } = null!;
        public decimal Amount { get; set; }
        public DateTimeOffset DueDate { get; set; }
    }
}
