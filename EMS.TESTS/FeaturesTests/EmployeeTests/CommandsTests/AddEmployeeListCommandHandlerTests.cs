using EMS.APPLICATION.Features.Employee.Commands;
using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using MediatR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EMS.TESTS.FeaturesTests.EmployeeTests.CommandsTests
{
    [TestClass]
    public class AddEmployeeListCommandHandlerTests
    {
        private Mock<IEmployeeRepository> _mockEmployeeRepository;
        private AddEmployeeListCommandHandler _handler;

        [TestInitialize]
        public void Setup()
        {
            _mockEmployeeRepository = new Mock<IEmployeeRepository>();
            var mockMediator = new Mock<IPublisher>();
            _handler = new AddEmployeeListCommandHandler(_mockEmployeeRepository.Object, mockMediator.Object);
        }

        [TestMethod]
        public async Task Handle_AddEmployeeList_And_Returns_EmployeeList_Successful_Result()
        {
            // Arrange
            var employeeList = new EmployeeListsEntity
            {
                Id = Guid.NewGuid(),
                Name = "Test List",
                AppUserId = "user-id-123"
            };

            var employeeIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };

            _mockEmployeeRepository
               .Setup(x => x.EmployeeListExistsAsync(employeeList.Name, employeeList.AppUserId))
               .ReturnsAsync(false);

            _mockEmployeeRepository
               .Setup(x => x.AddEmployeeListsAsync(employeeList, employeeIds))
               .ReturnsAsync(employeeList);

            var command = new AddEmployeeListCommand(employeeList, employeeIds);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(employeeList, result.Value);
            _mockEmployeeRepository.Verify(x => x.AddEmployeeListsAsync(employeeList, employeeIds), Times.Once);
            _mockEmployeeRepository.Verify(x => x.EmployeeListExistsAsync(employeeList.Name, employeeList.AppUserId), Times.Once);
        }

        [TestMethod]
        public async Task Handle_AddEmployeeList_When_ListAlreadyExists_Returns_Failure()
        {
            // Arrange
            var employeeList = new EmployeeListsEntity
            {
                Id = Guid.NewGuid(),
                Name = "Duplicate List",
                AppUserId = "user-id-123"
            };

            var employeeIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };

            var expectedFailureMessage = "A list with that name already exists.";

            _mockEmployeeRepository
               .Setup(x => x.EmployeeListExistsAsync(employeeList.Name, employeeList.AppUserId))
               .ReturnsAsync(true);

            var command = new AddEmployeeListCommand(employeeList, employeeIds);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual(expectedFailureMessage, result.Error);
            _mockEmployeeRepository.Verify(x => x.EmployeeListExistsAsync(employeeList.Name, employeeList.AppUserId), Times.Once);
        }
    }
}