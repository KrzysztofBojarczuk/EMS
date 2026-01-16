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
            entity.CreationDate = DateTime.UtcNow;
            dbContext.Add(entity);

            await dbContext.SaveChangesAsync();

            await UpdateBudgetAsync(entity.BudgetId);

            return entity;
        }

        public async Task<IEnumerable<TransactionEntity>> GetTransactionsByBudgetIdAsync(Guid id, string searchTerm, List<CategoryType> category, DateTime? dateFrom, DateTime? dateTo, decimal? amountFrom, decimal? amountTo, string sortOrder)
        {
            var query = dbContext.Transactions.Where(x => x.BudgetId == id);

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(x => x.Name.ToLower().Contains(searchTerm.ToLower()));
            }

            if (category != null && category.Any())
            {
                query = query.Where(x => category.Contains(x.Category));
            }

            if (dateFrom.HasValue && dateTo.HasValue)
            {
                query = query.Where(x => x.CreationDate >= dateFrom.Value && x.CreationDate <= dateTo.Value);
            }

            if (amountFrom.HasValue && amountTo.HasValue)
            {
                query = query.Where(x => x.Amount >= amountFrom.Value && x.Amount <= amountTo.Value);
            }

            Console.WriteLine($"SortOrder: {sortOrder}");

            if (!string.IsNullOrEmpty(sortOrder))
            {
                switch (sortOrder)
                {
                    case "amount_asc":
                        query = query.OrderBy(x => x.Amount);
                        break;
                    case "amount_desc":
                        query = query.OrderByDescending(x => x.Amount);
                        break;
                    case "createdate_asc":
                        query = query.OrderBy(x => x.CreationDate);
                        break;
                    case "createdate_desc":
                        query = query.OrderByDescending(x => x.CreationDate);
                        break;
                    default:
                        query = query.OrderByDescending(x => x.CreationDate);
                        break;
                }
            }
            else
            {
                query = query.OrderByDescending(x => x.CreationDate);
            }

            return await query.ToListAsync();
        }

        private async Task UpdateBudgetAsync(Guid budgetId)
        {
            var transactions = await dbContext.Transactions.Where(x => x.BudgetId == budgetId).ToListAsync();

            var income = transactions.Where(x => x.Category == CategoryType.Income).Sum(x => x.Amount);
            var expenses = transactions.Where(x => x.Category == CategoryType.Expense).Sum(x => x.Amount);

            var newBudget = income - expenses;

            var budgetEntity = await dbContext.Budgets.FirstOrDefaultAsync(x => x.Id == budgetId);

            if (budgetEntity is not null)
            {
                budgetEntity.Budget = newBudget;

                await dbContext.SaveChangesAsync();
            }
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
    }
}