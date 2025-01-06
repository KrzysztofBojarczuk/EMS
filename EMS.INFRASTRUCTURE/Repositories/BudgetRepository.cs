﻿using EMS.APPLICATION.Dtos;
using EMS.CORE.Enums;
using EMS.CORE.Interfaces;
using EMS.INFRASTRUCTURE.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace EMS.INFRASTRUCTURE.Repositories
{
    public class BudgetRepository(AppDbContext dbContext) : IBudgetRepository
    {
        public async Task<BudgetEntity> AddBudgetAsync(BudgetEntity entity)
        {
            entity.Id = Guid.NewGuid();
            dbContext.Budgets.Add(entity);

            await dbContext.SaveChangesAsync();

            return entity;
        }

        public async Task<bool> DeleteBudgetAsync(Guid budgetId)
        {
            var budget = await dbContext.Budgets.FirstOrDefaultAsync(x => x.Id == budgetId);

            if (budget is not null)
            {
                dbContext.Budgets.Remove(budget);

                return await dbContext.SaveChangesAsync() > 0; //Jeśli usunięcie się powiodło: SaveChangesAsync() zwróci liczbę większą od 0, więc metoda zwróci true.
            }

            return false;
        }

        public async Task<BudgetEntity> GetUserBudgetAsync(string appUserId)
        {
            var budget = await dbContext.Budgets.FirstOrDefaultAsync(b => b.AppUserId == appUserId);

            return budget;
        }
    }
}
