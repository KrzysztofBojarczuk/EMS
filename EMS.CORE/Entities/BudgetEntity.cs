using EMS.CORE.Entities;

namespace EMS.APPLICATION.Dtos
{
    public class BudgetEntity
    {
        public Guid Id { get; set; }
        public string AppUserId { get; set; } = null!;
        public AppUserEntity AppUserEntity { get; set; } = null!;
        public decimal Budget { get; set; }
        public ICollection<TransactionEntity> TransactionEntity { get; set; } = new List<TransactionEntity>();
        public ICollection<PlannedExpenseEntity> PlannedExpenseEntity { get; set; } = new List<PlannedExpenseEntity>();

    }
}
