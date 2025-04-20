using EMS.CORE.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using EMS.CORE.Interfaces;
using EMS.INFRASTRUCTURE.Extensions;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace EMS.TESTS.Repository
{
    [TestClass]
    public class EmployeeRepositoryTests
    {
        [TestMethod]
        public async Task GetNumberOfEmployeesAsync_ReturnsTotalCount()
        {
            // Arrange
            var employees = new List<EmployeeEntity>
            {
                new EmployeeEntity { Name = "Employee 1", AppUserId = "user1", Email = "employee1@example.com", Phone = "123-456-7891" },
                new EmployeeEntity { Name = "Employee 2", AppUserId = "user1", Email = "employee1@example.com", Phone = "123-456-7891" }
            };

            var mockRepo = new Mock<IEmployeeRepository>();
            mockRepo.Setup(repo => repo.GetNumberOfEmployeesAsync())
                    .ReturnsAsync(employees.Count);

            // Act
            var count = await mockRepo.Object.GetNumberOfEmployeesAsync();

            // Assert
            Assert.AreEqual(employees.Count, count);
        }

        [TestMethod]
        public async Task GetEmployeesAsync_BySearchTerm_Returns()
        {
            // Arrange
            var searchTerm = "Employee 1";
            var employees = new List<EmployeeEntity>
            {
                   new EmployeeEntity { Name = "Employee 1", AppUserId = "user1", Email = "employee1@example.com", Phone = "123-456-7891" }
            };

            var paginatedList = new PaginatedList<EmployeeEntity>(employees, employees.Count, 1, 10);

            var mockRepo = new Mock<IEmployeeRepository>();
            mockRepo.Setup(repo => repo.GetEmployeesAsync(1, 10, searchTerm))
                    .ReturnsAsync(paginatedList);

            // Act
            var result = await mockRepo.Object.GetEmployeesAsync(1, 10, searchTerm);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Items.Count);
            Assert.AreEqual("Employee 1", result.Items.First().Name);
        }
    }
}
