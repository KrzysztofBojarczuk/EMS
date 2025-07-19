using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using EMS.INFRASTRUCTURE.Data;
using EMS.INFRASTRUCTURE.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EMS.TESTS.RepositoriesTests
{
    [TestClass]
    public class EmployeeRepositoryTests
    {
        private AppDbContext _context;
        private IEmployeeRepository _repository;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) 
                .Options;

            _context = new AppDbContext(options);
            _repository = new EmployeeRepository(_context);
        }

        [TestMethod]
        public async Task GetUserEmployeesAsync_BySearchTerm_Returns_Employees()
        {
            // Arrange
            var userId = "user-id-123";
            var searchTerm = "Tomasz";

            var employees = new List<EmployeeEntity>
            {
                new EmployeeEntity { Name = "Grzegorz", AppUserId = userId, Email = "grzegorz@example.com", Phone = "111111111" },
                new EmployeeEntity { Name = "Janusz", AppUserId = userId, Email = "janusz@example.com", Phone = "222222222" },
                new EmployeeEntity { Name = "Tomasz", AppUserId = userId, Email = "tomasz@example.com", Phone = "333333333" }
            };

            _context.Employees.AddRange(employees);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetUserEmployeesAsync(userId, 1, 10, searchTerm);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Items.Count());
            Assert.AreEqual(employees[2].Name, result.Items.First().Name);
        }

        [TestMethod]
        public async Task GetUserEmployeesAsync_When_EmployeeesDoesNotExist_Returns_EmptyList()
        {
            // Arrange
            var userId = "user-id-123";
            var searchTerm = "nonexistent";

            var employees = new List<EmployeeEntity>
            {
                new EmployeeEntity { Name = "Grzegorz", AppUserId = userId, Email = "grzegorz@example.com", Phone = "111111111" },
                new EmployeeEntity { Name = "Janusz", AppUserId = userId, Email = "janusz@example.com", Phone = "222222222" },
                new EmployeeEntity { Name = "Tomasz", AppUserId = userId, Email = "tomasz@example.com", Phone = "333333333" }
            };

            _context.Employees.AddRange(employees);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetUserEmployeesAsync(userId, 1, 10, searchTerm);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Items.Count());
        }

        [TestMethod]
        public async Task GetUserNumberOfEmployeesAsync_Returns_TotalCount()
        {
            // Arrange
            var userId1 = "user-id-123";
            var userId2 = "user-id-1234";

            var employees = new List<EmployeeEntity>
            {
                new EmployeeEntity { Name = "Grzegorz", AppUserId = userId1, Email = "grzegorz@example.com", Phone = "111111111" },
                new EmployeeEntity { Name = "Janusz", AppUserId = userId1, Email = "janusz@example.com", Phone = "222222222" },
                new EmployeeEntity { Name = "Tomasz", AppUserId = userId2, Email = "tomasz@example.com", Phone = "333333333" }
            };

            _context.Employees.AddRange(employees);
            await _context.SaveChangesAsync();

            // Act
            var count = await _repository.GetUserNumberOfEmployeesAsync(userId1);

            // Assert
            Assert.AreEqual(employees.Where(x => x.AppUserId == userId1).Count(), count);
        }

        [TestMethod]
        public async Task GetNumberOfEmployeesAsync_Returns_TotalCount()
        {
            // Arrange
            var userId = "user-id-123";

            var employees = new List<EmployeeEntity>
            {
                new EmployeeEntity { Name = "Grzegorz", AppUserId = userId, Email = "grzegorz@example.com", Phone = "111111111" },
                new EmployeeEntity { Name = "Janusz", AppUserId = userId, Email = "janusz@example.com", Phone = "222222222" },
                new EmployeeEntity { Name = "Tomasz", AppUserId = userId, Email = "tomasz@example.com", Phone = "333333333" }
            };

            _context.Employees.AddRange(employees);
            await _context.SaveChangesAsync();

            // Act
            var count = await _repository.GetNumberOfEmployeesAsync();

            // Assert
            Assert.AreEqual(employees.Count(), count);
        }

        [TestMethod]
        public async Task GetEmployeesAsync_BySearchTerm_Returns_Employees()
        {
            // Arrange
            var searchTerm = "Tomasz";
            var userId = "user-id-123";

            var employees = new List<EmployeeEntity>
            {
                new EmployeeEntity { Name = "Grzegorz", AppUserId = userId, Email = "grzegorz@example.com", Phone = "111111111" },
                new EmployeeEntity { Name = "Janusz", AppUserId = userId, Email = "janusz@example.com", Phone = "222222222" },
                new EmployeeEntity { Name = "Tomasz", AppUserId = userId, Email = "tomasz@example.com", Phone = "333333333" }
            };

            _context.Employees.AddRange(employees);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetEmployeesAsync(1, 10, searchTerm);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Items.Count());
            Assert.AreEqual(employees[2].Name, result.Items.First().Name);
        }

        [TestMethod]
        public async Task GetEmployeesAsync_When_EmployeeesDoesNotExist_Returns_EmptyList()
        {
            // Arrange
            var searchTerm = "nonexistent";
            var userId = "user-id-123";

            var employees = new List<EmployeeEntity>
            {
                new EmployeeEntity { Name = "Grzegorz", AppUserId = userId, Email = "grzegorz@example.com", Phone = "111111111" },
                new EmployeeEntity { Name = "Janusz", AppUserId = userId, Email = "janusz@example.com", Phone = "222222222" },
                new EmployeeEntity { Name = "Tomasz", AppUserId = userId, Email = "tomasz@example.com", Phone = "333333333" }
            };

            _context.Employees.AddRange(employees);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetEmployeesAsync(1, 10, searchTerm);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Items.Count());
        }

        [TestMethod]
        public async Task GetEmployeeByIdAsync_Returns_Employee()
        {
            // Arrange
            var employee = new EmployeeEntity
            {
                Name = "Anna Nowak",
                Email = "anna@example.com",
                Phone = "123456789",
                AppUserId = "user123"
            };

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetEmployeeByIdAsync(employee.Id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(employee.Id, result.Id);
            Assert.AreEqual(employee.Name, result.Name);
        }

        [TestMethod]
        public async Task AddEmployeeAsync_Returns_Employee()
        {
            // Arrange
            var employee = new EmployeeEntity
            {
                Name = "Anna Nowak",
                Email = "anna@example.com",
                Phone = "123456789",
                AppUserId = "user123"
            };

            // Act
            var result = await _repository.AddEmployeeAsync(employee);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(employee.Name, result.Name);
            Assert.AreNotEqual(Guid.Empty, result.Id);
            Assert.AreEqual(1, _context.Employees.Count());
        }

        [TestMethod]
        public async Task UpdateEmployeeAsync_When_EntityIsNotNullAndExists_UpdatesAnd_Returns_Employee()
        {
            // Arrange
            var userId = "user-id-123";

            var employee = new EmployeeEntity
            {
                Name = "Tomasz Wójcik",
                Email = "tomasz@example.com",
                Phone = "111222333",
                Salary = 4000,
                AppUserId = userId
            };

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            var updatedEmployee = new EmployeeEntity
            {
                Name = "Tomasz Nowy",
                Email = "nowy@example.com",
                Phone = "444555666",
                Salary = 6500,
                AppUserId = userId
            };

            // Act
            var result = await _repository.UpdateEmployeeAsync(employee.Id, updatedEmployee);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(employee.Id, result.Id);
            Assert.AreEqual(updatedEmployee.Name, result.Name);
            Assert.AreEqual(updatedEmployee.Email, result.Email);
            Assert.AreEqual(updatedEmployee.Phone, result.Phone);
            Assert.AreEqual(updatedEmployee.Salary, result.Salary);
        }

        [TestMethod]
        public async Task UpdateEmployeeAsync_When_EntityIsNull_Returns_Null()
        {
            // Arrange
            var employeeId = Guid.NewGuid();

            // Act
            var result = await _repository.UpdateEmployeeAsync(employeeId, null);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task DeleteEmployeeAsync_When_EmployeeExists_Returns_True()
        {
            // Arrange
            var employee = new EmployeeEntity
            {
                Name = "Anna Kowalska",
                Email = "anna.k@example.com",
                Phone = "987654321",
                AppUserId = "user999"
            };

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            var emplyeeCountBefore = _context.Employees.Count();

            // Act
            var result = await _repository.DeleteEmployeeAsync(employee.Id);

            var emplyeeCountAfter = _context.Employees.Count();

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(emplyeeCountBefore - 1, emplyeeCountAfter);
            Assert.AreEqual(0, _context.Employees.Count());
        }

        [TestMethod]
        public async Task DeleteEmployeeAsync_When_EmployeeDoesNotExist_Returns_False()
        {
            // Act
            var result = await _repository.DeleteEmployeeAsync(Guid.NewGuid());

            // Assert
            Assert.IsFalse(result);
        }
    }
}