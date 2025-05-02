using EMS.APPLICATION.Dtos;

namespace EMS.CORE.Entities
{
    public class PlannedExpenseEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Amount { get; set; }
        public decimal SavedAmount { get; private set; } 
        public DateTimeOffset DueDate { get; set; }
        public Guid BudgetId { get; set; }
        public BudgetEntity BudgetEntity { get; set; } = null!;
    }
}