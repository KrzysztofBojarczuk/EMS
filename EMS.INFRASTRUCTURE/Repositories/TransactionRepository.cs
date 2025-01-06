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

        public async Task<IEnumerable<TransactionEntity>> GetTransactionsByBudgetIdAsync(Guid id)
        {
            return await dbContext.Transactions.Where(x => x.BudgetId == id).ToListAsync();
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
