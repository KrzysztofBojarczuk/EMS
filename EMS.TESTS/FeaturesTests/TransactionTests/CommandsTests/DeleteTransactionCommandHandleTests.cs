using EMS.APPLICATION.Features.Transaction.Commands;
using EMS.CORE.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EMS.TESTS.FeaturesTests.TransactionTests.CommandsTests
{
    [TestClass]
    public class DeleteTransactionCommandHandleTests
    {
        private Mock<ITransactionRepository> _mockTransactionRepository;
        private DeleteTransactionCommandHandler _handler;

        [TestInitialize]
        public void Setup()
        {
            _mockTransactionRepository = new Mock<ITransactionRepository>();
            _handler = new DeleteTransactionCommandHandler(_mockTransactionRepository.Object);
        }

        [TestMethod]
        public async Task Handle_ShouldReturnTrue_When_TransactionIsDeletedSuccessfully()
        {
            // Arrange
            var transactionId = Guid.NewGuid();
            var expectedResult = true;

            _mockTransactionRepository.Setup(x => x.DeleteTransactionsAsync(transactionId))
                .ReturnsAsync(expectedResult);

            var command = new DeleteTransactionCommand(transactionId);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsTrue(result);
            _mockTransactionRepository.Verify(x => x.DeleteTransactionsAsync(transactionId), Times.Once);
        }
    }
}