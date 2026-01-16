using EMS.APPLICATION.Features.Transaction.Queries;
using EMS.CORE.Entities;
using EMS.CORE.Enums;
using EMS.CORE.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EMS.TESTS.FeaturesTests.TransactionTests.QueriesTests
{
    [TestClass]
    public class GetTransactionByBudgetIdQueryHandlerTests
    {
        private Mock<ITransactionRepository> _mockTransactionRepository;
        private GetTransactionsByIdBudgetIdQueryHandler _handler;

        public GetTransactionByBudgetIdQueryHandlerTests()
        {
            _mockTransactionRepository = new Mock<ITransactionRepository>();
            _handler = new GetTransactionsByIdBudgetIdQueryHandler(_mockTransactionRepository.Object);
        }

        [TestMethod]
        public async Task Handle_Returns_AllTransactions()
        {
            // Arrange
            var budgetId = Guid.NewGuid();
            var expectedTransactions = new List<TransactionEntity>
            {
                new TransactionEntity { Id = Guid.NewGuid(), Name = "Transaction 1", CreationDate = new DateTime(2026, 1, 15, 10, 0, 0), Category = CategoryType.Income, Amount = 200, BudgetId = budgetId },
                new TransactionEntity { Id = Guid.NewGuid(), Name = "Transaction 2", CreationDate = new DateTime(2026, 1, 15, 10, 0, 0), Category = CategoryType.Income, Amount = 50, BudgetId = budgetId },
                new TransactionEntity { Id = Guid.NewGuid(), Name = "Transaction 3", CreationDate = new DateTime(2026, 1, 15, 10, 0, 0), Category = CategoryType.Income, Amount = 50, BudgetId = budgetId }
            };

            _mockTransactionRepository.Setup(x => x.GetTransactionsByBudgetIdAsync(budgetId, null, null, null, null, null, null, null))
                .ReturnsAsync(expectedTransactions);

            var query = new GetTransactionsByBudgetIdQuery(budgetId, null, null, null, null, null, null, null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedTransactions.Count(), result.Count());
            CollectionAssert.AreEqual(expectedTransactions, result.ToList());
            _mockTransactionRepository.Verify(x => x.GetTransactionsByBudgetIdAsync(budgetId, null, null, null, null, null, null, null), Times.Once);
        }

        [TestMethod]
        public async Task Handle_Returns_BySearchTerm_Transactions()
        {
            // Arrange
            var budgetId = Guid.NewGuid();
            var searchTerm = "test";

            var expectedTransactions = new List<TransactionEntity>
            {
                new TransactionEntity { Id = Guid.NewGuid(), Name = "Transaction 1 Test", CreationDate = new DateTime(2026, 1, 15, 10, 0, 0), Category = CategoryType.Income, Amount = 200, BudgetId = budgetId },
                new TransactionEntity { Id = Guid.NewGuid(), Name = "Transaction 2", CreationDate = new DateTime(2026, 1, 15, 10, 0, 0), Category = CategoryType.Income, Amount = 50, BudgetId = budgetId },
            };

            _mockTransactionRepository.Setup(x => x.GetTransactionsByBudgetIdAsync(budgetId, searchTerm, null, null, null, null, null, null))
                .ReturnsAsync(expectedTransactions);

            var query = new GetTransactionsByBudgetIdQuery(budgetId, searchTerm, null, null, null, null, null, null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedTransactions.Count(), result.Count());
            CollectionAssert.AreEqual(expectedTransactions, result.ToList());
            _mockTransactionRepository.Verify(x => x.GetTransactionsByBudgetIdAsync(budgetId, searchTerm, null, null, null, null, null, null), Times.Once);
        }

        [TestMethod]
        public async Task Handle_Returns_EmptyList_When_Transactions_NotFound()
        {
            // Arrange
            var budgetId = Guid.NewGuid();
            var searchTerm = "nonexistent";

            _mockTransactionRepository.Setup(x => x.GetTransactionsByBudgetIdAsync(budgetId, searchTerm, null, null, null, null, null, null))
                .ReturnsAsync(new List<TransactionEntity>());

            var query = new GetTransactionsByBudgetIdQuery(budgetId, searchTerm, null, null, null, null, null, null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
            _mockTransactionRepository.Verify(x => x.GetTransactionsByBudgetIdAsync(budgetId, searchTerm, null, null, null, null, null, null), Times.Once);
        }

        [TestMethod]
        public async Task Handle_Returns_ByCategoryType_Transactions()
        {
            // Arrange
            var budgetId = Guid.NewGuid();
            var categoryType = new List<CategoryType> { CategoryType.Income };

            var expectedTransactions = new List<TransactionEntity>
            {
                new TransactionEntity { Id = Guid.NewGuid(), Name = "Transaction 1", CreationDate = new DateTime(2026, 1, 15, 10, 0, 0), Category = CategoryType.Income, Amount = 200, BudgetId = budgetId },
                new TransactionEntity { Id = Guid.NewGuid(), Name = "Transaction 2", CreationDate = new DateTime(2026, 1, 15, 10, 0, 0), Category = CategoryType.Income, Amount = 50, BudgetId = budgetId },
            };

            _mockTransactionRepository.Setup(x => x.GetTransactionsByBudgetIdAsync(budgetId, null, categoryType, null, null, null, null, null))
                .ReturnsAsync(expectedTransactions);

            var query = new GetTransactionsByBudgetIdQuery(budgetId, null, categoryType, null, null, null, null, null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedTransactions.Count(), result.Count());
            CollectionAssert.AreEqual(expectedTransactions, result.ToList());
            _mockTransactionRepository.Verify(x => x.GetTransactionsByBudgetIdAsync(budgetId, null, categoryType, null, null, null, null, null), Times.Once);
        }
    }
}