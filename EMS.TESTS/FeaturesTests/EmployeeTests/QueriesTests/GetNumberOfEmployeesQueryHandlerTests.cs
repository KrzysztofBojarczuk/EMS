using EMS.APPLICATION.Features.Employee.Queries;
using EMS.CORE.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EMS.TESTS.FeaturesTests.EmployeeTests.QueriesTests
{
    [TestClass]
    public class GetNumberOfEmployeesQueryHandlerTests
    {
        private Mock<IEmployeeRepository> _mockEmployeeRepository;
        private GetNumberOfEmployeesQueryHandler _handler;

        [TestInitialize]
        public void Setup()
        {
            _mockEmployeeRepository = new Mock<IEmployeeRepository>();
            _handler = new GetNumberOfEmployeesQueryHandler(_mockEmployeeRepository.Object);
        }

        [TestMethod]
        public async Task Handle_Returns_NumberOfEmployees()
        {
            // Arrange
            var expectedCount = 5;

            _mockEmployeeRepository.Setup(x => x.GetNumberOfEmployeesAsync())
                .ReturnsAsync(expectedCount);

            var query = new GetNumberOfEmployeesQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.AreEqual(expectedCount, result);
            _mockEmployeeRepository.Verify(x => x.GetNumberOfEmployeesAsync(), Times.Once);
        }

        [TestMethod]
        public async Task Handle_Returns_Zero_When_NoEmployees()
        {
            // Arrange
            _mockEmployeeRepository.Setup(x => x.GetNumberOfEmployeesAsync())
                .ReturnsAsync(0);

            var query = new GetNumberOfEmployeesQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.AreEqual(0, result);
            _mockEmployeeRepository.Verify(x => x.GetNumberOfEmployeesAsync(), Times.Once);
        }
    }
}