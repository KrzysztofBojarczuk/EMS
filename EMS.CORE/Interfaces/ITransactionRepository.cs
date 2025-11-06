using EMS.CORE.Entities;
using EMS.CORE.Enums;

namespace EMS.CORE.Interfaces
{
    public interface ITransactionRepository
    {
        Task<TransactionEntity> AddTransactionAsync(TransactionEntity entity);
        Task<IEnumerable<TransactionEntity>> GetTransactionsByBudgetIdAsync(Guid id, string searchTerm, List<CategoryType> category);
        Task<bool> DeleteTransactionsAsync(Guid transactionId);
    }
}