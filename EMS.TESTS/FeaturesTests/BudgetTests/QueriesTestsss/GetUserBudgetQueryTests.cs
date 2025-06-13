using EMS.APPLICATION.Dtos;
using EMS.APPLICATION.Features.Budget.Queries;
using EMS.CORE.Interfaces;
using MediatR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EMS.TESTS.FeaturesTests.BudgetTests.QueriesTestsss
{
    [TestClass]
    public class GetUserBudgetQueryTests
    {
        private Mock<IBudgetRepository> _mockBudgetRepository;
        private GetUserBudgetQueryHandler _handler;

        [TestInitialize]
        public void Setup()
        {
            _mockBudgetRepository = new Mock<IBudgetRepository>();
            var mockPublisher = new Mock<IPublisher>();
            _handler = new GetUserBudgetQueryHandler(_mockBudgetRepository.Object);
        }

        [TestMethod]
        public async Task Handle_Returns_UserBudget()
        {
            // Arrange
            var appUserId = "user123";
            var expectedBudget = new BudgetEntity
            {
                AppUserId = appUserId,
                Budget = 5000.00m,
            };

            _mockBudgetRepository
                .Setup(repo => repo.GetUserBudgetAsync(appUserId))
                .ReturnsAsync(expectedBudget);

            var query = new GetUserBudgetQuery(appUserId);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedBudget.AppUserId, result.AppUserId);
            Assert.AreEqual(expectedBudget.Budget, result.Budget);
            _mockBudgetRepository.Verify(repo => repo.GetUserBudgetAsync(appUserId), Times.Once);
        }
    }
}