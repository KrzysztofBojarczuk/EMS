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
                Name = "Transaction",
                CreationDate = DateTime.UtcNow,
                Category = CategoryType.Expense,
                Amount = 250.75m,
                BudgetId = Guid.NewGuid()
            };

            // Act
            var result = await _repository.AddTransactionAsync(transaction);

            var transactionCount = await _context.Transactions.CountAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(transaction.Name, result.Name);
            Assert.AreEqual(transaction.CreationDate, result.CreationDate);
            Assert.AreEqual(transaction.Category, result.Category);
            Assert.AreEqual(transaction.Amount, result.Amount);
            Assert.AreEqual(transaction.BudgetId, result.BudgetId);
            Assert.AreNotEqual(Guid.Empty, result.Id);
            Assert.AreEqual(1, transactionCount);
        }

        [TestMethod]
        public async Task GetTransactionsByBudgetIdAsync_Returns_AllTransactions()
        {
            // Arrange
            var budgetId = Guid.NewGuid();

            var transactions = new List<TransactionEntity>
            {
                new TransactionEntity { Id = Guid.NewGuid(), Name = "Transaction 1", CreationDate = new DateTime(2026, 1, 15, 10, 0, 0), Category = CategoryType.Income, Amount = 100, BudgetId = budgetId },
                new TransactionEntity { Id = Guid.NewGuid(), Name = "Transaction 2", CreationDate = new DateTime(2026, 1, 15, 10, 0, 0), Category = CategoryType.Income, Amount = 500, BudgetId = budgetId },
                new TransactionEntity { Id = Guid.NewGuid(), Name = "Transaction 3", CreationDate = new DateTime(2026, 1, 15, 10, 0, 0), Category = CategoryType.Income, Amount = 200, BudgetId = budgetId },
                new TransactionEntity { Id = Guid.NewGuid(), Name = "Transaction 4", CreationDate = new DateTime(2026, 1, 15, 10, 0, 0), Category = CategoryType.Expense, Amount = 80, BudgetId = budgetId }
            };

            _context.Transactions.AddRange(transactions);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetTransactionsByBudgetIdAsync(budgetId, null, null, null, null, null, null, null);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.Count());
        }

        [TestMethod]
        public async Task GetTransactionsByBudgetIdAsync_BySearchTerm_Returns_Transactions()
        {
            // Arrange
            var budgetId = Guid.NewGuid();
            var searchTerm = "test";

            var transactions = new List<TransactionEntity>
            {
                new TransactionEntity { Id = Guid.NewGuid(), Name = "Transaction 1 Test", CreationDate = new DateTime(2026, 1, 15, 10, 0, 0), Category = CategoryType.Income, Amount = 100, BudgetId = budgetId },
                new TransactionEntity { Id = Guid.NewGuid(), Name = "Transaction 2 Test", CreationDate = new DateTime(2026, 1, 15, 10, 0, 0), Category = CategoryType.Income, Amount = 100, BudgetId = budgetId },
                new TransactionEntity { Id = Guid.NewGuid(), Name = "Transaction 3", CreationDate = new DateTime(2026, 1, 15, 10, 0, 0), Category = CategoryType.Income, Amount = 500, BudgetId = budgetId },
                new TransactionEntity { Id = Guid.NewGuid(), Name = "Transaction 4", CreationDate = new DateTime(2026, 1, 15, 10, 0, 0), Category = CategoryType.Expense, Amount = 20, BudgetId = budgetId }
             };

            _context.Transactions.AddRange(transactions);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetTransactionsByBudgetIdAsync(budgetId, searchTerm, null, null, null, null, null, null);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual(transactions[0].Name, result.First().Name);
        }

        [TestMethod]
        public async Task GetTransactionsByBudgetIdAsync_When_TransactionDoesNotExist_Returns_EmptyList()
        {
            // Arrange
            var budgetId = Guid.NewGuid();
            var searchTerm = "nonexistent";

            var transactions = new List<TransactionEntity>
            {
                new TransactionEntity { Id = Guid.NewGuid(), Name = "Transaction 1", CreationDate = new DateTime(2026, 1, 15, 10, 0, 0), Category = CategoryType.Income, Amount = 100, BudgetId = budgetId },
                new TransactionEntity { Id = Guid.NewGuid(), Name = "Transaction 2", CreationDate = new DateTime(2026, 1, 15, 10, 0, 0), Category = CategoryType.Income, Amount = 500, BudgetId = budgetId },
                new TransactionEntity { Id = Guid.NewGuid(), Name = "Transaction 3", CreationDate = new DateTime(2026, 1, 15, 10, 0, 0), Category = CategoryType.Expense, Amount = 20, BudgetId = budgetId }
             };

            _context.Transactions.AddRange(transactions);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetTransactionsByBudgetIdAsync(budgetId, searchTerm, null, null, null, null, null, null);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public async Task GetTransactionsByBudgetIdAsync_ByCategoryType_Returns_Transactions()
        {
            // Arrange
            var budgetId = Guid.NewGuid();
            var categoryType = new List<CategoryType> { CategoryType.Expense };

            var transactions = new List<TransactionEntity>
            {
                new TransactionEntity { Id = Guid.NewGuid(), Name = "Transaction 1", CreationDate = new DateTime(2026, 1, 15, 10, 0, 0), Category = CategoryType.Income, Amount = 100, BudgetId = budgetId },
                new TransactionEntity { Id = Guid.NewGuid(), Name = "Transaction 2", CreationDate = new DateTime(2026, 1, 15, 10, 0, 0), Category = CategoryType.Income, Amount = 500, BudgetId = budgetId },
                new TransactionEntity { Id = Guid.NewGuid(), Name = "Transaction 3", CreationDate = new DateTime(2026, 1, 15, 10, 0, 0), Category = CategoryType.Expense, Amount = 20, BudgetId = budgetId },
             };

            _context.Transactions.AddRange(transactions);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetTransactionsByBudgetIdAsync(budgetId, null, categoryType, null, null, null, null, null);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(transactions[2].Name, result.First().Name);
        }

        [TestMethod]
        public async Task GetTransactionsByBudgetIdAsync_ByDateRange_Returns_Transactions()
        {
            // Arrange
            var budgetId = Guid.NewGuid();
            var dateFrom = new DateTime(2010, 1, 1);
            var dateTo = new DateTime(2015, 1, 1);

            var transactions = new List<TransactionEntity>
            {
                new TransactionEntity { Id = Guid.NewGuid(), Name = "Transaction 1", CreationDate = new DateTime(2026, 1, 15, 10, 0, 0), Category = CategoryType.Income, Amount = 100, BudgetId = budgetId },
                new TransactionEntity { Id = Guid.NewGuid(), Name = "Transaction 2", CreationDate = new DateTime(2026, 1, 15, 10, 0, 0), Category = CategoryType.Income, Amount = 500, BudgetId = budgetId },
                new TransactionEntity { Id = Guid.NewGuid(), Name = "Transaction 3", CreationDate = new DateTime(2026, 1, 15, 10, 0, 0), Category = CategoryType.Income, Amount = 200, BudgetId = budgetId },
                new TransactionEntity { Id = Guid.NewGuid(), Name = "Transaction 4", CreationDate = new DateTime(2011, 1, 15, 10, 0, 0), Category = CategoryType.Income, Amount = 200, BudgetId = budgetId },
                new TransactionEntity { Id = Guid.NewGuid(), Name = "Transaction 5", CreationDate = new DateTime(2014, 1, 15, 10, 0, 0), Category = CategoryType.Income, Amount = 200, BudgetId = budgetId },
             };

            _context.Transactions.AddRange(transactions);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetTransactionsByBudgetIdAsync(budgetId, null, null, dateFrom, dateTo, null, null, null);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.All(x => x.CreationDate >= dateFrom && x.CreationDate <= dateTo));
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
                Id = Guid.NewGuid(),
                Name = "Test",
                CreationDate = DateTime.UtcNow,
                Category = CategoryType.Expense,
                Amount = 250.75m,
                BudgetId = budget.Id
            };

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            var transactionCountBefore = await _context.Transactions.CountAsync();

            // Act
            var result = await _repository.DeleteTransactionsAsync(transaction.Id);

            var deletedTransaction = await _context.Transactions.FirstOrDefaultAsync(x => x.Id == transaction.Id);

            var transactionCountAfter = await _context.Transactions.CountAsync();

            // Assert
            Assert.IsTrue(result);
            Assert.IsNull(deletedTransaction);
            Assert.AreEqual(transactionCountBefore - 1, transactionCountAfter);
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