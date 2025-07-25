using EMS.APPLICATION.Features.Employee.Commands;
using EMS.CORE.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EMS.TESTS.FeaturesTests.EmployeeTests.CommandsTests
{
    [TestClass]
    public class DeleteEmployeeListCommandHandleTests
    {
        private Mock<IEmployeeRepository> _mockEmployeeRepository;
        private DeleteEmployeeListCommandHandler _handler;

        [TestInitialize]
        public void Setup()
        {
            _mockEmployeeRepository = new Mock<IEmployeeRepository>();
            _handler = new DeleteEmployeeListCommandHandler(_mockEmployeeRepository.Object);
        }

        [TestMethod]
        public async Task Handle_ShouldReturnTrue_When_EmployeeListIsDeletedSuccessfully()
        {
            // Arrange
            var employeeListId = Guid.NewGuid();
            var expectedResult = true;

            _mockEmployeeRepository.Setup(x => x.DeleteEmployeeListsAsync(employeeListId))
                .ReturnsAsync(expectedResult);

            var command = new DeleteEmployeeListCommand(employeeListId);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsTrue(result);
            _mockEmployeeRepository.Verify(x => x.DeleteEmployeeListsAsync(employeeListId), Times.Once);
        }

        [TestMethod]
        public async Task Handle_ShouldReturnFalse_When_EmployeeListDeletionFails()
        {
            // Arrange
            var employeeListId = Guid.NewGuid();
            var expectedResult = false;

            _mockEmployeeRepository.Setup(x => x.DeleteEmployeeListsAsync(employeeListId))
                .ReturnsAsync(expectedResult);

            var command = new DeleteEmployeeListCommand(employeeListId);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsFalse(result);
            _mockEmployeeRepository.Verify(x => x.DeleteEmployeeListsAsync(employeeListId), Times.Once);
        }
    }
}