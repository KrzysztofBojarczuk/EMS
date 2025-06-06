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
        private AppDbContext _context;
        private IEmployeeRepository _repository;
        private Mock<IEmployeeRepository> _mockEmployeeRepository;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) 
                .Options;

            _context = new AppDbContext(options);
            _repository = new EmployeeRepository(_context);

            _mockEmployeeRepository = new Mock<IEmployeeRepository>();
        }

        [TestMethod]
        public async Task GetUserEmployeesAsync_BySearchTerm_Returns_Employees()
        {
            // Arrange
            var userId = "user1";
            var searchTerm = "Tomasz";

            var employees = new List<EmployeeEntity>
            {
                new EmployeeEntity { Name = "Grzegorz", AppUserId = "user1" },
                new EmployeeEntity { Name = "Janusz", AppUserId = "user1" },
                new EmployeeEntity { Name = "Tomasz", AppUserId = "user1" }
            };

            var paginatedList = new PaginatedList<EmployeeEntity>(
                employees.Where(x => x.Name.ToLower().Contains(searchTerm.ToLower())).ToList(),
                1, 1, 10);

            _mockEmployeeRepository.Setup(x => x.GetUserEmployeesAsync(userId, 1, 10, searchTerm))
                .ReturnsAsync(paginatedList);

            // Act
            var result = await _mockEmployeeRepository.Object.GetUserEmployeesAsync(userId, 1, 10, searchTerm);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Items.Count);
            Assert.AreEqual(employees[2].Name, result.Items.First().Name);
        }

        [TestMethod]
        public async Task GetUserNumberOfEmployeesAsync_Returns_TotalCount()
        {
            // Arrange
            var userId = "user1";
            var employees = new List<EmployeeEntity>
            {
                new EmployeeEntity { Name = "Grzegorz", AppUserId = "user1" },
                new EmployeeEntity { Name = "Janusz", AppUserId = "user1" },
                new EmployeeEntity { Name = "Tomasz", AppUserId = "user1" }
            };

            _mockEmployeeRepository.Setup(r => r.GetUserNumberOfEmployeesAsync(userId))
                .ReturnsAsync(employees.Count);

            // Act
            var count = await _mockEmployeeRepository.Object.GetUserNumberOfEmployeesAsync(userId);

            // Assert
            Assert.AreEqual(employees.Count(), count);
        }

        [TestMethod]
        public async Task GetNumberOfEmployeesAsync_Returns_TotalCount()
        {
            // Arrange
            var employees = new List<EmployeeEntity>
            {
                new EmployeeEntity { Name = "Grzegorz" },
                new EmployeeEntity { Name = "Janusz" },
                new EmployeeEntity { Name = "Tomasz" }
            };

            _mockEmployeeRepository.Setup(r => r.GetNumberOfEmployeesAsync())
                .ReturnsAsync(employees.Count);

            // Act
            var count = await _mockEmployeeRepository.Object.GetNumberOfEmployeesAsync();

            // Assert
            Assert.AreEqual(employees.Count(), count);
        }

        [TestMethod]
        public async Task GetEmployeesAsync_BySearchTerm_Returns_Employees()
        {
            // Arrange
            var searchTerm = "Tomasz";
            var employees = new List<EmployeeEntity>
            {
                new EmployeeEntity { Name = "Grzegorz" },
                new EmployeeEntity { Name = "Janusz" },
                new EmployeeEntity { Name = "Tomasz" }
            };

            var paginatedList = new PaginatedList<EmployeeEntity>(
                employees.Where(x => x.Name.ToLower().Contains(searchTerm.ToLower())).ToList(),
                1, 1, 10);

            _mockEmployeeRepository.Setup(x => x.GetEmployeesAsync(1, 10, searchTerm))
                .ReturnsAsync(paginatedList);

            // Act
            var result = await _mockEmployeeRepository.Object.GetEmployeesAsync(1, 10, searchTerm);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Items.Count);
            Assert.AreEqual(employees[2].Name, result.Items.First().Name);
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

            // Act    await _context.Employees.AddAsync(employee);
            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetEmployeeByIdAsync(employee.Id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(employee.Id, result.Id);
            Assert.AreEqual(employee.Name, result.Name);
        }

        [TestMethod]
        public async Task UpdateEmployeeAsync_WhenEntityIsNull_ReturnsNull()
        {
            // Arrange
            var employeeId = Guid.NewGuid();

            // Act
            var result = await _repository.UpdateEmployeeAsync(employeeId, null);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task UpdateEmployeeAsync_WhenEntityIsNotNullAndExists_UpdatesAndReturnsEmployee()
        {
            // Arrange
            var originalEmployee = new EmployeeEntity
            {
                Name = "Tomasz Wójcik",
                Email = "tomasz@example.com",
                Phone = "111222333",
                Salary = 4000,
                AppUserId = "user1"
            };

            await _context.Employees.AddAsync(originalEmployee);
            await _context.SaveChangesAsync();

            var updatedEmployee = new EmployeeEntity
            {
                Name = "Tomasz Nowy",
                Email = "nowy@example.com",
                Phone = "444555666",
                Salary = 6500
            };

            // Act
            var result = await _repository.UpdateEmployeeAsync(originalEmployee.Id, updatedEmployee);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(originalEmployee.Id, result.Id);
            Assert.AreEqual(updatedEmployee.Name, result.Name);
            Assert.AreEqual(updatedEmployee.Email, result.Email);
            Assert.AreEqual(updatedEmployee.Phone, result.Phone);
            Assert.AreEqual(6500, result.Salary);
        }

        [TestMethod]
        public async Task DeleteEmployeeAsync_WhenEmployeeExists_ReturnsTrue()
        {
            // Arrange
            var employee = new EmployeeEntity
            {
                Name = "Anna Kowalska",
                Email = "anna.k@example.com",
                Phone = "987654321",
                AppUserId = "user999"
            };

            await _context.Employees.AddAsync(employee);
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
        public async Task DeleteEmployeeAsync_WhenEmployeeDoesNotExist_ReturnsFalse()
        {
            // Act
            var result = await _repository.DeleteEmployeeAsync(Guid.NewGuid());

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task AddEmployeeAsync_AddsEmployee_Returns_Employee()
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
    }
}