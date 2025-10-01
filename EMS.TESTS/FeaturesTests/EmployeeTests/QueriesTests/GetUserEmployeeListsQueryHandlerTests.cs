using EMS.APPLICATION.Features.Employee.Queries;
using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EMS.TESTS.FeaturesTests.EmployeeTests.QueriesTests
{
    [TestClass]
    public class GetUserEmployeeListsQueryHandlerTests
    {
        private Mock<IEmployeeRepository> _mockEmployeeRepository;
        private GetUserEmployeeListsQueryHandler _handler;

        public GetUserEmployeeListsQueryHandlerTests()
        {
            _mockEmployeeRepository = new Mock<IEmployeeRepository>();
            _handler = new GetUserEmployeeListsQueryHandler(_mockEmployeeRepository.Object);
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

            _mockEmployeeRepository.Setup(x => x.GetUserEmployeeListsAsync(appUserId, searchTerm))
                .ReturnsAsync(expectedEmployeeLists);

            var query = new GetUserEmployeeListsQuery(appUserId, searchTerm);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedEmployeeLists.Count(), result.Count());
            CollectionAssert.AreEqual(expectedEmployeeLists.ToList(), result.ToList());
            _mockEmployeeRepository.Verify(x => x.GetUserEmployeeListsAsync(appUserId, searchTerm), Times.Once);
        }

        [TestMethod]
        public async Task Handle_Returns_EmptyList_When_EmployeeLists_NotFound()
        {
            // Arrange
            var appUserId = "user-id-123";
            var searchTerm = "nonexistent";

            _mockEmployeeRepository.Setup(x => x.GetUserEmployeeListsAsync(appUserId, searchTerm))
                .ReturnsAsync(new List<EmployeeListsEntity>());

            var query = new GetUserEmployeeListsQuery(appUserId, searchTerm);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
            _mockEmployeeRepository.Verify(x => x.GetUserEmployeeListsAsync(appUserId, searchTerm), Times.Once);
        }
    }
}