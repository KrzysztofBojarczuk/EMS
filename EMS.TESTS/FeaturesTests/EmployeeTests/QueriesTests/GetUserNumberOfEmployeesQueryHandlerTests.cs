using EMS.APPLICATION.Features.Employee.Queries;
using EMS.CORE.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EMS.TESTS.FeaturesTests.EmployeeTests.QueriesTests
{
    [TestClass]
    public class GetUserNumberOfEmployeesQueryHandlerTests
    {
        private Mock<IEmployeeRepository> _mockEmployeeRepository;
        private GetUserNumberOfEmployeesQueryHandler _handler;

        [TestInitialize]
        public void Setup()
        {
            _mockEmployeeRepository = new Mock<IEmployeeRepository>();
            _handler = new GetUserNumberOfEmployeesQueryHandler(_mockEmployeeRepository.Object);
        }

        [TestMethod]
        public async Task Handle_Returns_NumberOfUserEmployees()
        {
            // Arrange
            var appUserId = "user-id-123";
            var expectedCount = 3;

            _mockEmployeeRepository.Setup(x => x.GetUserNumberOfEmployeesAsync(appUserId))
                .ReturnsAsync(expectedCount);

            var query = new GetUserNumberOfEmployeesQuery(appUserId);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.AreEqual(expectedCount, result);
            _mockEmployeeRepository.Verify(x => x.GetUserNumberOfEmployeesAsync(appUserId), Times.Once);
        }

        [TestMethod]
        public async Task Handle_Returns_Zero_When_UserHasNoEmployees()
        {
            // Arrange
            var appUserId = "user-id-123";
            _mockEmployeeRepository.Setup(repo => repo.GetUserNumberOfEmployeesAsync(appUserId))
                .ReturnsAsync(0);

            var query = new GetUserNumberOfEmployeesQuery(appUserId);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.AreEqual(0, result);
            _mockEmployeeRepository.Verify(repo => repo.GetUserNumberOfEmployeesAsync(appUserId), Times.Once);
        }
    }
}