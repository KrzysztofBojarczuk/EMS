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
        private readonly Mock<IEmployeeRepository> _mockEmployeeRepository;
        private readonly GetAllEmployeesQueryHandler _handler;

        public GetAllEmployeesQueryHandlerTests()
        {
            _mockEmployeeRepository = new Mock<IEmployeeRepository>();
            _handler = new GetAllEmployeesQueryHandler(_mockEmployeeRepository.Object);
        }

        [TestMethod]
        public async Task Handle_Returns_BySearchTerm_Employees()
        {
            // Arrange
            int pageNumber = 1;
            int pageSize = 10;
            string searchTerm = "John";

            var employees = new List<EmployeeEntity>
            {
                new EmployeeEntity { Name = "John", Email = "john@example.com", Phone = "123" },
                new EmployeeEntity { Name = "Johnny", Email = "johnny@example.com", Phone = "456" }
            };

            var paginatedList = new PaginatedList<EmployeeEntity>(employees, employees.Count(), pageNumber, pageSize);

            _mockEmployeeRepository
                .Setup(repo => repo.GetEmployeesAsync(pageNumber, pageSize, searchTerm))
                .ReturnsAsync(paginatedList);

            var query = new GetAllEmployeesQuery(pageNumber, pageSize, searchTerm);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(employees.Count(), result.Items.Count);
            Assert.AreEqual(employees[0].Name, result.Items[0].Name);
            Assert.AreEqual(employees[1].Name, result.Items[1].Name);
            _mockEmployeeRepository.Verify(repo => repo.GetEmployeesAsync(pageNumber, pageSize, searchTerm), Times.Once);
        }
    }
}