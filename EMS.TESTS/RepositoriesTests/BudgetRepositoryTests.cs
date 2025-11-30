using EMS.APPLICATION.Dtos;
using EMS.CORE.Interfaces;
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
        private IBudgetRepository _repository;

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
                AppUserId = "user-id-123"
            };

            // Act
            var result = await _repository.AddBudgetAsync(budget);

            var budgetCount = await _context.Budgets.CountAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(budget.Budget, result.Budget);
            Assert.AreEqual(budget.AppUserId, result.AppUserId);
            Assert.AreNotEqual(Guid.Empty, result.Id);
            Assert.AreEqual(1, budgetCount);
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

        [TestMethod]
        public async Task DeleteBudgetAsync_When_BudgetExists_Returns_True()
        {
            // Arrange
            var appUserId = "user-id-123";

            var budget = new BudgetEntity
            {
                Id = Guid.NewGuid(),
                Budget = 2500.00m,
                AppUserId = appUserId
            };

            _context.Budgets.Add(budget);
            await _context.SaveChangesAsync();

            var budgetCountBefore = await _context.Budgets.CountAsync();

            // Act
            var result = await _repository.DeleteBudgetAsync(budget.Id, appUserId);

            var deletedBudget = await _context.Budgets.FirstOrDefaultAsync(x => x.Id == budget.Id && x.AppUserId == appUserId);

            var budgetCountAfter = await _context.Budgets.CountAsync();

            // Assert
            Assert.IsTrue(result);
            Assert.IsNull(deletedBudget);
            Assert.AreEqual(budgetCountBefore - 1, budgetCountAfter);
        }

        [TestMethod]
        public async Task DeleteBudgetAsync_When_BudgetDoesNotExist_Returns_False()
        {
            // Arrange
            var appUserId = "user-id-123";

            // Act
            var result = await _repository.DeleteBudgetAsync(Guid.NewGuid(), appUserId);

            // Assert
            Assert.IsFalse(result);
        }
    }
}