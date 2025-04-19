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

            var repository = mockRepo.Object;

            // Act
            var count = await repository.GetNumberOfEmployeesAsync();

            // Assert
            Assert.AreEqual(employees.Count, count);
        }

        [TestMethod]
        public async Task GetEmployeesAsync_WithSearchTerm_ReturnsFilteredResults()
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

            var repository = mockRepo.Object;

            // Act
            var result = await repository.GetEmployeesAsync(1, 10, searchTerm);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Items.Count);
            Assert.AreEqual("Employee 1", result.Items.First().Name);
        }

        [TestMethod]
        public async Task AddEmployeeAsync_AddsEmployeeSuccessfully()
        {
            // Arrange
            var newEmployee = new EmployeeEntity
            {
                Id = Guid.NewGuid(),
                Name = "New Employee",
                AppUserId = "user3",
                Email = "newemployee@example.com",
                Phone = "987-654-3210"
            };

            var mockRepo = new Mock<IEmployeeRepository>();
            mockRepo.Setup(repo => repo.AddEmployeeAsync(It.IsAny<EmployeeEntity>()))
                    .ReturnsAsync((EmployeeEntity e) =>
                    {
                        e.Id = Guid.NewGuid();
                        return e;
                    });

            var repository = mockRepo.Object;

            // Act
            var result = await repository.AddEmployeeAsync(newEmployee);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreNotEqual(Guid.Empty, result.Id);
        }
    }
}
