using EMS.APPLICATION.Features.Employee.Queries;
using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EMS.TESTS.FeaturesTests.EmployeeTests.QueriesTests
{
    [TestClass]
    public class GetUserEmployeeListsForTaskQueryHandlerTests
    {
        private readonly Mock<IEmployeeRepository> _mockEmployeeRepository;
        private readonly GetUserEmployeeListsForTaskQueryHandler _handler;

        public GetUserEmployeeListsForTaskQueryHandlerTests()
        {
            _mockEmployeeRepository = new Mock<IEmployeeRepository>();
            _handler = new GetUserEmployeeListsForTaskQueryHandler(_mockEmployeeRepository.Object);
        }

        [TestMethod]
        public async Task Handle_Returns_BySearchTerm_EmployeeLists()
        {
            // Arrange
            var appUserId = "user-id-123";
            var searchTerm = "dev";

            var employeeLists = new List<EmployeeListsEntity>
            {
                new EmployeeListsEntity
                {
                    Id = Guid.NewGuid(),
                    Name = "Dev Team",
                    AppUserId = appUserId,
                    EmployeesEntities = new List<EmployeeEntity>
                    {
                        new EmployeeEntity { Name = "Alice" },
                        new EmployeeEntity { Name = "Bob" }
                    }
                },
                new EmployeeListsEntity
                {
                    Id = Guid.NewGuid(),
                    Name = "QA and Dev Team",
                    AppUserId = appUserId,
                    EmployeesEntities = new List<EmployeeEntity>
                    {
                        new EmployeeEntity { Name = "Charlie" }
                    }
                }
            };

            _mockEmployeeRepository.Setup(x => x.GetUserEmployeeListsForTaskAsync(appUserId, searchTerm))
                .ReturnsAsync(employeeLists);

            var query = new GetUserEmployeeListsForTaskQuery(appUserId, searchTerm);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual("Dev Team", result.First().Name);
            _mockEmployeeRepository.Verify(x => x.GetUserEmployeeListsForTaskAsync(appUserId, searchTerm), Times.Once);
        }
    }
}