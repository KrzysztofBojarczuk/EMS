using EMS.CORE.Entities;
using EMS.CORE.Enums;
using EMS.CORE.Interfaces;
using EMS.INFRASTRUCTURE.Data;
using Microsoft.EntityFrameworkCore;

namespace EMS.INFRASTRUCTURE.Repositories
{
    public class TransactionRepository(AppDbContext dbContext) : ITransactionRepository
    {
        public async Task<TransactionEntity> AddTransactionAsync(TransactionEntity entity)
        {
            entity.Id = Guid.NewGuid();

            dbContext.Add(entity);
            await dbContext.SaveChangesAsync();

            await UpdateBudgetAsync(entity.BudgetId);

            return entity;
        }

        public async Task<bool> DeleteTransactionsAsync(Guid transactionId)
        {
            var transaction = await dbContext.Transactions.FirstOrDefaultAsync(x => x.Id == transactionId);

            if (transaction is null)
            {
                return false;
            }

            var budget = await dbContext.Budgets.FirstOrDefaultAsync(x => x.Id == transaction.BudgetId);

            if (budget is not null)
            {
                if (transaction.Category == CategoryType.Income)
                {
                    budget.Budget -= transaction.Amount;
                }
                else if (transaction.Category == CategoryType.Expense)
                {
                    budget.Budget += transaction.Amount;
                }
            }

            dbContext.Transactions.Remove(transaction);
            return await dbContext.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<TransactionEntity>> GetTransactionsByBudgetIdAsync(Guid id, List<CategoryType> category, string searchTerm)
        {
            var query = dbContext.Transactions.OrderByDescending(x => x.CreationDate).AsQueryable();

            query = query.Where(x => x.BudgetId == id);

            if (category.Any())
            {
                query = query.Where(x => category.Contains(x.Category));
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(x => x.Name.Contains(searchTerm));
            }

            return await query.ToListAsync();
        }

        private async Task UpdateBudgetAsync(Guid budgetId)
        {
            var transactions = await dbContext.Transactions
                .Where(t => t.BudgetId == budgetId)
                .ToListAsync();

            var income = transactions.Where(x => x.Category == CategoryType.Income).Sum(t => t.Amount);
            var expenses = transactions.Where(x => x.Category == CategoryType.Expense).Sum(t => t.Amount);

            var newBudget = income - expenses;

            var budgetEntity = await dbContext.Budgets.FirstOrDefaultAsync(x => x.Id == budgetId);

            if (budgetEntity is not null)
            {
                budgetEntity.Budget = newBudget;

                await dbContext.SaveChangesAsync();
            }
        }
    }
}
