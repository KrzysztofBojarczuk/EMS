using EMS.APPLICATION.Features.Employee.Commands;
using EMS.CORE.Common;
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
        public async Task Handle_AddEmployeeList_Amd_Returns_EmployeeList_Successful_Result()
        {
            // Arrange
            var employeeList = new EmployeeListsEntity
            {
                Id = Guid.NewGuid(),
                Name = "Test List",
                AppUserId = "user-id-123"
            };

            var employeeIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };

            var expectedResult = Result<EmployeeListsEntity>.Success(employeeList);

            _mockEmployeeRepository
                .Setup(x => x.AddEmployeeListsAsync(employeeList, employeeIds))
                .ReturnsAsync(expectedResult);

            var command = new AddEmployeeListCommand(employeeList, employeeIds);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(employeeList, result.Value);
            _mockEmployeeRepository.Verify(x => x.AddEmployeeListsAsync(employeeList, employeeIds), Times.Once);
        }
    }
}