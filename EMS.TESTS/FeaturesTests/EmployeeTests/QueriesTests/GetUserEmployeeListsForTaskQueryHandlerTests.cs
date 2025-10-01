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
        private Mock<IEmployeeRepository> _mockEmployeeRepository;
        private GetUserEmployeeListsForTaskQueryHandler _handler;

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

            var expectedEmployeeLists = new List<EmployeeListsEntity>
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
                .ReturnsAsync(expectedEmployeeLists);

            var query = new GetUserEmployeeListsForTaskQuery(appUserId, searchTerm);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedEmployeeLists.Count(), result.Count());
            CollectionAssert.AreEqual(expectedEmployeeLists, result.ToList());
            _mockEmployeeRepository.Verify(x => x.GetUserEmployeeListsForTaskAsync(appUserId, searchTerm), Times.Once);
        }

        [TestMethod]
        public async Task Handle_Returns_EmptyList_When_EmployeeLists_NotFound()
        {
            // Arrange
            var appUserId = "user-id-123";
            var searchTerm = "nonexistent";

            _mockEmployeeRepository.Setup(repo => repo.GetUserEmployeeListsForTaskAsync(appUserId, searchTerm))
                .ReturnsAsync(new List<EmployeeListsEntity>());

            var query = new GetUserEmployeeListsForTaskQuery(appUserId, searchTerm);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
            _mockEmployeeRepository.Verify(repo => repo.GetUserEmployeeListsForTaskAsync(appUserId, searchTerm), Times.Once);
        }
    }
}