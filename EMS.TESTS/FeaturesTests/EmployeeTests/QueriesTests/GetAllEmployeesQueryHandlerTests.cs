using EMS.APPLICATION.Features.Employee.Queries;
using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using EMS.INFRASTRUCTURE.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EMS.TESTS.FeaturesTests.EmployeeTests.QueriesTests
{
    [TestClass]
    public class GetAllEmployeesQueryHandlerTests
    {
        private Mock<IEmployeeRepository> _mockEmployeeRepository;
        private GetAllEmployeesQueryHandler _handler;

        public GetAllEmployeesQueryHandlerTests()
        {
            _mockEmployeeRepository = new Mock<IEmployeeRepository>();
            _handler = new GetAllEmployeesQueryHandler(_mockEmployeeRepository.Object);
        }

        [TestMethod]
        public async Task Handle_Returns_AllEmployees()
        {
            // Arrange
            var pageNumber = 1;
            var pageSize = 10;

            var expectedEmployees = new List<EmployeeEntity>
            {
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "John", Email = "john@example.com", Phone = "123-456-789", Salary = 5000, DateOfBirth = new DateTime(1990, 1, 1), EmploymentDate = new DateTime(2022, 1, 1), MedicalCheckValidUntil = new DateTime(2025, 1, 1) },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Bob", Email = "bob@example.com", Phone = "123-456-789", Salary = 5000, DateOfBirth = new DateTime(1991, 1, 1), EmploymentDate = new DateTime(2022, 2, 2), MedicalCheckValidUntil = new DateTime(2025, 2, 2) },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Tom", Email = "tom@example.com", Phone = "123-456-789", Salary = 5000, DateOfBirth = new DateTime(1992, 1, 1), EmploymentDate = new DateTime(2022, 3, 3), MedicalCheckValidUntil = new DateTime(2025, 3, 3) },
            };

            var paginatedList = new PaginatedList<EmployeeEntity>(expectedEmployees, expectedEmployees.Count(), pageNumber, pageSize);

            _mockEmployeeRepository.Setup(x => x.GetEmployeesAsync(pageNumber, pageSize, null))
                .ReturnsAsync(paginatedList);

            var query = new GetAllEmployeesQuery(pageNumber, pageSize, null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedEmployees.Count(), result.Items.Count());
            CollectionAssert.AreEqual(expectedEmployees, result.Items.ToList());
            _mockEmployeeRepository.Verify(x => x.GetEmployeesAsync(pageNumber, pageSize, null), Times.Once);
        }

        [TestMethod]
        public async Task Handle_Returns_BySearchTerm_Employees()
        {
            // Arrange
            var pageNumber = 1;
            var pageSize = 10;
            var searchTerm = "John";

            var expectedEmployees = new List<EmployeeEntity>
            {
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "John", Email = "john@example.com", Phone = "123-456-789", Salary = 5000, DateOfBirth = new DateTime(1990, 1, 1), EmploymentDate = new DateTime(2022, 1, 1), MedicalCheckValidUntil = new DateTime(2025, 1, 1) },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Johnny", Email = "johnny@example.com", Phone = "123-456-789", Salary = 5000, DateOfBirth = new DateTime(1990, 1, 1), EmploymentDate = new DateTime(2022, 2, 2), MedicalCheckValidUntil = new DateTime(2025, 2, 2) },
            };

            var paginatedList = new PaginatedList<EmployeeEntity>(expectedEmployees, expectedEmployees.Count(), pageNumber, pageSize);

            _mockEmployeeRepository.Setup(x => x.GetEmployeesAsync(pageNumber, pageSize, searchTerm))
                .ReturnsAsync(paginatedList);

            var query = new GetAllEmployeesQuery(pageNumber, pageSize, searchTerm);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedEmployees.Count(), result.Items.Count());
            CollectionAssert.AreEqual(expectedEmployees, result.Items.ToList());
            _mockEmployeeRepository.Verify(x => x.GetEmployeesAsync(pageNumber, pageSize, searchTerm), Times.Once);
        }

        [TestMethod]
        public async Task Handle_Returns_EmptyList_When_Employees_NotFound()
        {
            // Arrange
            var pageNumber = 1;
            var pageSize = 10;
            var searchTerm = "nonexistent";

            var paginatedList = new PaginatedList<EmployeeEntity>(new List<EmployeeEntity>(), 0, pageNumber, pageSize);

            _mockEmployeeRepository.Setup(x => x.GetEmployeesAsync(pageNumber, pageSize, searchTerm))
                .ReturnsAsync(paginatedList);

            var query = new GetAllEmployeesQuery(pageNumber, pageSize, searchTerm);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Items.Count());
            _mockEmployeeRepository.Verify(x => x.GetEmployeesAsync(pageNumber, pageSize, searchTerm), Times.Once);
        }
    }
}