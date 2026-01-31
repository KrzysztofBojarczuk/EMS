using EMS.APPLICATION.Features.Employee.Commands;
using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
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
            _handler = new AddEmployeeListCommandHandler(_mockEmployeeRepository.Object);
        }

        [TestMethod]
        public async Task Handle_AddEmployeeList_When_ListAlreadyExists_Returns_Failure()
        {
            // Arrange
            var expectedEmployeeList = new EmployeeListsEntity
            {
                Id = Guid.NewGuid(),
                Name = "EmployeeList",
                AppUserId = "user-id-123"
            };

            var employeeIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };

            var expectedFailureMessage = "A list with that name already exists.";

            _mockEmployeeRepository.Setup(x => x.EmployeeListExistsForAddAsync(expectedEmployeeList.Name, expectedEmployeeList.AppUserId))
                .ReturnsAsync(true);

            var command = new AddEmployeeListCommand(expectedEmployeeList, employeeIds);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual(expectedFailureMessage, result.Error);
            _mockEmployeeRepository.Verify(x => x.EmployeeListExistsForAddAsync(expectedEmployeeList.Name, expectedEmployeeList.AppUserId), Times.Once);
            _mockEmployeeRepository.Verify(x => x.AddEmployeeListsAsync(It.IsAny<EmployeeListsEntity>(), It.IsAny<List<Guid>>()), Times.Never);
        }

        [TestMethod]
        public async Task Handle_AddEmployeeList_And_Returns_EmployeeList_Successful_Result()
        {
            // Arrange
            var expectedEmployeeList = new EmployeeListsEntity
            {
                Id = Guid.NewGuid(),
                Name = "EmployeeList",
                AppUserId = "user-id-123"
            };

            var employeeIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };

            _mockEmployeeRepository.Setup(x => x.EmployeeListExistsForAddAsync(expectedEmployeeList.Name, expectedEmployeeList.AppUserId))
                .ReturnsAsync(false);

            _mockEmployeeRepository.Setup(x => x.AddEmployeeListsAsync(expectedEmployeeList, employeeIds))
                .ReturnsAsync(expectedEmployeeList);

            var command = new AddEmployeeListCommand(expectedEmployeeList, employeeIds);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(expectedEmployeeList, result.Value);
            _mockEmployeeRepository.Verify(x => x.EmployeeListExistsForAddAsync(expectedEmployeeList.Name, expectedEmployeeList.AppUserId), Times.Once);
            _mockEmployeeRepository.Verify(x => x.AddEmployeeListsAsync(expectedEmployeeList, employeeIds), Times.Once);
        }
    }
}