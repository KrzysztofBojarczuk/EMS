using EMS.APPLICATION.Features.Employee.Queries;
using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EMS.TESTS.FeaturesTests.EmployeeTests.QueriesTests
{
    [TestClass]
    public class GetUserEmployeesForListUpdateQueryHandlerTests
    {
        private Mock<IEmployeeRepository> _mockEmployeeRepository;
        private GetUserEmployeesForListUpdateQueryHandler _handler;

        public GetUserEmployeesForListUpdateQueryHandlerTests()
        {
            _mockEmployeeRepository = new Mock<IEmployeeRepository>();
            _handler = new GetUserEmployeesForListUpdateQueryHandler(_mockEmployeeRepository.Object);
        }

        [TestMethod]
        public async Task Handle_Returns_AllEmployees()
        {
            // Arrange
            var appUserId = "user-id-123";
            var employeeListId = Guid.NewGuid();

            var expectedEmployees = new List<EmployeeEntity>
            {
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 1", Email = "employee1@example.com", Phone = "123-456-789", Salary = 5000, DateOfBirth = new DateTime(1990, 1, 1), EmploymentDate = new DateTime(2022, 1, 1), MedicalCheckValidUntil = new DateTime(2025, 1, 1), EmployeeListId = employeeListId },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 2", Email = "employee2@example.com", Phone = "123-456-789", Salary = 5000, DateOfBirth = new DateTime(1991, 1, 1), EmploymentDate = new DateTime(2022, 2, 2), MedicalCheckValidUntil = new DateTime(2025, 2, 2), EmployeeListId = employeeListId },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 3", Email = "employee3@example.com", Phone = "123-456-789", Salary = 5000, DateOfBirth = new DateTime(1992, 1, 1), EmploymentDate = new DateTime(2022, 3, 3), MedicalCheckValidUntil = new DateTime(2025, 3, 3), EmployeeListId = employeeListId },
            };

            _mockEmployeeRepository.Setup(x => x.GetUserEmployeesForListUpdateAsync(appUserId, employeeListId, null))
                .ReturnsAsync(expectedEmployees);

            var query = new GetUserEmployeesForListUpdateQuery(appUserId, employeeListId, null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedEmployees.Count(), result.Count());
            CollectionAssert.AreEqual(expectedEmployees, result.ToList());
            _mockEmployeeRepository.Verify(x => x.GetUserEmployeesForListUpdateAsync(appUserId, employeeListId, null), Times.Once);
        }
    }
}