using EMS.APPLICATION.Features.Employee.Commands;
using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EMS.TESTS.FeaturesTests.EmployeeTests.CommandsTests
{
    [TestClass]
    public class UpdateEmployeeCommandHandlerTests
    {
        private Mock<IEmployeeRepository> _mockEmployeeRepository;
        private UpdateEmployeeCommandHandler _handler;

        [TestInitialize]
        public void Setup()
        {
            _mockEmployeeRepository = new Mock<IEmployeeRepository>();
            _handler = new UpdateEmployeeCommandHandler(_mockEmployeeRepository.Object);
        }

        [TestMethod]
        public async Task Handle_UpdateEmployee_Returns_UpdatedEmployee()
        {
            //Arrange
            var employeeId = Guid.NewGuid();
            var updatedEmployee = new EmployeeEntity
            {
                Name = "Test User",
                Email = "test@example.com",
                Phone = "123456789",
                AppUserId = "user-id-123"
            };

            _mockEmployeeRepository.Setup(x => x.UpdateEmployeeAsync(employeeId, updatedEmployee))
                .ReturnsAsync(updatedEmployee);

            var command = new UpdateEmployeeCommand(employeeId, updatedEmployee);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(updatedEmployee, result);
            _mockEmployeeRepository.Verify(x => x.UpdateEmployeeAsync(employeeId, updatedEmployee), Times.Once);
        }
    }
}