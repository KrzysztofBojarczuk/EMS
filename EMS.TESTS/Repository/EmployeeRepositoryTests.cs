using EMS.CORE.Entities;
using EMS.INFRASTRUCTURE.Data;
using EMS.INFRASTRUCTURE.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.InMemory;

namespace EMS.TESTS.Repository
{
    public class EmployeeRepositoryTests
    {
        private async Task<AppDbContext> GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var databaseContext = new AppDbContext(options);
            databaseContext.Database.EnsureCreated();

            if (await databaseContext.Employees.CountAsync() <= 0)
            {
                for (int i = 1; i <= 5; i++)
                {
                    databaseContext.Employees.Add(new EmployeeEntity
                    {
                        Id = Guid.NewGuid(),
                        Name = $"Employee {i}",
                        AppUserId = i % 2 == 0 ? "user1" : "user2",
                        Email = $"employee{i}@example.com",  
                        Phone = $"123-456-789{i}"            
                    });
                }
                await databaseContext.SaveChangesAsync();
            }
            return databaseContext;
        }

        [Fact]
        public async Task EmployeeRepository_GetUserNumberOfEmployees_ReturnsCorrectCount()
        {
            // Arrange
            var userId = "user1";
            var dbContext = await GetDatabaseContext();
            var repository = new EmployeeRepository(dbContext);

            // Act
            var result = await repository.GetUserNumberOfEmployeesAsync(userId);

            // Assert
            result.Should().Be(2); 
        }

        [Fact]
        public async Task EmployeeRepository_GetNumberOfEmployees_ReturnsTotalCount()
        {
            // Arrange
            var dbContext = await GetDatabaseContext();
            var repository = new EmployeeRepository(dbContext);

            // Act
            var result = await repository.GetNumberOfEmployeesAsync();

            // Assert
            result.Should().Be(5); 
        }

        [Fact]
        public async Task EmployeeRepository_GetEmployees_WithSearchTerm_ReturnsFilteredResults()
        {
            // Arrange
            var searchTerm = "Employee 1";
            var dbContext = await GetDatabaseContext();
            var repository = new EmployeeRepository(dbContext);

            // Act
            var result = await repository.GetEmployeesAsync(1,10,searchTerm);

            // Assert
            // Assert
            result.Should().NotBeNull();
            result.Items.Count.Should().Be(1);
            result.Items.First().Name.Should().Be("Employee 1");
        }

        [Fact]
        public async Task EmployeeRepository_AddEmployee_AddsEmployeeSuccessfully()
        {
            // Arrange
            var newEmployee = new EmployeeEntity
            {
                Name = "New Employee",
                AppUserId = "user3",
                Email = "newemployee@example.com",  
                Phone = "987-654-3210"              
            };
            var dbContext = await GetDatabaseContext();
            var repository = new EmployeeRepository(dbContext);

            // Act
            var addedEmployee = await repository.AddEmployeeAsync(newEmployee);

            // Assert
            addedEmployee.Should().NotBeNull();
            addedEmployee.Id.Should().NotBe(Guid.Empty);
            dbContext.Employees.Should().Contain(e => e.Id == addedEmployee.Id);
        }
    }
}
