using EMS.APPLICATION.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.CORE.Entities
{
    public class PlannedExpenseEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Amount { get; set; }
        public DateTimeOffset DueDate { get; set; }
        public Guid BudgetId { get; set; }
        public BudgetEntity BudgetEntity { get; set; } = null!;
    }
}
