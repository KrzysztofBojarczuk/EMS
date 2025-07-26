using EMS.APPLICATION.Features.Employee.Queries;
using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EMS.TESTS.FeaturesTests.EmployeeTests.QueriesTests
{
    [TestClass]
    public class GetEmployeeByIdQueryHandlerTests
    {
        private Mock<IEmployeeRepository> _mockEmployeeRepository;
        private GetEmployeeByIdQueryHandler _handler;

        [TestInitialize]
        public void Setup()
        {
            _mockEmployeeRepository = new Mock<IEmployeeRepository>();
            _handler = new GetEmployeeByIdQueryHandler(_mockEmployeeRepository.Object);
        }

        [TestMethod]
        public async Task Handle_Returns_Employee_When_Found()
        {
            // Arrange
            var employeeId = Guid.NewGuid();
            var expectedEmployee = new EmployeeEntity
            {
                Id = employeeId,
                Name = "Jane Doe",
                Email = "jane.doe@example.com",
                Phone = "555-1234"
            };

            _mockEmployeeRepository.Setup(x => x.GetEmployeeByIdAsync(employeeId))
                .ReturnsAsync(expectedEmployee);

            var query = new GetEmployeeByIdQuery(employeeId);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedEmployee.Id, result.Id);
            Assert.AreEqual(expectedEmployee.Name, result.Name);
            _mockEmployeeRepository.Verify(x => x.GetEmployeeByIdAsync(employeeId), Times.Once);
        }

    }
}