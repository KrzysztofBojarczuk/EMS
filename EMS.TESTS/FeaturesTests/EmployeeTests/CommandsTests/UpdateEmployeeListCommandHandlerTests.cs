using EMS.APPLICATION.Features.Employee.Commands;
using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EMS.TESTS.FeaturesTests.EmployeeTests.CommandsTests
{
    [TestClass]
    public class UpdateEmployeeListCommandHandlerTests
    {
        private Mock<IEmployeeRepository> _mockEmployeeRepository;
        private UpdateEmployeeListCommandHandler _handler;

        [TestInitialize]
        public void Setup()
        {
            _mockEmployeeRepository = new Mock<IEmployeeRepository>();
            _handler = new UpdateEmployeeListCommandHandler(_mockEmployeeRepository.Object);
        }

        [TestMethod]
        public async Task Handle_UpdateEmployeeList_When_ListAlreadyExists_Returns_Failure()
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

            _mockEmployeeRepository.Setup(x => x.EmployeeListExistsForUpdateAsync(expectedEmployeeList.Name, expectedEmployeeList.AppUserId, expectedEmployeeList.Id))
                .ReturnsAsync(true);

            var command = new UpdateEmployeeListCommand(expectedEmployeeList.Id, expectedEmployeeList.AppUserId, expectedEmployeeList, employeeIds);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual(expectedFailureMessage, result.Error);
            _mockEmployeeRepository.Verify(x => x.EmployeeListExistsForUpdateAsync(expectedEmployeeList.Name, expectedEmployeeList.AppUserId, expectedEmployeeList.Id), Times.Once);
            _mockEmployeeRepository.Verify(x => x.UpdateEmployeeListAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<EmployeeListsEntity>(), It.IsAny<List<Guid>>()), Times.Never);
        }
    }
}