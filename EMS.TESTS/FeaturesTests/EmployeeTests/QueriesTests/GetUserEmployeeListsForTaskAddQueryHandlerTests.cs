using EMS.APPLICATION.Features.Employee.Queries;
using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EMS.TESTS.FeaturesTests.EmployeeTests.QueriesTests
{
    [TestClass]
    public class GetUserEmployeeListsForTaskAddQueryHandlerTests
    {
        private Mock<IEmployeeRepository> _mockEmployeeRepository;
        private GetUserEmployeeListsForTaskAddQueryHandler _handler;

        public GetUserEmployeeListsForTaskAddQueryHandlerTests()
        {
            _mockEmployeeRepository = new Mock<IEmployeeRepository>();
            _handler = new GetUserEmployeeListsForTaskAddQueryHandler(_mockEmployeeRepository.Object);
        }

        [TestMethod]
        public async Task Handle_Returns_AllEmployeeLists()
        {
            // Arrange
            var appUserId = "user-id-123";

            var expectedEmployeeLists = new List<EmployeeListsEntity>
            {
                new EmployeeListsEntity
                {
                    Id = Guid.NewGuid(),
                    Name = "EmployeeList 1",
                    AppUserId = appUserId,
                    EmployeesEntities = new List<EmployeeEntity>
                    {
                        new EmployeeEntity { Name = "Employee 1" },
                        new EmployeeEntity { Name = "Employee 2" }
                    }
                },
                new EmployeeListsEntity
                {
                    Id = Guid.NewGuid(),
                    Name = "EmployeeList 2",
                    AppUserId = appUserId,
                    EmployeesEntities = new List<EmployeeEntity>
                    {
                        new EmployeeEntity { Name = "Employee 3" }
                    }
                },
                new EmployeeListsEntity
                {
                    Id = Guid.NewGuid(),
                    Name = "EmployeeList 3",
                    AppUserId = appUserId,
                    EmployeesEntities = new List<EmployeeEntity>
                    {
                        new EmployeeEntity { Name = "Employee 4" }
                    }
                }
            };

            _mockEmployeeRepository.Setup(x => x.GetUserEmployeeListsForTaskAddAsync(appUserId, null))
                .ReturnsAsync(expectedEmployeeLists);

            var query = new GetUserEmployeeListsForTaskAddQuery(appUserId, null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedEmployeeLists.Count(), result.Count());
            CollectionAssert.AreEqual(expectedEmployeeLists, result.ToList());
            _mockEmployeeRepository.Verify(x => x.GetUserEmployeeListsForTaskAddAsync(appUserId, null), Times.Once);
        }

        [TestMethod]
        public async Task Handle_Returns_BySearchTerm_EmployeeLists()
        {
            // Arrange
            var appUserId = "user-id-123";
            var searchTerm = "test";

            var expectedEmployeeLists = new List<EmployeeListsEntity>
            {
                new EmployeeListsEntity
                {
                    Id = Guid.NewGuid(),
                    Name = "EmployeeList 1 Test",
                    AppUserId = appUserId,
                    EmployeesEntities = new List<EmployeeEntity>
                    {
                        new EmployeeEntity { Name = "Employee 1" },
                        new EmployeeEntity { Name = "Employee 2" }
                    }
                },
                new EmployeeListsEntity
                {
                    Id = Guid.NewGuid(),
                    Name = "EmployeeList 2 Test",
                    AppUserId = appUserId,
                    EmployeesEntities = new List<EmployeeEntity>
                    {
                        new EmployeeEntity { Name = "Employee 3" }
                    }
                }
            };

            _mockEmployeeRepository.Setup(x => x.GetUserEmployeeListsForTaskAddAsync(appUserId, searchTerm))
                .ReturnsAsync(expectedEmployeeLists);

            var query = new GetUserEmployeeListsForTaskAddQuery(appUserId, searchTerm);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedEmployeeLists.Count(), result.Count());
            CollectionAssert.AreEqual(expectedEmployeeLists, result.ToList());
            _mockEmployeeRepository.Verify(x => x.GetUserEmployeeListsForTaskAddAsync(appUserId, searchTerm), Times.Once);
        }

        [TestMethod]
        public async Task Handle_Returns_EmptyList_When_EmployeeLists_NotFound()
        {
            // Arrange
            var appUserId = "user-id-123";
            var searchTerm = "nonexistent";

            _mockEmployeeRepository.Setup(x => x.GetUserEmployeeListsForTaskAddAsync(appUserId, searchTerm))
                .ReturnsAsync(new List<EmployeeListsEntity>());

            var query = new GetUserEmployeeListsForTaskAddQuery(appUserId, searchTerm);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
            _mockEmployeeRepository.Verify(x => x.GetUserEmployeeListsForTaskAddAsync(appUserId, searchTerm), Times.Once);
        }
    }
}