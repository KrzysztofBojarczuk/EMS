using EMS.CORE.Entities;
using EMS.CORE.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.CORE.Interfaces
{
    public interface ITransactionRepository
    {
        Task<TransactionEntity> AddTransactionAsync(TransactionEntity entity);
        Task<IEnumerable<TransactionEntity>> GetTransactionsByBudgetIdAsync(Guid id, List<CategoryType> category, string searchTerm);
    }
}
