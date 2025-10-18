using EMS.APPLICATION.Dtos;

namespace EMS.CORE.Interfaces
{
    public interface IBudgetRepository
    {
        Task<BudgetEntity> GetUserBudgetAsync(string appUserId);
        Task<BudgetEntity> AddBudgetAsync(BudgetEntity entity);
        Task<bool> DeleteBudgetAsync(Guid budgetId, string appUserId);
    }
}