using EMS.APPLICATION.Features.Employee.Commands;
using EMS.CORE.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EMS.TESTS.FeaturesTests.EmployeeTests.CommandsTests
{
    [TestClass]
    public class DeleteEmployeeCommandHandlerTests
    {
        private Mock<IEmployeeRepository> _mockEmployeeRepository;
        private DeleteEmployeeCommandHandler _handler;

        [TestInitialize]
        public void Setup()
        {
            _mockEmployeeRepository = new Mock<IEmployeeRepository>();
            _handler = new DeleteEmployeeCommandHandler(_mockEmployeeRepository.Object);
        }

        [TestMethod]
        public async Task Handle_ShouldReturnTrue_When_EmployeeIsDeletedSuccessfully()
        {
            // Arrange
            var emplyeeId = Guid.NewGuid();
            var expectedResult = true;
            var appUserId = "user-id-123";

            _mockEmployeeRepository.Setup(x => x.DeleteEmployeeAsync(emplyeeId, appUserId))
                .ReturnsAsync(expectedResult);

            var command = new DeleteEmployeeCommand(emplyeeId, appUserId);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsTrue(result);
            _mockEmployeeRepository.Verify(x => x.DeleteEmployeeAsync(emplyeeId, appUserId), Times.Once);
        }

        [TestMethod]
        public async Task Handle_ShouldReturnFalse_When_EmployeeDeletionFails()
        {
            // Arrange
            var emplyeeId = Guid.NewGuid();
            var expectedResult = false;
            var appUserId = "user-id-123";

            _mockEmployeeRepository.Setup(x => x.DeleteEmployeeAsync(emplyeeId, appUserId))
                .ReturnsAsync(expectedResult);

            var command = new DeleteEmployeeCommand(emplyeeId, appUserId);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsFalse(result);
            _mockEmployeeRepository.Verify(x => x.DeleteEmployeeAsync(emplyeeId, appUserId), Times.Once);
        }
    }
}