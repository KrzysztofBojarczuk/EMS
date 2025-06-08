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
            var employeeToAdd = new EmployeeEntity
            {
                Name = "Test User",
                Email = "test@example.com",
                Phone = "123456789",
                AppUserId = "user123"
            };

            _mockEmployeeRepository
                .Setup(repo => repo.AddEmployeeAsync(employeeToAdd))
                .ReturnsAsync(employeeToAdd);

            var command = new AddEmployeeCommand(employeeToAdd);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(employeeToAdd, result);
            _mockEmployeeRepository.Verify(repo => repo.AddEmployeeAsync(employeeToAdd), Times.Once);
        }
    }
}