using EMS.APPLICATION.Dtos;
using EMS.CORE.Enums;

namespace EMS.CORE.Entities
{
    public class TransactionEntity
    {
        public Guid Id { get; set; } 
        public string Name { get; set; } = null!;
        public DateTime CreationDate { get; set; }
        public CategoryType Category { get; set; }
        public decimal Amount { get; set; }
        public Guid BudgetId { get; set; }
        public BudgetEntity BudgetEntity { get; set; } = null!;
    }
}