using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using EMS.INFRASTRUCTURE.Data;
using EMS.INFRASTRUCTURE.Extensions;
using EMS.INFRASTRUCTURE.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace EMS.TESTS.Repository
{
    [TestClass]
    public class EmployeeRepositoryTests
    {

        [TestMethod]
        public async Task GetUserEmployeesAsync_BySearchTerm_Returns_Employees()
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
        public async Task GetUserNumberOfEmployeesAsync_Returns_TotalCount()
        {
            // Arrange
            var userId = "user1";
            var employees = new List<EmployeeEntity>
            {
                  new EmployeeEntity { Name = "Grzegorz", AppUserId = "user1", Email = "employee1@example.com", Phone = "123-456-7891" },
                  new EmployeeEntity { Name = "Janusz", AppUserId = "user1", Email = "employee1@example.com", Phone = "123-456-7891" },
                  new EmployeeEntity { Name = "Tomasz", AppUserId = "user1", Email = "employee1@example.com", Phone = "123-456-7891" }
            };

            var mockRepo = new Mock<IEmployeeRepository>();
            mockRepo.Setup(repo => repo.GetUserNumberOfEmployeesAsync(userId))
                    .ReturnsAsync(employees.Count);

            // Act
            var count = await mockRepo.Object.GetUserNumberOfEmployeesAsync(userId);

            // Assert
            Assert.AreEqual(employees.Count, count);
        }

        [TestMethod]
        public async Task GetNumberOfEmployeesAsync_Returns_TotalCount()
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
        public async Task GetEmployeesAsync_BySearchTerm_Returns_Employees()
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

        [TestMethod]
        public async Task AddEmployeeAsync_AddsEmployee_ReturnsEntity()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "AddEmployeeTestDb")
                .Options;

            var employee = new EmployeeEntity
            {
                Name = "Anna Nowak",
                Email = "anna@example.com",
                Phone = "123456789",
                AppUserId = "user123"
            };

            using (var context = new AppDbContext(options))
            {
                var repository = new EmployeeRepository(context);

                // Act
                var result = await repository.AddEmployeeAsync(employee);

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual("Anna Nowak", result.Name);
                Assert.AreNotEqual(Guid.Empty, result.Id); 
                Assert.AreEqual(1, context.Employees.Count());
            }
        }
    }
}