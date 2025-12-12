using EMS.APPLICATION.Features.Employee.Commands;
using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using MediatR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EMS.TESTS.Features.EmployeeTests.CommandsTests
{
    [TestClass]
    public class AddEmployeeCommandHandlerTests
    {
        private Mock<IEmployeeRepository> _mockEmployeeRepository;
        private AddEmployeeCommandHandler _handler;

        [TestInitialize]
        public void Setup()
        {
            _mockEmployeeRepository = new Mock<IEmployeeRepository>();
            var mockMediator = new Mock<IPublisher>();
            _handler = new AddEmployeeCommandHandler(_mockEmployeeRepository.Object, mockMediator.Object);
        }

        [TestMethod]
        public async Task Handle_AddEmployee_And_Returns_Employee()
        {
            // Arrange
            var expectedEmployee = new EmployeeEntity
            {
                Name = "Test User",
                Email = "test@example.com",
                Phone = "123456789",
                Salary = 50000m,
                Age = 30,
                EmploymentDate = new DateTime(2022, 1, 1),
                MedicalCheckValidUntil = new DateTime(2025, 1, 1),
                AppUserId = "user-id-123"
            };

            _mockEmployeeRepository.Setup(x => x.AddEmployeeAsync(expectedEmployee))
                .ReturnsAsync(expectedEmployee);

            var command = new AddEmployeeCommand(expectedEmployee);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedEmployee, result);
            _mockEmployeeRepository.Verify(x => x.AddEmployeeAsync(expectedEmployee), Times.Once);
        }
    }
}