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
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 1", Email = "employee1@example.com", Phone = "123-456-789", Salary = 5000, DateOfBirth = new DateTime(1990, 1, 1), EmploymentDate = new DateTime(2022, 1, 1), MedicalCheckValidUntil = new DateTime(2025, 1, 1) },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 2", Email = "employee2@example.com", Phone = "123-456-789", Salary = 5000, DateOfBirth = new DateTime(1991, 1, 1), EmploymentDate = new DateTime(2022, 2, 2), MedicalCheckValidUntil = new DateTime(2025, 2, 2) },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 3", Email = "employee3@example.com", Phone = "123-456-789", Salary = 5000, DateOfBirth = new DateTime(1992, 1, 1), EmploymentDate = new DateTime(2022, 3, 3), MedicalCheckValidUntil = new DateTime(2025, 3, 3) },
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
            var searchTerm = "test";

            var expectedEmployees = new List<EmployeeEntity>
            {
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 1 Test", Email = "employee1@example.com", Phone = "123-456-789", Salary = 5000, DateOfBirth = new DateTime(1990, 1, 1), EmploymentDate = new DateTime(2022, 1, 1), MedicalCheckValidUntil = new DateTime(2025, 1, 1) },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 2 Test", Email = "employee2@example.com", Phone = "123-456-789", Salary = 5000, DateOfBirth = new DateTime(1991, 1, 1), EmploymentDate = new DateTime(2022, 2, 2), MedicalCheckValidUntil = new DateTime(2025, 2, 2) },
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
            var searchTerm = "nonexistent";

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