using EMS.APPLICATION.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.CORE.Interfaces
{
    public interface IBudgetRepository
    {
        Task<BudgetEntity> GetUserBudgetAsync(string appUserId);
        Task<BudgetEntity> AddBudgetAsync(BudgetEntity entity);
        Task<bool> DeleteBudgetAsync(Guid budgetId);
    }
}
