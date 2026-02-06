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
        public async Task AddEmployeeAsync_Returns_Employee()
        {
            // Arrange
            var employee = new EmployeeEntity
            {
                Name = "Employee",
                Email = "employee@example.com",
                Phone = "123-456-789",
                Salary = 5000,
                DateOfBirth = new DateTime(1990, 1, 1),
                EmploymentDate = new DateTime(2022, 1, 1),
                MedicalCheckValidUntil = new DateTime(2025, 1, 1),
                AppUserId = "user-id-123"
            };

            // Act
            var result = await _repository.AddEmployeeAsync(employee);

            var employeeCount = await _context.Employees.CountAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(employee.Name, result.Name);
            Assert.AreEqual(employee.Email, result.Email);
            Assert.AreEqual(employee.Phone, result.Phone);
            Assert.AreEqual(employee.AppUserId, result.AppUserId);
            Assert.AreNotEqual(Guid.Empty, result.Id);
            Assert.AreEqual(1, employeeCount);
        }

        [TestMethod]
        public async Task AddEmployeeListAsync_Returns_EmployeeList()
        {
            // Arrange
            var appUserId = "user-id-123";

            var employee1 = new EmployeeEntity
            {
                Id = Guid.NewGuid(),
                Name = "Employee",
                Email = "employee@example.com",
                Phone = "123-456-789",
                Salary = 3000,
                DateOfBirth = new DateTime(1990, 1, 1),
                EmploymentDate = new DateTime(2022, 1, 1),
                MedicalCheckValidUntil = new DateTime(2025, 2, 2),
                AppUserId = appUserId
            };
            var employee2 = new EmployeeEntity
            {
                Id = Guid.NewGuid(),
                Name = "Employee",
                Email = "employee@example.com",
                Phone = "123-456-789",
                Salary = 7000,
                DateOfBirth = new DateTime(1990, 1, 1),
                EmploymentDate = new DateTime(2022, 1, 1),
                MedicalCheckValidUntil = new DateTime(2025, 2, 2),
                AppUserId = appUserId
            };

            _context.Employees.AddRange(employee1, employee2);
            await _context.SaveChangesAsync();

            var employeeList = new EmployeeListsEntity
            {
                Name = "EmployeeList",
                AppUserId = appUserId
            };

            // Act
            var result = await _repository.AddEmployeeListsAsync(employeeList, new List<Guid> { employee1.Id, employee2.Id });

            var employeeListCount = await _context.EmployeeLists.CountAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(employeeList.Name, result.Name);
            Assert.AreNotEqual(Guid.Empty, result.Id);
            Assert.AreEqual(1, employeeListCount);
        }

        [TestMethod]
        public async Task GetEmployeeByIdAsync_Returns_Employee()
        {
            // Arrange
            var employeeId = Guid.NewGuid();

            var employee = new EmployeeEntity
            {
                Id = employeeId,
                Name = "Employee",
                Email = "employee@example.com",
                Phone = "123-456-789",
                Salary = 5000,
                DateOfBirth = new DateTime(1990, 1, 1),
                EmploymentDate = new DateTime(2022, 1, 1),
                MedicalCheckValidUntil = new DateTime(2025, 1, 1),
                AppUserId = "user-id-123"
            };

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetEmployeeByIdAsync(employeeId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(employeeId, result.Id);
            Assert.AreEqual(employee.Name, result.Name);
            Assert.AreEqual(employee.Email, result.Email);
            Assert.AreEqual(employee.Phone, result.Phone);
            Assert.AreEqual(employee.Salary, result.Salary);
            Assert.AreEqual(employee.AppUserId, result.AppUserId);
        }

        [TestMethod]
        public async Task GetEmployeeByIdAsync_When_EmployeeDoesNotExist_Returns_Null()
        {
            // Act
            var result = await _repository.GetEmployeeByIdAsync(Guid.NewGuid());

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetUserEmployeesAsync_Returns_AllEmployees()
        {
            // Arrange
            var appUserId1 = "user-id-123";
            var appUserId2 = "user-id-1234";

            var employees = new List<EmployeeEntity>
            {
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 1", Email = "employee1@example.com", Phone = "123-456-789", Salary = 5000, DateOfBirth = new DateTime(1990, 1, 1), EmploymentDate = new DateTime(2021, 1, 1), MedicalCheckValidUntil = new DateTime(2021, 1, 1), AppUserId = appUserId1 },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 2", Email = "employee2@example.com", Phone = "123-456-789", Salary = 5000, DateOfBirth = new DateTime(1991, 1, 1), EmploymentDate = new DateTime(2022, 1, 1), MedicalCheckValidUntil = new DateTime(2022, 1, 1), AppUserId = appUserId1 },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 3", Email = "employee3@example.com", Phone = "123-456-789", Salary = 5000, DateOfBirth = new DateTime(1992, 1, 1), EmploymentDate = new DateTime(2023, 1, 1), MedicalCheckValidUntil = new DateTime(2023, 1, 1), AppUserId = appUserId1 },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 4", Email = "employee4@example.com", Phone = "123-456-789", Salary = 5000, DateOfBirth = new DateTime(1993, 1, 1), EmploymentDate = new DateTime(2024, 1, 1), MedicalCheckValidUntil = new DateTime(2024, 1, 1), AppUserId = appUserId1 },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 5", Email = "employee5@example.com", Phone = "123-456-789", Salary = 5000, DateOfBirth = new DateTime(1994, 1, 1), EmploymentDate = new DateTime(2025, 1, 1), MedicalCheckValidUntil = new DateTime(2025, 1, 1), AppUserId = appUserId1 },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 6", Email = "employee5@example.com", Phone = "123-456-789", Salary = 5000, DateOfBirth = new DateTime(1994, 1, 1), EmploymentDate = new DateTime(2025, 1, 1), MedicalCheckValidUntil = new DateTime(2025, 1, 1), AppUserId = appUserId2 },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 7", Email = "employee5@example.com", Phone = "123-456-789", Salary = 5000, DateOfBirth = new DateTime(1994, 1, 1), EmploymentDate = new DateTime(2025, 1, 1), MedicalCheckValidUntil = new DateTime(2025, 1, 1), AppUserId = appUserId2 }
            };

            _context.Employees.AddRange(employees);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetUserEmployeesAsync(appUserId1, 1, 10, null, null);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(5, result.Items.Count());
        }

        [TestMethod]
        public async Task GetUserEmployeesAsync_BySearchTerm_Returns_Employees()
        {
            // Arrange
            var appUserId = "user-id-123";
            var searchTerm = "test";

            var employees = new List<EmployeeEntity>
            {
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 1 Test", Email = "employee1@example.com", Phone = "123-456-789", Salary = 5000,  DateOfBirth = new DateTime(1990, 1, 1), EmploymentDate = new DateTime(2021, 1, 1), MedicalCheckValidUntil = new DateTime(2022, 1, 1), AppUserId = appUserId, },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 2", Email = "employee2@example.com", Phone = "123-456-789", Salary = 5000, DateOfBirth = new DateTime(1991, 1, 1), EmploymentDate = new DateTime(2022, 1, 1), MedicalCheckValidUntil = new DateTime(2022, 1, 1), AppUserId = appUserId },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 3", Email = "employee3@example.com", Phone = "123-456-789", Salary = 5000, DateOfBirth = new DateTime(1992, 1, 1), EmploymentDate = new DateTime(2023, 1, 1), MedicalCheckValidUntil = new DateTime(2023, 1, 1), AppUserId = appUserId }
            };

            _context.Employees.AddRange(employees);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetUserEmployeesAsync(appUserId, 1, 10, searchTerm, null);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Items.Count());
            Assert.AreEqual(employees[0].Name, result.Items.First().Name);
        }

        [TestMethod]
        public async Task GetUserEmployeesAsync_When_EmployeesDoesNotExist_Returns_EmptyList()
        {
            // Arrange
            var appUserId = "user-id-123";
            var searchTerm = "nonexistent";

            var employees = new List<EmployeeEntity>
            {
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 1", Email = "employee1@example.com", Phone = "123-456-789", Salary = 5000, DateOfBirth = new DateTime(1990, 1, 1), EmploymentDate = new DateTime(2021, 1, 1), MedicalCheckValidUntil = new DateTime(2021, 1, 1), AppUserId = appUserId, },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 2", Email = "employee2@example.com", Phone = "123-456-789", Salary = 5000, DateOfBirth = new DateTime(1991, 1, 1), EmploymentDate = new DateTime(2022, 1, 1), MedicalCheckValidUntil = new DateTime(2022, 1, 1), AppUserId = appUserId },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 3", Email = "employee3@example.com", Phone = "123-456-789", Salary = 5000, DateOfBirth = new DateTime(1992, 1, 1), EmploymentDate = new DateTime(2023, 1, 1), MedicalCheckValidUntil = new DateTime(2023, 1, 1), AppUserId = appUserId }
            };

            _context.Employees.AddRange(employees);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetUserEmployeesAsync(appUserId, 1, 10, searchTerm, null);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Items.Count());
        }

        [TestMethod]
        public async Task GetUserEmployeesAsync_SortedBySalaryAscending_Returns_SortedEmployees()
        {
            // Arrange
            var appUserId = "user-id-123";
            var sortOrder = "salary_asc";

            var employees = new List<EmployeeEntity>
            {
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 1", Email = "employee1@example.com", Phone = "123-456-789", Salary = 3000, DateOfBirth = new DateTime(1990, 1, 1), EmploymentDate = new DateTime(2021, 1, 1), MedicalCheckValidUntil = new DateTime(2021, 1, 1), AppUserId = appUserId },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 2", Email = "employee2@example.com", Phone = "123-456-789", Salary = 2000, DateOfBirth = new DateTime(1991, 1, 1), EmploymentDate = new DateTime(2022, 1, 1), MedicalCheckValidUntil = new DateTime(2022, 1, 1), AppUserId = appUserId },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 3", Email = "employee3@example.com", Phone = "123-456-789", Salary = 1000, DateOfBirth = new DateTime(1992, 1, 1), EmploymentDate = new DateTime(2023, 1, 1), MedicalCheckValidUntil = new DateTime(2023, 1, 1), AppUserId = appUserId }
            };

            _context.Employees.AddRange(employees);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetUserEmployeesAsync(appUserId, 1, 10, null, sortOrder);
            var sorted = result.Items.ToList();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, sorted.Count());
            Assert.AreEqual(employees[0].Id, sorted[2].Id);
            Assert.AreEqual(employees[1].Id, sorted[1].Id);
            Assert.AreEqual(employees[2].Id, sorted[0].Id);
        }

        [TestMethod]
        public async Task GetUserEmployeesAsync_SortedBySalaryDescending_Returns_SortedEmployees()
        {
            // Arrange
            var appUserId = "user-id-123";
            var sortOrder = "salary_desc";

            var employees = new List<EmployeeEntity>
            {
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 1", Email = "employee1@example.com", Phone = "123-456-789", Salary = 3000, DateOfBirth = new DateTime(1990, 1, 1), EmploymentDate = new DateTime(2021, 1, 1), MedicalCheckValidUntil = new DateTime(2021, 1, 1), AppUserId = appUserId },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 2", Email = "employee2@example.com", Phone = "123-456-789", Salary = 1000, DateOfBirth = new DateTime(1991, 1, 1), EmploymentDate = new DateTime(2022, 1, 1), MedicalCheckValidUntil = new DateTime(2022, 1, 1), AppUserId = appUserId },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 3", Email = "employee3@example.com", Phone = "123-456-789", Salary = 2000, DateOfBirth = new DateTime(1992, 1, 1), EmploymentDate = new DateTime(2023, 1, 1), MedicalCheckValidUntil = new DateTime(2023, 1, 1), AppUserId = appUserId }
            };

            _context.Employees.AddRange(employees);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetUserEmployeesAsync(appUserId, 1, 10, null, sortOrder);
            var sorted = result.Items.ToList();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, sorted.Count());
            Assert.AreEqual(employees[0].Id, sorted[0].Id);
            Assert.AreEqual(employees[1].Id, sorted[2].Id);
            Assert.AreEqual(employees[2].Id, sorted[1].Id);
        }

        [TestMethod]
        public async Task GetUserEmployeesAsync_SortedByDateOfBirthAscending_Returns_SortedEmployees()
        {
            // Arrange
            var appUserId = "user-id-123";
            var sortOrder = "birthDate_asc";

            var employees = new List<EmployeeEntity>
            {
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 1", Email = "employee1@example.com", Phone = "123-456-789", Salary = 3000, DateOfBirth = new DateTime(1990, 1, 1), EmploymentDate = new DateTime(2021, 1, 1), MedicalCheckValidUntil = new DateTime(2021, 1, 1), AppUserId = appUserId },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 2", Email = "employee2@example.com", Phone = "123-456-789", Salary = 1000, DateOfBirth = new DateTime(1991, 1, 1), EmploymentDate = new DateTime(2022, 1, 1), MedicalCheckValidUntil = new DateTime(2022, 1, 1), AppUserId = appUserId },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 3", Email = "employee3@example.com", Phone = "123-456-789", Salary = 2000, DateOfBirth = new DateTime(1992, 1, 1), EmploymentDate = new DateTime(2023, 1, 1), MedicalCheckValidUntil = new DateTime(2023, 1, 1), AppUserId = appUserId }
            };

            _context.Employees.AddRange(employees);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetUserEmployeesAsync(appUserId, 1, 10, null, sortOrder);
            var sorted = result.Items.ToList();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, sorted.Count());
            Assert.AreEqual(employees[0].Id, sorted[0].Id);
            Assert.AreEqual(employees[1].Id, sorted[1].Id);
            Assert.AreEqual(employees[2].Id, sorted[2].Id);
        }

        [TestMethod]
        public async Task GetUserEmployeesAsync_SortedByDateOfBirthDescending_Returns_SortedEmployees()
        {
            // Arrange
            var appUserId = "user-id-123";
            var sortOrder = "birthDate_desc";

            var employees = new List<EmployeeEntity>
            {
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 1", Email = "employee1@example.com", Phone = "123-456-789", Salary = 3000, DateOfBirth = new DateTime(1990, 1, 1), EmploymentDate = new DateTime(2021, 1, 1), MedicalCheckValidUntil = new DateTime(2021, 1, 1), AppUserId = appUserId },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 2", Email = "employee2@example.com", Phone = "123-456-789", Salary = 1000, DateOfBirth = new DateTime(1991, 1, 1), EmploymentDate = new DateTime(2022, 1, 1), MedicalCheckValidUntil = new DateTime(2022, 1, 1), AppUserId = appUserId },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 3", Email = "employee3@example.com", Phone = "123-456-789", Salary = 2000, DateOfBirth = new DateTime(1992, 1, 1), EmploymentDate = new DateTime(2023, 1, 1), MedicalCheckValidUntil = new DateTime(2023, 1, 1), AppUserId = appUserId }
            };

            _context.Employees.AddRange(employees);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetUserEmployeesAsync(appUserId, 1, 10, null, sortOrder);
            var sorted = result.Items.ToList();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, sorted.Count());
            Assert.AreEqual(employees[0].Id, sorted[2].Id);
            Assert.AreEqual(employees[1].Id, sorted[1].Id);
            Assert.AreEqual(employees[2].Id, sorted[0].Id);
        }

        [TestMethod]
        public async Task GetUserEmployeesAsync_SortedByEmploymentDateAscending_Returns_SortedEmployees()
        {
            // Arrange
            var appUserId = "user-id-123";
            var sortOrder = "employmentDate_asc";

            var employees = new List<EmployeeEntity>
            {
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 1", Email = "employee1@example.com", Phone = "123-456-789", Salary = 3000, DateOfBirth = new DateTime(1990, 1, 1), EmploymentDate = new DateTime(2021, 1, 1), MedicalCheckValidUntil = new DateTime(2021, 1, 1), AppUserId = appUserId },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 2", Email = "employee2@example.com", Phone = "123-456-789", Salary = 1000, DateOfBirth = new DateTime(1991, 1, 1), EmploymentDate = new DateTime(2022, 1, 1), MedicalCheckValidUntil = new DateTime(2022, 1, 1), AppUserId = appUserId },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 3", Email = "employee3@example.com", Phone = "123-456-789", Salary = 2000, DateOfBirth = new DateTime(1992, 1, 1), EmploymentDate = new DateTime(2023, 1, 1), MedicalCheckValidUntil = new DateTime(2023, 1, 1), AppUserId = appUserId }
            };

            _context.Employees.AddRange(employees);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetUserEmployeesAsync(appUserId, 1, 10, null, sortOrder);
            var sorted = result.Items.ToList();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, sorted.Count());
            Assert.AreEqual(employees[0].Id, sorted[0].Id);
            Assert.AreEqual(employees[1].Id, sorted[1].Id);
            Assert.AreEqual(employees[2].Id, sorted[2].Id);
        }

        [TestMethod]
        public async Task GetUserEmployeesAsync_SortedByEmploymentDateDescending_Returns_SortedEmployees()
        {
            // Arrange
            var appUserId = "user-id-123";
            var sortOrder = "employmentDate_desc";

            var employees = new List<EmployeeEntity>
            {
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 1", Email = "employee1@example.com", Phone = "123-456-789", Salary = 3000, DateOfBirth = new DateTime(1990, 1, 1), EmploymentDate = new DateTime(2021, 1, 1), MedicalCheckValidUntil = new DateTime(2021, 1, 1), AppUserId = appUserId },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 2", Email = "employee2@example.com", Phone = "123-456-789", Salary = 1000, DateOfBirth = new DateTime(1991, 1, 1), EmploymentDate = new DateTime(2022, 1, 1), MedicalCheckValidUntil = new DateTime(2022, 1, 1), AppUserId = appUserId },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 3", Email = "employee3@example.com", Phone = "123-456-789", Salary = 2000, DateOfBirth = new DateTime(1992, 1, 1), EmploymentDate = new DateTime(2023, 1, 1), MedicalCheckValidUntil = new DateTime(2023, 1, 1), AppUserId = appUserId }
            };

            _context.Employees.AddRange(employees);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetUserEmployeesAsync(appUserId, 1, 10, null, sortOrder);
            var sorted = result.Items.ToList();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, sorted.Count());
            Assert.AreEqual(employees[0].Id, sorted[2].Id);
            Assert.AreEqual(employees[1].Id, sorted[1].Id);
            Assert.AreEqual(employees[2].Id, sorted[0].Id);
        }

        [TestMethod]
        public async Task GetUserEmployeesAsync_SortedByMedicalCheckValidUntilAscending_Returns_SortedEmployees()
        {
            // Arrange
            var appUserId = "user-id-123";
            var sortOrder = "medicalCheckValidUntil_asc";

            var employees = new List<EmployeeEntity>
            {
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 1", Email = "employee1@example.com", Phone = "123-456-789", Salary = 3000, DateOfBirth = new DateTime(1990, 1, 1), EmploymentDate = new DateTime(2021, 1, 1), MedicalCheckValidUntil = new DateTime(2021, 1, 1), AppUserId = appUserId },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 2", Email = "employee2@example.com", Phone = "123-456-789", Salary = 1000, DateOfBirth = new DateTime(1991, 1, 1), EmploymentDate = new DateTime(2022, 1, 1), MedicalCheckValidUntil = new DateTime(2022, 1, 1), AppUserId = appUserId },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 3", Email = "employee3@example.com", Phone = "123-456-789", Salary = 2000, DateOfBirth = new DateTime(1992, 1, 1), EmploymentDate = new DateTime(2023, 1, 1), MedicalCheckValidUntil = new DateTime(2023, 1, 1), AppUserId = appUserId }
            };

            _context.Employees.AddRange(employees);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetUserEmployeesAsync(appUserId, 1, 10, null, sortOrder);
            var sorted = result.Items.ToList();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, sorted.Count());
            Assert.AreEqual(employees[0].Id, sorted[0].Id);
            Assert.AreEqual(employees[1].Id, sorted[1].Id);
            Assert.AreEqual(employees[2].Id, sorted[2].Id);
        }

        [TestMethod]
        public async Task GetUserEmployeesAsync_SortedByMedicalCheckValidUntilDescending_Returns_SortedEmployees()
        {
            // Arrange
            var appUserId = "user-id-123";
            var sortOrder = "medicalCheckValidUntil_desc";

            var employees = new List<EmployeeEntity>
            {
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 1", Email = "employee1@example.com", Phone = "123-456-789", Salary = 3000, DateOfBirth = new DateTime(1990, 1, 1), EmploymentDate = new DateTime(2021, 1, 1), MedicalCheckValidUntil = new DateTime(2021, 1, 1), AppUserId = appUserId },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 2", Email = "employee2@example.com", Phone = "123-456-789", Salary = 1000, DateOfBirth = new DateTime(1991, 1, 1), EmploymentDate = new DateTime(2022, 1, 1), MedicalCheckValidUntil = new DateTime(2022, 1, 1), AppUserId = appUserId },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 3", Email = "employee3@example.com", Phone = "123-456-789", Salary = 2000, DateOfBirth = new DateTime(1992, 1, 1), EmploymentDate = new DateTime(2023, 1, 1), MedicalCheckValidUntil = new DateTime(2023, 1, 1), AppUserId = appUserId }
            };

            _context.Employees.AddRange(employees);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetUserEmployeesAsync(appUserId, 1, 10, null, sortOrder);
            var sorted = result.Items.ToList();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, sorted.Count());
            Assert.AreEqual(employees[0].Id, sorted[2].Id);
            Assert.AreEqual(employees[1].Id, sorted[1].Id);
            Assert.AreEqual(employees[2].Id, sorted[0].Id);
        }

        [TestMethod]
        public async Task GetUserEmployeesAsync_When_SortedDoesNotExist_Returns_SortedEmployees()
        {
            // Arrange
            var appUserId = "user-id-123";
            var sortOrder = "nonexistent";

            var employees = new List<EmployeeEntity>
            {
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 1", Email = "employee1@example.com", Phone = "123-456-789", Salary = 3000, DateOfBirth = new DateTime(1990, 1, 1), EmploymentDate = new DateTime(2021, 1, 1), MedicalCheckValidUntil = new DateTime(2021, 1, 1), AppUserId = appUserId },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 2", Email = "employee2@example.com", Phone = "123-456-789", Salary = 1000, DateOfBirth = new DateTime(1991, 1, 1), EmploymentDate = new DateTime(2022, 1, 1), MedicalCheckValidUntil = new DateTime(2022, 1, 1), AppUserId = appUserId },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 3", Email = "employee3@example.com", Phone = "123-456-789", Salary = 2000, DateOfBirth = new DateTime(1992, 1, 1), EmploymentDate = new DateTime(2023, 1, 1), MedicalCheckValidUntil = new DateTime(2023, 1, 1), AppUserId = appUserId }
            };

            _context.Employees.AddRange(employees);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetUserEmployeesAsync(appUserId, 1, 10, null, sortOrder);
            var sorted = result.Items.ToList();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, sorted.Count());
            Assert.AreEqual(employees[0].Id, sorted[2].Id);
            Assert.AreEqual(employees[1].Id, sorted[1].Id);
            Assert.AreEqual(employees[2].Id, sorted[0].Id);
        }

        [TestMethod]
        public async Task GetEmployeesAsync_Returns_AllEmployees()
        {
            // Arrange
            var appUserId = "user-id-123";

            var employees = new List<EmployeeEntity>
            {
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 1", Email = "employee1@example.com", Phone = "123-456-789", Salary = 5000, DateOfBirth = new DateTime(1990, 1, 1), EmploymentDate = new DateTime(2021, 1, 1), MedicalCheckValidUntil = new DateTime(2021, 1, 1), AppUserId = appUserId },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 2", Email = "employee2@example.com", Phone = "123-456-789", Salary = 5000, DateOfBirth = new DateTime(1991, 1, 1), EmploymentDate = new DateTime(2022, 1, 1), MedicalCheckValidUntil = new DateTime(2022, 1, 1), AppUserId = appUserId },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 3", Email = "employee3@example.com", Phone = "123-456-789", Salary = 5000, DateOfBirth = new DateTime(1992, 1, 1), EmploymentDate = new DateTime(2023, 1, 1), MedicalCheckValidUntil = new DateTime(2023, 1, 1), AppUserId = appUserId },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 4", Email = "employee4@example.com", Phone = "123-456-789", Salary = 5000, DateOfBirth = new DateTime(1993, 1, 1), EmploymentDate = new DateTime(2024, 1, 1), MedicalCheckValidUntil = new DateTime(2024, 1, 1), AppUserId = appUserId },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 5", Email = "employee5@example.com", Phone = "123-456-789", Salary = 5000, DateOfBirth = new DateTime(1994, 1, 1), EmploymentDate = new DateTime(2025, 1, 1), MedicalCheckValidUntil = new DateTime(2025, 1, 1), AppUserId = appUserId }
            };

            _context.Employees.AddRange(employees);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetEmployeesAsync(1, 10, null, null);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(5, result.Items.Count());
        }

        [TestMethod]
        public async Task GetEmployeesAsync_BySearchTerm_Returns_Employees()
        {
            // Arrange
            var appUserId = "user-id-123";
            var searchTerm = "test";

            var employees = new List<EmployeeEntity>
            {
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 1 Test", Email = "employee1@example.com", Phone = "123-456-789", Salary = 5000, DateOfBirth = new DateTime(1990, 1, 1), EmploymentDate = new DateTime(2021, 1, 1), MedicalCheckValidUntil = new DateTime(2021, 1, 1), AppUserId = appUserId },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 2", Email = "employee2@example.com", Phone = "123-456-789", Salary = 5000, DateOfBirth = new DateTime(1991, 1, 1), EmploymentDate = new DateTime(2022, 1, 1), MedicalCheckValidUntil = new DateTime(2022, 1, 1), AppUserId = appUserId },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 3", Email = "employee3@example.com", Phone = "123-456-789", Salary = 5000, DateOfBirth = new DateTime(1992, 1, 1), EmploymentDate = new DateTime(2023, 1, 1), MedicalCheckValidUntil = new DateTime(2023, 1, 1), AppUserId = appUserId },
            };

            _context.Employees.AddRange(employees);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetEmployeesAsync(1, 10, searchTerm, null);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Items.Count());
            Assert.AreEqual(employees[0].Name, result.Items.First().Name);
        }

        [TestMethod]
        public async Task GetEmployeesAsync_When_EmployeesDoesNotExist_Returns_EmptyList()
        {
            // Arrange
            var appUserId = "user-id-123";
            var searchTerm = "nonexistent";

            var employees = new List<EmployeeEntity>
            {
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 1", Email = "employee1@example.com", Phone = "123-456-789", Salary = 5000,  DateOfBirth = new DateTime(1990, 1, 1), EmploymentDate = new DateTime(2021, 1, 1), MedicalCheckValidUntil = new DateTime(2021, 1, 1), AppUserId = appUserId },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 2", Email = "employee2@example.com", Phone = "123-456-789", Salary = 5000, DateOfBirth = new DateTime(1991, 1, 1), EmploymentDate = new DateTime(2022, 1, 1), MedicalCheckValidUntil = new DateTime(2022, 1, 1), AppUserId = appUserId },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 3", Email = "employee3@example.com", Phone = "123-456-789", Salary = 5000,  DateOfBirth = new DateTime(1992, 1, 1), EmploymentDate = new DateTime(2023, 1, 1), MedicalCheckValidUntil = new DateTime(2023, 1, 1), AppUserId = appUserId },
            };

            _context.Employees.AddRange(employees);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetEmployeesAsync(1, 10, searchTerm, null);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Items.Count());
        }

        [TestMethod]
        public async Task GetUserEmployeeListsAsync_Returns_AllEmployeeList()
        {
            // Arrange
            var appUserId = "user-id-123";

            var employeeList = new List<EmployeeListsEntity>
            {
                new EmployeeListsEntity { Id = Guid.NewGuid(), Name = "EmployeeList 1", AppUserId = appUserId },
                new EmployeeListsEntity { Id = Guid.NewGuid(), Name = "EmployeeList 2", AppUserId = appUserId },
                new EmployeeListsEntity { Id = Guid.NewGuid(), Name = "EmployeeList 3", AppUserId = appUserId }
            };

            _context.EmployeeLists.AddRange(employeeList);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetUserEmployeeListsAsync(appUserId, null);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count());
        }

        [TestMethod]
        public async Task GetUserEmployeeListsAsync_BySearchTerm_Returns_EmployeeList()
        {
            // Arrange
            var appUserId = "user-id-123";
            var searchTerm = "test";

            var employeeList = new List<EmployeeListsEntity>
            {
                new EmployeeListsEntity { Id = Guid.NewGuid(), Name = "EmployeeList 1 Test", AppUserId = appUserId },
                new EmployeeListsEntity { Id = Guid.NewGuid(), Name = "EmployeeList 2", AppUserId = appUserId },
                new EmployeeListsEntity { Id = Guid.NewGuid(), Name = "EmployeeList 3", AppUserId = "user-id-999" }
            };

            _context.EmployeeLists.AddRange(employeeList);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetUserEmployeeListsAsync(appUserId, searchTerm);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(employeeList[0].Name, result.First().Name);
        }

        [TestMethod]
        public async Task GetUserEmployeeListsAsync_When_EmployeeListsDoesNotExist_Returns_EmptyList()
        {
            // Arrange
            var appUserId = "user-id-123";
            var searchTerm = "nonexistent";

            var employeeList = new List<EmployeeListsEntity>
            {
                new EmployeeListsEntity { Id = Guid.NewGuid(), Name = "EmployeeList 1", AppUserId = appUserId },
                new EmployeeListsEntity { Id = Guid.NewGuid(), Name = "EmployeeList 2", AppUserId = appUserId },
                new EmployeeListsEntity { Id = Guid.NewGuid(), Name = "EmployeeList 3", AppUserId = "user-id-999" }
            };

            _context.EmployeeLists.AddRange(employeeList);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetUserEmployeeListsAsync(appUserId, searchTerm);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public async Task GetUserEmployeeListsForTaskAsync_Returns_OnlyEmployeeListsWithoutTask()
        {
            // Arrange
            var appUserId = "user-id-123";

            var employeeList = new List<EmployeeListsEntity>
            {
                new EmployeeListsEntity { Id = Guid.NewGuid(), Name = "EmployeeList 1", AppUserId = appUserId, TaskId = null },
                new EmployeeListsEntity { Id = Guid.NewGuid(), Name = "EmployeeList 2", AppUserId = appUserId, TaskId = null },
                new EmployeeListsEntity { Id = Guid.NewGuid(), Name = "EmployeeList 3", AppUserId = appUserId, TaskId = Guid.NewGuid() },
                new EmployeeListsEntity { Id = Guid.NewGuid(), Name = "EmployeeList 4", AppUserId = "user-id-1234", TaskId = Guid.NewGuid() }
            };

            _context.EmployeeLists.AddRange(employeeList);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetUserEmployeeListsForTaskAsync(appUserId, null);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual(employeeList[0].Name, result.First().Name);
        }

        [TestMethod]
        public async Task GetUserEmployeeListsForTaskAsync_BySearchTerm_Returns_EmployeeLists()
        {
            // Arrange
            var appUserId = "user-id-123";
            var searchTerm = "test";

            var employeeList = new List<EmployeeListsEntity>
            {
                new EmployeeListsEntity { Id = Guid.NewGuid(), Name = "EmployeeList 1 Test", AppUserId = appUserId, TaskId = null },
                new EmployeeListsEntity { Id = Guid.NewGuid(), Name = "EmployeeList 2 Test", AppUserId = appUserId, TaskId = null},
                new EmployeeListsEntity { Id = Guid.NewGuid(), Name = "EmployeeList 3", AppUserId = appUserId, TaskId = null },
                new EmployeeListsEntity { Id = Guid.NewGuid(), Name = "EmployeeList 4", AppUserId = appUserId, TaskId = null }
            };

            _context.EmployeeLists.AddRange(employeeList);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetUserEmployeeListsForTaskAsync(appUserId, searchTerm);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual(employeeList[0].Name, result.First().Name);
        }


        [TestMethod]
        public async Task GetUserEmployeeListsForTaskAsync_When_EmployeeListsDoesNotExist_Returns_EmptyList()
        {
            // Arrange
            var appUserId = "user-id-123";
            var searchTerm = "nonexistent";

            var employeeList = new List<EmployeeListsEntity>
            {
                new EmployeeListsEntity { Id = Guid.NewGuid(), Name = "EmployeeList 1", AppUserId = appUserId },
                new EmployeeListsEntity { Id = Guid.NewGuid(), Name = "EmployeeList 2", AppUserId = appUserId },
                new EmployeeListsEntity { Id = Guid.NewGuid(), Name = "EmployeeList 3", AppUserId = appUserId },
                new EmployeeListsEntity { Id = Guid.NewGuid(), Name = "EmployeeList 4", AppUserId = "user-id-999" }
            };

            _context.EmployeeLists.AddRange(employeeList);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetUserEmployeeListsForTaskAsync(appUserId, searchTerm);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public async Task GetUserEmployeesForListAddAsync_Returns_OnlyEmployeesWithoutList()
        {
            // Arrange
            var appUserId = "user-id-123";
            var listId = Guid.NewGuid();

            var employees = new List<EmployeeEntity>
            {
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 1", Email = "employee1@example.com", Phone = "123-456-789", Salary = 5000, DateOfBirth = new DateTime(1990, 1, 1), EmploymentDate = new DateTime(2021, 1, 1), MedicalCheckValidUntil = new DateTime(2021, 1, 1), AppUserId = appUserId, EmployeeListId = null },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 2", Email = "employee2@example.com", Phone = "123-456-789", Salary = 5000, DateOfBirth = new DateTime(1991, 1, 1), EmploymentDate = new DateTime(2022, 1, 1), MedicalCheckValidUntil = new DateTime(2022, 1, 1), AppUserId = appUserId, EmployeeListId = null },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 3", Email = "employee3@example.com", Phone = "123-456-789", Salary = 5000, DateOfBirth = new DateTime(1992, 1, 1), EmploymentDate = new DateTime(2023, 1, 1), MedicalCheckValidUntil = new DateTime(2023, 1, 1), AppUserId = appUserId, EmployeeListId = null },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 4", Email = "employee4@example.com", Phone = "123-456-789", Salary = 5000, DateOfBirth = new DateTime(1993, 1, 1), EmploymentDate = new DateTime(2024, 1, 1), MedicalCheckValidUntil = new DateTime(2024, 1, 1), AppUserId = appUserId, EmployeeListId = listId },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 5", Email = "employee5@example.com", Phone = "123-456-789", Salary = 5000, DateOfBirth = new DateTime(1994, 1, 1), EmploymentDate = new DateTime(2025, 1, 1), MedicalCheckValidUntil = new DateTime(2025, 1, 1), AppUserId = "user-id-999", EmployeeListId =  null  }
            };

            _context.Employees.AddRange(employees);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetUserEmployeesForListAddAsync(appUserId, null);

            // Assert
            Assert.AreEqual(3, result.Count());
            Assert.AreEqual(employees[0].Name, result.First().Name);
        }

        [TestMethod]
        public async Task GetUserEmployeesForListAddAsync_BySearchTerm_Returns_Employees()
        {
            // Arrange
            var appUserId = "user-id-123";
            var listId = Guid.NewGuid();
            var searchTerm = "test";

            var employees = new List<EmployeeEntity>
            {
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 1 Test", Email = "employee1@example.com", Phone = "123-456-789", Salary = 5000, DateOfBirth = new DateTime(1990, 1, 1), EmploymentDate = new DateTime(2021, 1, 1), MedicalCheckValidUntil = new DateTime(2021, 1, 1), AppUserId = appUserId, EmployeeListId = null },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 2 Test", Email = "employee2@example.com", Phone = "123-456-789", Salary = 5000, DateOfBirth = new DateTime(1991, 1, 1), EmploymentDate = new DateTime(2022, 1, 1), MedicalCheckValidUntil = new DateTime(2022, 1, 1), AppUserId = appUserId, EmployeeListId = null },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 3", Email = "employee3@example.com", Phone = "123-456-789", Salary = 5000, DateOfBirth = new DateTime(1992, 1, 1), EmploymentDate = new DateTime(2023, 1, 1), MedicalCheckValidUntil = new DateTime(2023, 1, 1), AppUserId = appUserId, EmployeeListId = null },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 4", Email = "employee4@example.com", Phone = "123-456-789", Salary = 5000, DateOfBirth = new DateTime(1993, 1, 1), EmploymentDate = new DateTime(2024, 1, 1), MedicalCheckValidUntil = new DateTime(2024, 1, 1), AppUserId = appUserId, EmployeeListId = null },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 5", Email = "employee5@example.com", Phone = "123-456-789", Salary = 5000, DateOfBirth = new DateTime(1994, 1, 1), EmploymentDate = new DateTime(2025, 1, 1), MedicalCheckValidUntil = new DateTime(2025, 1, 1), AppUserId = appUserId, EmployeeListId = listId },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 6", Email = "employee6@example.com", Phone = "123-456-789", Salary = 5000, DateOfBirth = new DateTime(1995, 1, 1), EmploymentDate = new DateTime(2026, 1, 1), MedicalCheckValidUntil = new DateTime(2026, 1, 1), AppUserId = "user-id-999", EmployeeListId =  null  }
            };

            _context.Employees.AddRange(employees);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetUserEmployeesForListAddAsync(appUserId, searchTerm);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual(employees[0].Name, result.First().Name);
        }

        [TestMethod]
        public async Task GetUserEmployeesForListAddAsync_When_EmployeesDoesNotExist_Returns_EmptyList()
        {
            // Arrange
            var appUserId = "user-id-123";
            var listId = Guid.NewGuid();
            var searchTerm = "nonexistent";

            var employees = new List<EmployeeEntity>
            {
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 1", Email = "employee1@example.com", Phone = "123-456-789", Salary = 5000, DateOfBirth = new DateTime(1990, 1, 1), EmploymentDate = new DateTime(2021, 1, 1), MedicalCheckValidUntil = new DateTime(2021, 1, 1), AppUserId = appUserId, EmployeeListId = null },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 2", Email = "employee2@example.com", Phone = "123-456-789", Salary = 5000, DateOfBirth = new DateTime(1991, 1, 1), EmploymentDate = new DateTime(2022, 1, 1), MedicalCheckValidUntil = new DateTime(2022, 1, 1), AppUserId = appUserId, EmployeeListId = null },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 3", Email = "employee3@example.com", Phone = "123-456-789", Salary = 5000, DateOfBirth = new DateTime(1992, 1, 1), EmploymentDate = new DateTime(2023, 1, 1), MedicalCheckValidUntil = new DateTime(2023, 1, 1), AppUserId = appUserId, EmployeeListId = null },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 4", Email = "employee4@example.com", Phone = "123-456-789", Salary = 5000, DateOfBirth = new DateTime(1993, 1, 1), EmploymentDate = new DateTime(2024, 1, 1), MedicalCheckValidUntil = new DateTime(2024, 1, 1), AppUserId = appUserId, EmployeeListId = null },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 5", Email = "employee5@example.com", Phone = "123-456-789", Salary = 5000, DateOfBirth = new DateTime(1994, 1, 1), EmploymentDate = new DateTime(2025, 1, 1), MedicalCheckValidUntil = new DateTime(2025, 1, 1), AppUserId = appUserId, EmployeeListId = listId },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 6", Email = "employee6@example.com", Phone = "123-456-789", Salary = 5000, DateOfBirth = new DateTime(1995, 1, 1), EmploymentDate = new DateTime(2026, 1, 1), MedicalCheckValidUntil = new DateTime(2026, 1, 1), AppUserId = "user-id-999", EmployeeListId =  null  }
            };

            _context.Employees.AddRange(employees);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetUserEmployeesForListAddAsync(appUserId, searchTerm);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public async Task EmployeeListExistsForAddAsync_When_ListExists_Returns_True()
        {
            // Arrange
            var appUserId = "user-id-123";

            var existingList = new EmployeeListsEntity
            {
                Id = Guid.NewGuid(),
                Name = "EmployeeList",
                AppUserId = appUserId
            };

            _context.EmployeeLists.Add(existingList);
            await _context.SaveChangesAsync();

            var employeeList = new EmployeeListsEntity
            {
                Name = "EmployeeList",
                AppUserId = appUserId
            };

            // Act
            var result = await _repository.EmployeeListExistsForAddAsync(employeeList.Name, appUserId);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task EmployeeListExistsForAddAsync_When_ListNoExists_Returns_False()
        {
            // Arrange
            var appUserId = "user-id-123";

            var existingList = new EmployeeListsEntity
            {
                Id = Guid.NewGuid(),
                Name = "EmployeeList Test",
                AppUserId = appUserId
            };

            _context.EmployeeLists.Add(existingList);
            await _context.SaveChangesAsync();

            var employeeList = new EmployeeListsEntity
            {
                Name = "EmployeeList",
                AppUserId = appUserId
            };

            // Act
            var result = await _repository.EmployeeListExistsForAddAsync(employeeList.Name, appUserId);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task EmployeeListExistsForUpdateAsync_When_ListExistsButDifferentId_Returns_True()
        {
            // Arrange
            var appUserId = "user-id-123";

            var existingList = new EmployeeListsEntity
            {
                Id = Guid.NewGuid(),
                Name = "EmployeeList",
                AppUserId = appUserId
            };

            _context.EmployeeLists.Add(existingList);
            await _context.SaveChangesAsync();

            var employeeList = new EmployeeListsEntity
            {
                Id = Guid.NewGuid(),
                Name = "EmployeeList",
                AppUserId = appUserId
            };

            // Act
            var result = await _repository.EmployeeListExistsForUpdateAsync(employeeList.Name, appUserId, employeeList.Id);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task EmployeeListExistsForUpdateAsync_When_ListExistsButSameId_Returns_False()
        {
            // Arrange
            var appUserId = "user-id-123";
            var employeeListId = Guid.NewGuid();

            var existingList = new EmployeeListsEntity
            {
                Id = employeeListId,
                Name = "EmployeeList",
                AppUserId = appUserId
            };

            _context.EmployeeLists.Add(existingList);
            await _context.SaveChangesAsync();

            var employeeList = new EmployeeListsEntity
            {
                Id = employeeListId,
                Name = "EmployeeList",
                AppUserId = appUserId
            };

            // Act
            var result = await _repository.EmployeeListExistsForUpdateAsync(employeeList.Name, appUserId, employeeList.Id);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task EmployeeListExistsForUpdateAsyncAsync_When_ListNoExists_Returns_False()
        {
            // Arrange
            var appUserId = "user-id-123";

            var existingList = new EmployeeListsEntity
            {
                Id = Guid.NewGuid(),
                Name = "EmployeeList",
                AppUserId = appUserId
            };

            _context.EmployeeLists.Add(existingList);
            await _context.SaveChangesAsync();

            var employeeList = new EmployeeListsEntity
            {
                Id = Guid.NewGuid(),
                Name = "EmployeeList Test",
                AppUserId = appUserId
            };

            // Act
            var result = await _repository.EmployeeListExistsForUpdateAsync(employeeList.Name, appUserId, employeeList.Id);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task GetUserNumberOfEmployeesAsync_Returns_TotalCount()
        {
            // Arrange
            var appUserId1 = "user-id-123";
            var appUserId2 = "user-id-1234";

            var employees = new List<EmployeeEntity>
            {
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 1", Email = "employee1@example.com", Phone = "123-456-789", Salary = 5000, DateOfBirth = new DateTime(1990, 1, 1), EmploymentDate = new DateTime(2021, 1, 1), MedicalCheckValidUntil = new DateTime(2021, 1, 1), AppUserId = appUserId1 },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 2", Email = "employee2@example.com", Phone = "123-456-789", Salary = 5000, DateOfBirth = new DateTime(1991, 1, 1), EmploymentDate = new DateTime(2022, 1, 1), MedicalCheckValidUntil = new DateTime(2022, 1, 1), AppUserId = appUserId1 },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 3", Email = "employee3@example.com", Phone = "123-456-789", Salary = 5000, DateOfBirth = new DateTime(1992, 1, 1), EmploymentDate = new DateTime(2023, 1, 1), MedicalCheckValidUntil = new DateTime(2023, 1, 1), AppUserId = appUserId2 }
            };

            _context.Employees.AddRange(employees);
            await _context.SaveChangesAsync();

            // Act
            var count = await _repository.GetUserNumberOfEmployeesAsync(appUserId1);

            // Assert
            Assert.AreEqual(employees.Where(x => x.AppUserId == appUserId1).Count(), count);
        }

        [TestMethod]
        public async Task GetNumberOfEmployeesAsync_Returns_TotalCount()
        {
            // Arrange
            var appUserId = "user-id-123";

            var employees = new List<EmployeeEntity>
            {
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 1", Email = "employee1@example.com", Phone = "123-456-789", Salary = 5000, DateOfBirth = new DateTime(1990, 1, 1), EmploymentDate = new DateTime(2021, 1, 1), MedicalCheckValidUntil = new DateTime(2021, 1, 1), AppUserId = appUserId },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 2", Email = "employee2@example.com", Phone = "123-456-789", Salary = 5000, DateOfBirth = new DateTime(1991, 1, 1), EmploymentDate = new DateTime(2022, 1, 1), MedicalCheckValidUntil = new DateTime(2022, 1, 1), AppUserId = appUserId },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 3", Email = "employee3@example.com", Phone = "123-456-789", Salary = 5000, DateOfBirth = new DateTime(1992, 1, 1), EmploymentDate = new DateTime(2023, 1, 1), MedicalCheckValidUntil = new DateTime(2023, 1, 1), AppUserId = appUserId }
            };

            _context.Employees.AddRange(employees);
            await _context.SaveChangesAsync();

            // Act
            var count = await _repository.GetNumberOfEmployeesAsync();

            // Assert
            Assert.AreEqual(employees.Count(), count);
        }

        [TestMethod]
        public async Task UpdateEmployeeAsync_When_EmployeeExists_Returns_Employee()
        {
            // Arrange
            var appUserId = "user-id-123";

            var employee = new EmployeeEntity
            {
                Name = "Employee",
                Email = "employee@example.com",
                Phone = "123-456-789",
                Salary = 5000,
                DateOfBirth = new DateTime(1990, 1, 1),
                EmploymentDate = new DateTime(2022, 1, 1),
                MedicalCheckValidUntil = new DateTime(2025, 1, 1),
                AppUserId = appUserId
            };

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            var updatedEmployee = new EmployeeEntity
            {
                Name = "Employee Test",
                Email = "employee@example.com",
                Phone = "123-456-789",
                Salary = 6500,
                DateOfBirth = new DateTime(1990, 1, 1),
                EmploymentDate = new DateTime(2022, 1, 1),
                MedicalCheckValidUntil = new DateTime(2025, 1, 1),
                AppUserId = appUserId
            };

            // Act
            var result = await _repository.UpdateEmployeeAsync(employee.Id, appUserId, updatedEmployee);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(employee.Id, result.Id);
            Assert.AreEqual(updatedEmployee.Name, result.Name);
            Assert.AreEqual(updatedEmployee.Email, result.Email);
            Assert.AreEqual(updatedEmployee.Phone, result.Phone);
            Assert.AreEqual(updatedEmployee.Salary, result.Salary);
        }

        [TestMethod]
        public async Task UpdateEmployeeAsync_When_EmployeeDoesNotExist_Returns_Entity()
        {
            // Arrange
            var nonExistentId = Guid.NewGuid();
            var appUserId = "user-id-123";

            var updatedEmployee = new EmployeeEntity
            {
                Name = "Employee",
                Email = "employee@example.com",
                Phone = "123-456-789",
                Salary = 5000,
                DateOfBirth = new DateTime(1990, 1, 1),
                EmploymentDate = new DateTime(2022, 1, 1),
                MedicalCheckValidUntil = new DateTime(2025, 1, 1),
                AppUserId = "user-id-123"
            };

            // Act
            var result = await _repository.UpdateEmployeeAsync(nonExistentId, appUserId, updatedEmployee);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(updatedEmployee.Name, result.Name);
            Assert.AreEqual(updatedEmployee.Email, result.Email);
            Assert.AreEqual(updatedEmployee.Phone, result.Phone);
            Assert.AreEqual(updatedEmployee.Salary, result.Salary);
            Assert.AreEqual(updatedEmployee.DateOfBirth, result.DateOfBirth);
            Assert.AreEqual(updatedEmployee.EmploymentDate, result.EmploymentDate);
            Assert.AreEqual(updatedEmployee.MedicalCheckValidUntil, result.MedicalCheckValidUntil);
        }

        [TestMethod]
        public async Task UpdateEmployeeListAsync_When_EmployeeListExists_Returns_EmployeeList()
        {
            // Arrange
            var appUserId = "user-id-123";

            var employees = new List<EmployeeEntity>
            {
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 1", Email = "employee1@example.com", Phone = "123-456-789", Salary = 5000, DateOfBirth = new DateTime(1990, 1, 1), EmploymentDate = new DateTime(2021, 1, 1), MedicalCheckValidUntil = new DateTime(2021, 1, 1), AppUserId = appUserId },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 2", Email = "employee2@example.com", Phone = "123-456-789", Salary = 5000, DateOfBirth = new DateTime(1991, 1, 1), EmploymentDate = new DateTime(2022, 1, 1), MedicalCheckValidUntil = new DateTime(2022, 1, 1), AppUserId = appUserId },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 3", Email = "employee3@example.com", Phone = "123-456-789", Salary = 5000, DateOfBirth = new DateTime(1992, 1, 1), EmploymentDate = new DateTime(2023, 1, 1), MedicalCheckValidUntil = new DateTime(2023, 1, 1), AppUserId = appUserId },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 4", Email = "employee4@example.com", Phone = "123-456-789", Salary = 5000, DateOfBirth = new DateTime(1993, 1, 1), EmploymentDate = new DateTime(2024, 1, 1), MedicalCheckValidUntil = new DateTime(2024, 1, 1), AppUserId = appUserId },
            };

            _context.Employees.AddRange(employees);
            await _context.SaveChangesAsync();

            var employeeList = new EmployeeListsEntity
            {
                Name = "EmployeeList",
                AppUserId = appUserId,
                EmployeesEntities = new List<EmployeeEntity> { employees[0], employees[1] }
            };

            _context.EmployeeLists.Add(employeeList);
            await _context.SaveChangesAsync();

            var updatedEmployeeList = new EmployeeListsEntity
            {
                Name = "EmployeeList Test",
                AppUserId = appUserId
            };

            // Act
            var result = await _repository.UpdateEmployeeListAsync(employeeList.Id, appUserId, updatedEmployeeList, new List<Guid> { employees[0].Id, employees[1].Id, employees[2].Id, employees[3].Id });

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(employeeList.Id, result.Id);
            Assert.AreEqual(employeeList.Name, result.Name);
            Assert.AreEqual(4, result.EmployeesEntities.Count());
        }

        [TestMethod]
        public async Task DeleteEmployeeAsync_When_EmployeeExists_Returns_True()
        {
            // Arrange
            var appUserId = "user-id-123";

            var employee = new EmployeeEntity
            {
                Id = Guid.NewGuid(),
                Name = "Employee",
                Email = "employee@example.com",
                Phone = "123-456-789",
                Salary = 6000,
                DateOfBirth = new DateTime(1990, 1, 1),
                EmploymentDate = new DateTime(2021, 6, 15),
                MedicalCheckValidUntil = new DateTime(2024, 6, 15),
                AppUserId = appUserId
            };

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            var employeeCountBefore = await _context.Employees.CountAsync();

            // Act
            var result = await _repository.DeleteEmployeeAsync(employee.Id, appUserId);

            var deletedEmployee = await _context.Employees.FirstOrDefaultAsync(x => x.Id == employee.Id && x.AppUserId == appUserId);

            var employeeCountAfter = await _context.Employees.CountAsync();

            // Assert
            Assert.IsTrue(result);
            Assert.IsNull(deletedEmployee);
            Assert.AreEqual(employeeCountBefore - 1, employeeCountAfter);
        }

        [TestMethod]
        public async Task DeleteEmployeeAsync_When_EmployeeDoesNotExist_Returns_False()
        {
            // Arrange
            var appUserId = "user-id-123";

            // Act
            var result = await _repository.DeleteEmployeeAsync(Guid.NewGuid(), appUserId);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task DeleteEmployeeListsAsync_When_EmployeeListsExists_Returns_True()
        {
            // Arrange
            var appUserId = "user-id-123";

            var employeeList = new List<EmployeeListsEntity>
            {
                new EmployeeListsEntity { Id = Guid.NewGuid(), Name = "EmployeeList 1", AppUserId = appUserId },
                new EmployeeListsEntity { Id = Guid.NewGuid(), Name = "EmployeeList 2", AppUserId = appUserId },
                new EmployeeListsEntity { Id = Guid.NewGuid(), Name = "EmployeeList 3", AppUserId = appUserId },
            };

            var employees = new List<EmployeeEntity>
            {
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 1", Email = "employee1@example.com", Phone = "123-456-789", Salary = 5000, DateOfBirth = new DateTime(1990, 1, 1), EmploymentDate = new DateTime(2021, 1, 1), MedicalCheckValidUntil = new DateTime(2021, 1, 1), AppUserId = appUserId, EmployeeListId = employeeList[0].Id },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 2", Email = "employee2@example.com", Phone = "123-456-789", Salary = 5000, DateOfBirth = new DateTime(1991, 1, 1), EmploymentDate = new DateTime(2022, 1, 1), MedicalCheckValidUntil = new DateTime(2022, 1, 1), AppUserId = appUserId, EmployeeListId = employeeList[0].Id },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 3", Email = "employee3@example.com", Phone = "123-456-789", Salary = 5000, DateOfBirth = new DateTime(1992, 1, 1), EmploymentDate = new DateTime(2023, 1, 1), MedicalCheckValidUntil = new DateTime(2023, 1, 1), AppUserId = appUserId, EmployeeListId = employeeList[1].Id },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 4", Email = "employee4@example.com", Phone = "123-456-789", Salary = 5000, DateOfBirth = new DateTime(1993, 1, 1), EmploymentDate = new DateTime(2024, 1, 1), MedicalCheckValidUntil = new DateTime(2024, 1, 1), AppUserId = appUserId, EmployeeListId = employeeList[2].Id },
            };

            _context.EmployeeLists.AddRange(employeeList);
            _context.Employees.AddRange(employees);
            await _context.SaveChangesAsync();

            var employeeListCountBefore = await _context.EmployeeLists.CountAsync();

            // Act
            var result = await _repository.DeleteEmployeeListsAsync(employeeList[0].Id, appUserId);

            var deletedEmployeeList = await _context.EmployeeLists.FirstOrDefaultAsync(x => x.Id == employeeList[0].Id && x.AppUserId == appUserId);

            var noEmployeesReferenceDeletedList = employees.All(x => x.EmployeeListId != employeeList[0].Id);

            var employeeListCountAfter = await _context.EmployeeLists.CountAsync();

            // Assert
            Assert.IsTrue(result);
            Assert.IsNull(deletedEmployeeList);
            Assert.IsTrue(noEmployeesReferenceDeletedList);
            Assert.AreEqual(employeeListCountBefore - 1, employeeListCountAfter);
        }

        [TestMethod]
        public async Task DeleteEmployeeListsAsync_When_EmployeeListsDoesNotExist_Returns_False()
        {
            // Arrange
            var appUserId = "user-id-123";

            // Act
            var result = await _repository.DeleteEmployeeListsAsync(Guid.NewGuid(), appUserId);

            // Assert
            Assert.IsFalse(result);
        }
    }
}