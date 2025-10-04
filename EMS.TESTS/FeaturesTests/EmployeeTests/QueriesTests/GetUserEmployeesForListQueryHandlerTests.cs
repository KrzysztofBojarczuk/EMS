using EMS.APPLICATION.Features.Employee.Queries;
using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace EMS.TESTS.Features.EmployeeTests.QueriesTests
{
    [TestClass]
    public class GetUserEmployeesForListQueryHandlerTests
    {
        private Mock<IEmployeeRepository> _mockEmployeeRepository;
        private GetUserEmployeesForListQueryHandler _handler;

        public GetUserEmployeesForListQueryHandlerTests()
        {
            _mockEmployeeRepository = new Mock<IEmployeeRepository>();
            _handler = new GetUserEmployeesForListQueryHandler(_mockEmployeeRepository.Object);
        }

        [TestMethod]
        public async Task Handle_Returns_AllEmployees()
        {
            // Arrange
            var appUserId = "user-id-123";

            var expectedEmployees = new List<EmployeeEntity>
            {
                new EmployeeEntity { Name = "John", Email = "john@example.com", Phone = "123", AppUserId = appUserId },
                new EmployeeEntity { Name = "Johnny", Email = "johnny@example.com", Phone = "456", AppUserId = appUserId },
                new EmployeeEntity { Name = "Tom", Email = "tom@example.com", Phone = "416", AppUserId = appUserId }
            };

            _mockEmployeeRepository.Setup(x => x.GetUserEmployeesForListAsync(appUserId, null))
                .ReturnsAsync(expectedEmployees);

            var query = new GetUserEmployeesForListQuery(appUserId, null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedEmployees.Count(), result.Count());
            CollectionAssert.AreEqual(expectedEmployees, result.ToList());
            _mockEmployeeRepository.Verify(x => x.GetUserEmployeesForListAsync(appUserId, null), Times.Once);
        }

        [TestMethod]
        public async Task Handle_Returns_BySearchTerm_Employees()
        {
            // Arrange
            var appUserId = "user-id-123";
            var searchTerm = "John";

            var expectedEmployees = new List<EmployeeEntity>
            {
                new EmployeeEntity { Name = "John", Email = "john@example.com", Phone = "123", AppUserId = appUserId },
                new EmployeeEntity { Name = "Johnny", Email = "johnny@example.com", Phone = "456", AppUserId = appUserId }
            };

            _mockEmployeeRepository.Setup(x => x.GetUserEmployeesForListAsync(appUserId, searchTerm))
                .ReturnsAsync(expectedEmployees);

            var query = new GetUserEmployeesForListQuery(appUserId, searchTerm);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedEmployees.Count(), result.Count());
            CollectionAssert.AreEqual(expectedEmployees, result.ToList());
            _mockEmployeeRepository.Verify(x => x.GetUserEmployeesForListAsync(appUserId, searchTerm), Times.Once);
        }

        [TestMethod]
        public async Task Handle_Returns_EmptyList_When_Employees_NotFound()
        {
            // Arrange
            var appUserId = "user-id-123";
            var searchTerm = "NonExistentName";

            _mockEmployeeRepository.Setup(x => x.GetUserEmployeesForListAsync(appUserId, searchTerm))
                .ReturnsAsync(new List<EmployeeEntity>());

            var query = new GetUserEmployeesForListQuery(appUserId, searchTerm);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
            _mockEmployeeRepository.Verify(x => x.GetUserEmployeesForListAsync(appUserId, searchTerm), Times.Once);
        }
    }
}