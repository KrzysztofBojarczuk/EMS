using EMS.APPLICATION.Dtos;
using EMS.CORE.Entities;
using EMS.CORE.Enums;
using EMS.CORE.Interfaces;
using EMS.INFRASTRUCTURE.Data;
using EMS.INFRASTRUCTURE.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EMS.TESTS.RepositoriesTests
{
    [TestClass]
    public class TransactionRepositoryTests
    {
        private AppDbContext _context;
        private ITransactionRepository _repository;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
            _repository = new TransactionRepository(_context);
        }

        [TestMethod]
        public async Task AddTransactionAsync_Returns_Transaction()
        {
            // Arrange
            var transaction = new TransactionEntity
            {
                Name = "Invoice",
                CreationDate = DateTimeOffset.UtcNow,
                Category = CategoryType.Expense,
                Amount = 250.75m,
                BudgetId = Guid.NewGuid()
            };

            // Act
            var result = await _repository.AddTransactionAsync(transaction);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(transaction.Name, result.Name);
            Assert.AreEqual(transaction.CreationDate, result.CreationDate);
            Assert.AreEqual(transaction.Category, result.Category);
            Assert.AreEqual(transaction.Amount, result.Amount);
            Assert.AreEqual(transaction.BudgetId, result.BudgetId);
            Assert.AreNotEqual(Guid.Empty, result.Id);
            Assert.AreEqual(1, _context.Transactions.Count());
        }

        [TestMethod]
        public async Task DeleteTransactionsAsync_When_TransactionExists_Returns_True()
        {
            // Arrange
            var budget = new BudgetEntity
            {
                Id = Guid.NewGuid(),
                Budget = 1000m,
                AppUserId = "user-id-123"
            };

            _context.Budgets.Add(budget);

            var transaction = new TransactionEntity
            {
                Name = "Invoice",
                CreationDate = DateTimeOffset.UtcNow,
                Category = CategoryType.Expense,
                Amount = 250.75m,
                BudgetId = budget.Id 
            };

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            var transactionCountBefore = _context.Transactions.Count();

            // Act
            var result = await _repository.DeleteTransactionsAsync(transaction.Id);

            var transactionCountAfter = _context.Transactions.Count();

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(transactionCountBefore - 1, transactionCountAfter);
            Assert.AreEqual(0, _context.Transactions.Count());
        }

        [TestMethod]
        public async Task DeleteTransactionsAsync_When_TransactionDoesNotExist_Returns_False()
        {
            // Act
            var result = await _repository.DeleteTransactionsAsync(Guid.NewGuid());

            // Assert
            Assert.IsFalse(result);
        }
    }
}