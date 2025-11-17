using EMS.APPLICATION.Features.Employee.Queries;
using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using EMS.INFRASTRUCTURE.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EMS.TESTS.FeaturesTests.EmployeeTests.QueriesTests
{
    [TestClass]
    public class GetUserEmployeesQueryHandlerTests
    {
        private Mock<IEmployeeRepository> _mockEmployeeRepository;
        private GetUserEmployeesQueryHandler _handler;

        public GetUserEmployeesQueryHandlerTests()
        {
            _mockEmployeeRepository = new Mock<IEmployeeRepository>();
            _handler = new GetUserEmployeesQueryHandler(_mockEmployeeRepository.Object);
        }

        [TestMethod]
        public async Task Handle_Returns_AllEmployees()
        {
            // Arrange
            var appUserId = "user-id-123";
            var pageNumber = 1;
            var pageSize = 10;

            var expectedEmployees = new List<EmployeeEntity>
            {
                new EmployeeEntity { Name = "John", Email = "john@example.com", Phone = "123-456-789", Salary = 5000, AppUserId = appUserId },
                new EmployeeEntity { Name = "Johnny", Email = "johnny@example.com", Phone = "123-456-789", Salary = 5000, AppUserId = appUserId },
                new EmployeeEntity { Name = "Tom", Email = "tom@example.com", Phone = "123-456-789", Salary = 5000, AppUserId = appUserId },
            };

            var paginatedList = new PaginatedList<EmployeeEntity>(expectedEmployees, expectedEmployees.Count(), pageNumber, pageSize);

            _mockEmployeeRepository.Setup(x => x.GetUserEmployeesAsync(appUserId, pageNumber, pageSize, null, null))
                .ReturnsAsync(paginatedList);

            var query = new GetUserEmployeesQuery(appUserId, pageNumber, pageSize, null, null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedEmployees.Count(), result.Items.Count());
            CollectionAssert.AreEqual(expectedEmployees, result.Items.ToList());
            _mockEmployeeRepository.Verify(x => x.GetUserEmployeesAsync(appUserId, pageNumber, pageSize, null, null), Times.Once);
        }

        [TestMethod]
        public async Task Handle_Returns_BySearchTerm_Employees()
        {
            // Arrange
            var appUserId = "user-id-123";
            var pageNumber = 1;
            var pageSize = 10;
            var searchTerm = "John";

            var expectedEmployees = new List<EmployeeEntity>
            {
                new EmployeeEntity { Name = "John", Email = "john@example.com", Phone = "123-456-789", Salary = 5000, AppUserId = appUserId },
                new EmployeeEntity { Name = "Johnny", Email = "johnny@example.com", Phone = "123-456-789", Salary = 5000, AppUserId = appUserId }
            };

            var paginatedList = new PaginatedList<EmployeeEntity>(expectedEmployees, expectedEmployees.Count(), pageNumber, pageSize);

            _mockEmployeeRepository.Setup(x => x.GetUserEmployeesAsync(appUserId, pageNumber, pageSize, searchTerm, null))
                .ReturnsAsync(paginatedList);

            var query = new GetUserEmployeesQuery(appUserId, pageNumber, pageSize, searchTerm, null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedEmployees.Count(), result.Items.Count());
            CollectionAssert.AreEqual(expectedEmployees, result.Items.ToList());
            _mockEmployeeRepository.Verify(x => x.GetUserEmployeesAsync(appUserId, pageNumber, pageSize, searchTerm, null), Times.Once);
        }

        [TestMethod]
        public async Task Handle_Returns_EmptyList_When_Employees_NotFound()
        {
            // Arrange
            var appUserId = "user-id-123";
            var pageNumber = 1;
            var pageSize = 10;
            var searchTerm = "nonexistent";

            var paginatedList = new PaginatedList<EmployeeEntity>(new List<EmployeeEntity>(), 0, pageNumber, pageSize);

            _mockEmployeeRepository.Setup(x => x.GetUserEmployeesAsync(appUserId, pageNumber, pageSize, searchTerm, null))
                .ReturnsAsync(paginatedList);

            var query = new GetUserEmployeesQuery(appUserId, pageNumber, pageSize, searchTerm, null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Items.Count());
            _mockEmployeeRepository.Verify(x => x.GetUserEmployeesAsync(appUserId, pageNumber, pageSize, searchTerm, null), Times.Once);
        }

        [TestMethod]
        public async Task Handle_Returns_SortedBySalaryAscending_Employees()
        {
            // Arrange
            var appUserId = "user-id-123";
            var pageNumber = 1;
            var pageSize = 10;
            var sortOrder = "salary_asc";

            var expectedEmployees = new List<EmployeeEntity>
            {
                new EmployeeEntity { Name = "John", Email = "john@example.com", Phone = "123-456-789", Salary = 1000, AppUserId = appUserId },
                new EmployeeEntity { Name = "Johnny", Email = "johnny@example.com", Phone = "123-456-789", Salary = 5000, AppUserId = appUserId },
                new EmployeeEntity { Name = "Tom", Email = "tom@example.com", Phone = "123-456-789", Salary = 7000, AppUserId = appUserId },
            };

            var paginatedList = new PaginatedList<EmployeeEntity>(expectedEmployees, expectedEmployees.Count(), pageNumber, pageSize);

            _mockEmployeeRepository.Setup(x => x.GetUserEmployeesAsync(appUserId, pageNumber, pageSize, null, sortOrder))
                .ReturnsAsync(paginatedList);

            var query = new GetUserEmployeesQuery(appUserId, pageNumber, pageSize, null, sortOrder);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);
  
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedEmployees.Count(), result.Items.Count());
            CollectionAssert.AreEqual(expectedEmployees, result.Items.ToList());
            _mockEmployeeRepository.Verify(x => x.GetUserEmployeesAsync(appUserId, pageNumber, pageSize, null, sortOrder), Times.Once);
        }

        [TestMethod]
        public async Task Handle_Returns_SortedBySalaryDescending_Employees()
        {
            // Arrange
            var appUserId = "user-id-123";
            var pageNumber = 1;
            var pageSize = 10;
            var sortOrder = "salary_desc";

            var expectedEmployees = new List<EmployeeEntity>
            {
                new EmployeeEntity { Name = "John", Email = "john@example.com", Phone = "123-456-789", Salary = 7000, AppUserId = appUserId },
                new EmployeeEntity { Name = "Johnny", Email = "johnny@example.com", Phone = "123-456-789", Salary = 5000, AppUserId = appUserId },
                new EmployeeEntity { Name = "Tom", Email = "tom@example.com", Phone = "123-456-789", Salary = 1000, AppUserId = appUserId },
            };

            var paginatedList = new PaginatedList<EmployeeEntity>(expectedEmployees, expectedEmployees.Count(), pageNumber, pageSize);

            _mockEmployeeRepository.Setup(x => x.GetUserEmployeesAsync(appUserId, pageNumber, pageSize, null, sortOrder))
                .ReturnsAsync(paginatedList);

            var query = new GetUserEmployeesQuery(appUserId, pageNumber, pageSize, null, sortOrder);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedEmployees.Count(), result.Items.Count());
            CollectionAssert.AreEqual(expectedEmployees, result.Items.ToList());
            _mockEmployeeRepository.Verify(x => x.GetUserEmployeesAsync(appUserId, pageNumber, pageSize, null, sortOrder), Times.Once);
        }
    }
}