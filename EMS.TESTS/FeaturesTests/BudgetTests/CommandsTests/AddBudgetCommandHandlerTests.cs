using EMS.APPLICATION.Dtos;
using EMS.APPLICATION.Features.Budget.Commands;
using EMS.CORE.Interfaces;
using MediatR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EMS.TESTS.Features.BudgetTests.CommandsTests
{
    [TestClass]
    public class AddBudgetCommandHandlerTests
    {
        private Mock<IBudgetRepository> _mockBudgetRepository;
        private AddBudgetCommandHandler _handler;

        [TestInitialize]
        public void Setup()
        {
            _mockBudgetRepository = new Mock<IBudgetRepository>();
            var mockPublisher = new Mock<IPublisher>();
            _handler = new AddBudgetCommandHandler(_mockBudgetRepository.Object, mockPublisher.Object);
        }

        [TestMethod]
        public async Task Handle_AddBudget_And_Returns_Budget()
        {
            // Arrange
            var budget = new BudgetEntity
            {
                Id = Guid.NewGuid(),
                Budget = 5000.00m,
            };

            _mockBudgetRepository.Setup(x => x.AddBudgetAsync(budget))
                .ReturnsAsync(budget);

            var command = new AddBudgetCommand(budget);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(budget, result);
            _mockBudgetRepository.Verify(x => x.AddBudgetAsync(budget), Times.Once);
        }
    }
}