using EMS.APPLICATION.Features.Budget.Commands;
using EMS.CORE.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EMS.TESTS.FeaturesTests.BudgetTests.CommandsTests
{
    [TestClass]
    public class DeleteBudgetCommandHandlerTests
    {
        private Mock<IBudgetRepository> _mockBudgetRepository;
        private DeleteBudgetCommandHandler _handler;

        [TestInitialize]
        public void Setup()
        {
            _mockBudgetRepository = new Mock<IBudgetRepository>();
            _handler = new DeleteBudgetCommandHandler(_mockBudgetRepository.Object);
        }

        [TestMethod]
        public async Task Handle_ShouldReturnTrue_When_BudgetIsDeletedSuccessfully()
        {
            // Arrange
            var budgetId = Guid.NewGuid();
            var expectedResult = true;

            _mockBudgetRepository.Setup(x => x.DeleteBudgetAsync(budgetId)).ReturnsAsync(expectedResult);

            var command = new DeleteBudgetCommand(budgetId);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsTrue(result);
            _mockBudgetRepository.Verify(x => x.DeleteBudgetAsync(budgetId), Times.Once);
        }

        [TestMethod]
        public async Task Handle_ShouldReturnFalse_When_BudgetDeletionFails()
        {
            // Arrange
            var budgetId = Guid.NewGuid();
            var expectedResult = false;

            _mockBudgetRepository.Setup(x => x.DeleteBudgetAsync(budgetId)).ReturnsAsync(expectedResult);

            var command = new DeleteBudgetCommand(budgetId);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsFalse(result);
            _mockBudgetRepository.Verify(x => x.DeleteBudgetAsync(budgetId), Times.Once);
        }
    }
}