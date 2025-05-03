using EMS.CORE.Entities;
using EMS.CORE.Enums;

namespace EMS.CORE.Interfaces
{
    public interface ITransactionRepository
    {
        Task<TransactionEntity> AddTransactionAsync(TransactionEntity entity);
        Task<IEnumerable<TransactionEntity>> GetTransactionsByBudgetIdAsync(Guid id, List<CategoryType> category, string searchTerm);
        Task<bool> DeleteTransactionsAsync(Guid transactionId);
    }
}
