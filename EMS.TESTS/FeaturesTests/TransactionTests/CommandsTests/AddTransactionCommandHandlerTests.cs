using EMS.APPLICATION.Features.Transaction.Commands;
using EMS.CORE.Entities;
using EMS.CORE.Enums;
using EMS.CORE.Interfaces;
using MediatR;
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
            var mockMediator = new Mock<IPublisher>();
            _handler = new AddTransactioCommandHandler(_mockTransactionRepository.Object, mockMediator.Object);
        }

        [TestMethod]
        public async Task Handle_AddTransaction_And_Returns_Transaction()
        {
            // Arrange
            var transaction = new TransactionEntity
            {
                Id = Guid.NewGuid(),
                Name = "Test Transaction",
                CreationDate = DateTimeOffset.UtcNow,
                Category = CategoryType.Expense,
                Amount = 250.75m,
                BudgetId = Guid.NewGuid()
            };

            _mockTransactionRepository.Setup(x => x.AddTransactionAsync(transaction))
                .ReturnsAsync(transaction);

            var command = new AddTransactionCommand(transaction);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(transaction, result);
            _mockTransactionRepository.Verify(x => x.AddTransactionAsync(transaction), Times.Once);
        }
    }
}