using EMS.APPLICATION.Features.Transaction.Commands;
using EMS.CORE.Entities;
using EMS.CORE.Enums;
using EMS.CORE.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EMS.TESTS.FeaturesTests.TransactionTests.CommandsTests
{
    [TestClass]
    public class AddTransactionCommandHandlerTests
    {
        private Mock<ITransactionRepository> _mockTransactionRepository;
        private AddTransactioCommandHandler _handler;

        [TestInitialize]
        public void Setup()
        {
            _mockTransactionRepository = new Mock<ITransactionRepository>();
            _handler = new AddTransactioCommandHandler(_mockTransactionRepository.Object);
        }

        [TestMethod]
        public async Task Handle_AddTransaction_And_Returns_Transaction()
        {
            // Arrange
            var expectedTransaction = new TransactionEntity
            {
                Id = Guid.NewGuid(),
                Name = "Transaction",
                CreationDate = DateTime.UtcNow,
                Category = CategoryType.Expense,
                Amount = 250.75m,
                BudgetId = Guid.NewGuid()
            };

            _mockTransactionRepository.Setup(x => x.AddTransactionAsync(expectedTransaction))
                .ReturnsAsync(expectedTransaction);

            var command = new AddTransactionCommand(expectedTransaction);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedTransaction, result);
            _mockTransactionRepository.Verify(x => x.AddTransactionAsync(expectedTransaction), Times.Once);
        }
    }
}