﻿using EMS.APPLICATION.Dtos;
using EMS.INFRASTRUCTURE.Data;
using EMS.INFRASTRUCTURE.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EMS.TESTS.RepositoriesTests
{
    [TestClass]
    public class BudgetRepositoryTests
    {
        private AppDbContext _context;
        private BudgetRepository _repository;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
            _repository = new BudgetRepository(_context);
        }

        [TestMethod]
        public async Task AddBudgetAsync_Returns_Budget()
        {
            // Arrange
            var budget = new BudgetEntity
            {
                Budget = 1500.00m,
                AppUserId = "user123"
            };

            // Act
            var result = await _repository.AddBudgetAsync(budget);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreNotEqual(Guid.Empty, result.Id);
            Assert.AreEqual(budget.Budget, result.Budget);
            Assert.AreEqual(budget.AppUserId, result.AppUserId);
            Assert.AreEqual(1, await _context.Budgets.CountAsync());
        }

        [TestMethod]
        public async Task DeleteBudgetAsync_When_BudgetExists_Returns_True()
        {
            // Arrange
            var budgetId = Guid.NewGuid();
            var budget = new BudgetEntity
            {
                Id = budgetId,
                Budget = 2500.00m,
                AppUserId = "user123"
            };

            _context.Budgets.Add(budget);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.DeleteBudgetAsync(budgetId);

            // Assert
            Assert.IsTrue(result);
            var deleted = await _context.Budgets.FirstOrDefaultAsync(x => x.Id == budgetId);
            Assert.IsNull(deleted);
            Assert.AreEqual(0, _context.Budgets.Count());
        }

        [TestMethod]
        public async Task DeleteBudgetAsync_When_BudgetDoesNotExist_Returns_False()
        {
            // Act
            var result = await _repository.DeleteBudgetAsync(Guid.NewGuid());

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task GetUserBudgetAsync_When_BudgetExists_Returns_Budget()
        {
            // Arrange
            var appUserId = "user-id-123";
            var budget = new BudgetEntity
            {
                Id = Guid.NewGuid(),
                Budget = 3000.00m,
                AppUserId = appUserId
            };

            _context.Budgets.Add(budget);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetUserBudgetAsync(appUserId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(budget.Id, result.Id);
            Assert.AreEqual(budget.Budget, result.Budget);
            Assert.AreEqual(budget.AppUserId, result.AppUserId);
        }

        [TestMethod]
        public async Task GetUserBudgetAsync_When_BudgetDoesNotExist_Returns_Null()
        {
            // Act
            var result = await _repository.GetUserBudgetAsync("nonexistent_user");

            // Assert
            Assert.IsNull(result);
        }
    }
}