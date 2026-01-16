using EMS.CORE.Entities;
using EMS.CORE.Enums;

namespace EMS.CORE.Interfaces
{
    public interface ITransactionRepository
    {
        Task<TransactionEntity> AddTransactionAsync(TransactionEntity entity);
        Task<IEnumerable<TransactionEntity>> GetTransactionsByBudgetIdAsync(Guid id, string searchTerm, List<CategoryType> category, DateTime? dateFrom, DateTime? dateTo, decimal? amountFrom, decimal? amountTo, string sortOrder);
        Task<bool> DeleteTransactionsAsync(Guid transactionId);
    }
}