using EMS.CORE.Entities;
using EMS.CORE.Enums;
using EMS.CORE.Interfaces;
using EMS.INFRASTRUCTURE.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
            var transaction = await dbContext.Transactions.FirstOrDefaultAsync(t => t.Id == transactionId);

            var budget = await dbContext.Budgets.FirstOrDefaultAsync(b => b.Id == transaction.BudgetId);

            if (transaction is not null)
            {
                if (transaction.Category == CategoryType.Income)
                {
                    budget.Budget -= transaction.Amount;
                }
                else if (transaction.Category == CategoryType.Expense)
                {
                    budget.Budget += transaction.Amount;
                }

                dbContext.Transactions.Remove(transaction);

                return await dbContext.SaveChangesAsync() > 0; 
            }

            return false;
        }

        public async Task<IEnumerable<TransactionEntity>> GetTransactionsByBudgetIdAsync(Guid id, List<CategoryType> category, string searchTerm)
        {
            var query = dbContext.Transactions.AsQueryable();

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

            var income = transactions.Where(t => t.Category == CategoryType.Income).Sum(t => t.Amount);
            var expenses = transactions.Where(t => t.Category == CategoryType.Expense).Sum(t => t.Amount);

            var newBudget = income - expenses;

            var budgetEntity = await dbContext.Budgets.FirstOrDefaultAsync(b => b.Id == budgetId);

            if (budgetEntity is not null)
            {
                budgetEntity.Budget = newBudget;

                await dbContext.SaveChangesAsync();
            }
        }
    }
}
