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
        private GetTransactionByIdBudgetIdQueryHandler _handler;

        public GetTransactionByBudgetIdQueryHandlerTests()
        {
            _mockTransactionRepository = new Mock<ITransactionRepository>();
            _handler = new GetTransactionByIdBudgetIdQueryHandler(_mockTransactionRepository.Object);
        }

        [TestMethod]
        public async Task Handle_Returns_AllTransactions()
        {
            // Arrange
            var budgetId = Guid.NewGuid();
            var expectedTransactions = new List<TransactionEntity>
            {
                new TransactionEntity { Id = Guid.NewGuid(), Name = "Salary", CreationDate = DateTime.UtcNow, Category = CategoryType.Income, Amount = 200, BudgetId = budgetId },
                new TransactionEntity { Id = Guid.NewGuid(), Name = "Salary", CreationDate = DateTime.UtcNow.AddMinutes(-1), Category = CategoryType.Income, Amount = 50, BudgetId = budgetId },
                new TransactionEntity { Id = Guid.NewGuid(), Name = "Samsung", CreationDate = DateTime.UtcNow.AddMinutes(-2), Category = CategoryType.Income, Amount = 50, BudgetId = budgetId }
            };

            _mockTransactionRepository.Setup(x => x.GetTransactionsByBudgetIdAsync(budgetId, null, null))
                .ReturnsAsync(expectedTransactions);

            var query = new GetTransactionByBudgetIdQuery(budgetId, null, null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedTransactions.Count(), result.Count());
            CollectionAssert.AreEqual(expectedTransactions, result.ToList());
            _mockTransactionRepository.Verify(x => x.GetTransactionsByBudgetIdAsync(budgetId, null, null), Times.Once);
        }

        [TestMethod]
        public async Task Handle_Returns_BySearchTerm_Transactions()
        {
            // Arrange
            var budgetId = Guid.NewGuid();
            var searchTerm = "Salary";

            var expectedTransactions = new List<TransactionEntity>
            {
                new TransactionEntity { Id = Guid.NewGuid(), Name = "Salary", CreationDate = DateTime.UtcNow, Category = CategoryType.Income, Amount = 200, BudgetId = budgetId },
                new TransactionEntity { Id = Guid.NewGuid(), Name = "Salary", CreationDate = DateTime.UtcNow.AddMinutes(-1), Category = CategoryType.Income, Amount = 50, BudgetId = budgetId },
            };

            _mockTransactionRepository.Setup(x => x.GetTransactionsByBudgetIdAsync(budgetId, searchTerm, null))
                .ReturnsAsync(expectedTransactions);

            var query = new GetTransactionByBudgetIdQuery(budgetId, searchTerm, null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedTransactions.Count(), result.Count());
            CollectionAssert.AreEqual(expectedTransactions, result.ToList());
            _mockTransactionRepository.Verify(x => x.GetTransactionsByBudgetIdAsync(budgetId, searchTerm, null), Times.Once);
        }

        [TestMethod]
        public async Task Handle_Returns_EmptyList_When_Transactions_NotFound()
        {
            // Arrange
            var budgetId = Guid.NewGuid();
            var searchTerm = "nonexistent";

            _mockTransactionRepository.Setup(x => x.GetTransactionsByBudgetIdAsync(budgetId, searchTerm, null))
                .ReturnsAsync(new List<TransactionEntity>());

            var query = new GetTransactionByBudgetIdQuery(budgetId, searchTerm, null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
            _mockTransactionRepository.Verify(x => x.GetTransactionsByBudgetIdAsync(budgetId, searchTerm, null), Times.Once);
        }

        [TestMethod]
        public async Task Handle_Returns_ByCategoryFilter_Transactions()
        {
            // Arrange
            var budgetId = Guid.NewGuid();
            var category = new List<CategoryType> { CategoryType.Income };

            var expectedTransactions = new List<TransactionEntity>
            {
                new TransactionEntity { Id = Guid.NewGuid(), Name = "Salary", CreationDate = DateTime.UtcNow, Category = CategoryType.Income, Amount = 200, BudgetId = budgetId },
                new TransactionEntity { Id = Guid.NewGuid(), Name = "Salary", CreationDate = DateTime.UtcNow.AddMinutes(-1), Category = CategoryType.Income, Amount = 50, BudgetId = budgetId },
            };

            _mockTransactionRepository.Setup(x => x.GetTransactionsByBudgetIdAsync(budgetId,null, category))
                .ReturnsAsync(expectedTransactions);

            var query = new GetTransactionByBudgetIdQuery(budgetId,null, category);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedTransactions.Count(), result.Count());
            CollectionAssert.AreEqual(expectedTransactions, result.ToList());
            _mockTransactionRepository.Verify(x => x.GetTransactionsByBudgetIdAsync(budgetId, null, category), Times.Once);
        }
    }
}