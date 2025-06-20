using EMS.APPLICATION.Dtos;
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
    }
}