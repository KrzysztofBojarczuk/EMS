using EMS.CORE.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using EMS.CORE.Interfaces;
using EMS.INFRASTRUCTURE.Extensions;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using EMS.INFRASTRUCTURE.Data;
using EMS.INFRASTRUCTURE.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EMS.TESTS.Repository
{
    [TestClass]
    public class EmployeeRepositoryTests
    {

        [TestMethod]
        public async Task GetUserEmployeesAsync_BySearchTerm_Returns()
        {
            // Arrange
            var userId = "user1";
            var searchTerm = "Tomasz";

            var employees = new List<EmployeeEntity>
            {
                   new EmployeeEntity { Name = "Grzegorz", AppUserId = "user1", Email = "employee1@example.com", Phone = "123-456-7891" },
                   new EmployeeEntity { Name = "Janusz", AppUserId = "user1", Email = "employee1@example.com", Phone = "123-456-7891" },
                   new EmployeeEntity { Name = "Tomasz", AppUserId = "user1", Email = "employee1@example.com", Phone = "123-456-7891" }
            };

            var paginatedList = new PaginatedList<EmployeeEntity>(
                 employees.Where(e => e.AppUserId == userId && e.Name.ToLower().Contains(searchTerm.ToLower())).ToList(),
                 1, 1, 10);

            var mockRepo = new Mock<IEmployeeRepository>();
            mockRepo.Setup(repo => repo.GetUserEmployeesAsync(userId, 1, 10, searchTerm))
                    .ReturnsAsync(paginatedList);

            // Act
            var result = await mockRepo.Object.GetUserEmployeesAsync(userId, 1, 10, searchTerm);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Items.Count);
            Assert.AreEqual(searchTerm, result.Items.First().Name);
        }

        [TestMethod]
        public async Task GetNumberOfEmployeesAsync_ReturnsTotalCount()
        {
            // Arrange
            var employees = new List<EmployeeEntity>
            {
                  new EmployeeEntity { Name = "Grzegorz", AppUserId = "user1", Email = "employee1@example.com", Phone = "123-456-7891" },
                  new EmployeeEntity { Name = "Janusz", AppUserId = "user1", Email = "employee1@example.com", Phone = "123-456-7891" },
                  new EmployeeEntity { Name = "Tomasz", AppUserId = "user1", Email = "employee1@example.com", Phone = "123-456-7891" }
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
            var searchTerm = "Tomasz";
            var employees = new List<EmployeeEntity>
            {
                   new EmployeeEntity { Name = "Grzegorz", AppUserId = "user1", Email = "employee1@example.com", Phone = "123-456-7891" },
                   new EmployeeEntity { Name = "Janusz", AppUserId = "user1", Email = "employee1@example.com", Phone = "123-456-7891" },
                   new EmployeeEntity { Name = "Tomasz", AppUserId = "user1", Email = "employee1@example.com", Phone = "123-456-7891" }
            };
            var paginatedList = new PaginatedList<EmployeeEntity>(
                            employees.Where(x => x.Name.ToLower().Contains(searchTerm.ToLower())).ToList(),
                            1, 1, 10);

            var mockRepo = new Mock<IEmployeeRepository>();
            mockRepo.Setup(repo => repo.GetEmployeesAsync(1, 10, searchTerm))
                    .ReturnsAsync(paginatedList);

            // Act
            var result = await mockRepo.Object.GetEmployeesAsync(1, 10, searchTerm);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Items.Count);
            Assert.AreEqual(searchTerm, result.Items.First().Name);
        }
    }
}
