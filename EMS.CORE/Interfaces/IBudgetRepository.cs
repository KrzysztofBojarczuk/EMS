using EMS.APPLICATION.Dtos;

namespace EMS.CORE.Interfaces
{
    public interface IBudgetRepository
    {
        Task<BudgetEntity> AddBudgetAsync(BudgetEntity entity);
        Task<BudgetEntity> GetUserBudgetAsync(string appUserId);
        Task<bool> DeleteBudgetAsync(Guid budgetId, string appUserId);
    }
}