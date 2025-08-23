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
            var appUserId = "user-id-123";
            var searchTerm = "Tomasz";

            var employees = new List<EmployeeEntity>
            {
                new EmployeeEntity { Name = "Grzegorz", AppUserId = appUserId, Email = "grzegorz@example.com", Phone = "111111111" },
                new EmployeeEntity { Name = "Janusz", AppUserId = appUserId, Email = "janusz@example.com", Phone = "222222222" },
                new EmployeeEntity { Name = "Tomasz", AppUserId = appUserId, Email = "tomasz@example.com", Phone = "333333333" }
            };

            _context.Employees.AddRange(employees);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetUserEmployeesAsync(appUserId, 1, 10, searchTerm);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Items.Count());
            Assert.AreEqual(employees[2].Name, result.Items.First().Name);
        }

        [TestMethod]
        public async Task GetUserEmployeesAsync_When_EmployeeesDoesNotExist_Returns_EmptyList()
        {
            // Arrange
            var appUserId = "user-id-123";
            var searchTerm = "nonexistent";

            var employees = new List<EmployeeEntity>
            {
                new EmployeeEntity { Name = "Grzegorz", AppUserId = appUserId, Email = "grzegorz@example.com", Phone = "111111111" },
                new EmployeeEntity { Name = "Janusz", AppUserId = appUserId, Email = "janusz@example.com", Phone = "222222222" },
                new EmployeeEntity { Name = "Tomasz", AppUserId = appUserId, Email = "tomasz@example.com", Phone = "333333333" }
            };

            _context.Employees.AddRange(employees);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetUserEmployeesAsync(appUserId, 1, 10, searchTerm);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Items.Count());
        }

        [TestMethod]
        public async Task GetUserNumberOfEmployeesAsync_Returns_TotalCount()
        {
            // Arrange
            var appUserId1 = "user-id-123";
            var appUserId2 = "user-id-1234";

            var employees = new List<EmployeeEntity>
            {
                new EmployeeEntity { Name = "Grzegorz", AppUserId = appUserId1, Email = "grzegorz@example.com", Phone = "111111111" },
                new EmployeeEntity { Name = "Janusz", AppUserId = appUserId1, Email = "janusz@example.com", Phone = "222222222" },
                new EmployeeEntity { Name = "Tomasz", AppUserId = appUserId2, Email = "tomasz@example.com", Phone = "333333333" }
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
                new EmployeeEntity { Name = "Grzegorz", AppUserId = appUserId, Email = "grzegorz@example.com", Phone = "111111111" },
                new EmployeeEntity { Name = "Janusz", AppUserId = appUserId, Email = "janusz@example.com", Phone = "222222222" },
                new EmployeeEntity { Name = "Tomasz", AppUserId = appUserId, Email = "tomasz@example.com", Phone = "333333333" }
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
            var appUserId = "user-id-123";

            var employees = new List<EmployeeEntity>
            {
                new EmployeeEntity { Name = "Grzegorz", AppUserId = appUserId, Email = "grzegorz@example.com", Phone = "111111111" },
                new EmployeeEntity { Name = "Janusz", AppUserId = appUserId, Email = "janusz@example.com", Phone = "222222222" },
                new EmployeeEntity { Name = "Tomasz", AppUserId = appUserId, Email = "tomasz@example.com", Phone = "333333333" }
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
            var appUserId = "user-id-123";

            var employees = new List<EmployeeEntity>
            {
                new EmployeeEntity { Name = "Grzegorz", AppUserId = appUserId, Email = "grzegorz@example.com", Phone = "111111111" },
                new EmployeeEntity { Name = "Janusz", AppUserId = appUserId, Email = "janusz@example.com", Phone = "222222222" },
                new EmployeeEntity { Name = "Tomasz", AppUserId = appUserId, Email = "tomasz@example.com", Phone = "333333333" }
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
            var appUserId = "user-id-123";
            var employee = new EmployeeEntity
            {
                Name = "Tomasz Wójcik",
                Email = "tomasz@example.com",
                Phone = "111222333",
                Salary = 4000,
                AppUserId = appUserId
            };

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            var updatedEmployee = new EmployeeEntity
            {
                Name = "Tomasz Nowy",
                Email = "nowy@example.com",
                Phone = "444555666",
                Salary = 6500,
                AppUserId = appUserId
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
        public async Task UpdateEmployeeAsync_When_EmployeeDoesNotExist_Returns_Entity()
        {
            // Arrange
            var nonExistentId = Guid.NewGuid();
            var updatedEmployee = new EmployeeEntity
            {
                Name = "Tomasz Nowy",
                Email = "nowy@example.com",
                Phone = "444555666",
                Salary = 6500,
                AppUserId = "user-id-123"
            };

            // Act
            var result = await _repository.UpdateEmployeeAsync(nonExistentId, updatedEmployee);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(updatedEmployee.Name, result.Name);
            Assert.AreEqual(updatedEmployee.Email, result.Email);
            Assert.AreEqual(updatedEmployee.Phone, result.Phone);
            Assert.AreEqual(updatedEmployee.Salary, result.Salary);
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

        [TestMethod]
        public async Task AddEmployeeListAsync_Returns_EmployeeList()
        {
            // Arrange
            var appUserId = "user-id-123";

            var employee1 = new EmployeeEntity { Name = "Janusz", AppUserId = appUserId, Email = "janusz@example.com", Phone = "222222222" };
            var employee2 = new EmployeeEntity { Name = "Tomasz", AppUserId = appUserId, Email = "tomasz@example.com", Phone = "333333333" };

            _context.Employees.AddRange(employee1, employee2);
            await _context.SaveChangesAsync();

            var employeeList = new EmployeeListsEntity
            {
                Name = "QA Team",
                AppUserId = appUserId
            };

            // Act
            var result = await _repository.AddEmployeeListsAsync(employeeList, new List<Guid> { employee1.Id, employee2.Id });

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(employeeList.Name, result.Name);
            Assert.AreNotEqual(Guid.Empty, result.Id);
            Assert.AreEqual(1, _context.EmployeeLists.Count());
        }

        [TestMethod]
        public async Task EmployeeListExistsAsync_When_ListExists_Returns_True()
        {
            // Arrange
            var appUserId = "user-id-123";
            var existingList = new EmployeeListsEntity
            {
                Id = Guid.NewGuid(),
                Name = "Dev Team",
                AppUserId = appUserId
            };

            _context.EmployeeLists.Add(existingList);
            await _context.SaveChangesAsync();

            var employeeList = new EmployeeListsEntity
            {
                Name = "Dev Team",
                AppUserId = appUserId
            };

            // Act
            var result = await _repository.EmployeeListExistsAsync(employeeList.Name, appUserId);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task EmployeeListExistsAsync_When_ListNoExists_Returns_False()
        {
            // Arrange
            var appUserId = "user-id-123";
            var existingList = new EmployeeListsEntity
            {
                Id = Guid.NewGuid(),
                Name = "Truck driver Team",
                AppUserId = appUserId
            };

            _context.EmployeeLists.Add(existingList);
            await _context.SaveChangesAsync();

            var employeeList = new EmployeeListsEntity
            {
                Name = "Dev Team",
                AppUserId = appUserId
            };

            // Act
            var result = await _repository.EmployeeListExistsAsync(employeeList.Name, appUserId);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task GetUserEmployeeListsAsync_BySearchTerm_Returns_EmployeeList()
        {
            // Arrange
            var appUserId = "user-id-123";
            var searchTerm = "dev";

            var employeeList = new List<EmployeeListsEntity> {
                new EmployeeListsEntity { Name = "Dev Team", AppUserId = appUserId },
                new EmployeeListsEntity { Name = "QA Team", AppUserId = appUserId },
                new EmployeeListsEntity { Name = "Other", AppUserId = "user-id-999" }
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

            var employeeList = new List<EmployeeListsEntity> {
                new EmployeeListsEntity { Name = "Dev Team", AppUserId = appUserId },
                new EmployeeListsEntity { Name = "QA Team", AppUserId = appUserId },
                new EmployeeListsEntity { Name = "Other", AppUserId = "user-id-999" }
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

            var employeeList = new List<EmployeeListsEntity> {
                new EmployeeListsEntity { Name = "No Task", AppUserId = appUserId, TaskId = null },
                new EmployeeListsEntity { Name = "Bulding a house", AppUserId = appUserId, TaskId = null },
                new EmployeeListsEntity { Name = "With Task", AppUserId = appUserId, TaskId = Guid.NewGuid() },
                new EmployeeListsEntity { Name = "Buy a car", AppUserId = "user-id-1234", TaskId = Guid.NewGuid() },
            };

            _context.EmployeeLists.AddRange(employeeList);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetUserEmployeeListsForTaskAsync(appUserId, "");

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
            var searchTerm = "team";

            var employeeList = new List<EmployeeListsEntity> {
                new EmployeeListsEntity { Name = "Dev Team", AppUserId = appUserId, TaskId = null },
                new EmployeeListsEntity { Name = "QA Team", AppUserId = appUserId, TaskId = null},
                new EmployeeListsEntity { Name = "Truck drivers", AppUserId = appUserId, TaskId = null },
                new EmployeeListsEntity { Name = "Other", AppUserId = appUserId, TaskId = null }
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

            var employeeList = new List<EmployeeListsEntity> {
                new EmployeeListsEntity { Name = "Dev Team", AppUserId = appUserId },
                new EmployeeListsEntity { Name = "QA Team", AppUserId = appUserId },
                new EmployeeListsEntity { Name = "Truck drivers", AppUserId = appUserId },
                new EmployeeListsEntity { Name = "Other", AppUserId = "user-id-999" }
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
        public async Task GetUserEmployeesForListAsync_Returns_OnlyEmployeesWithoutList()
        {
            // Arrange
            var appUserId = "user-id-123";
            var listId = Guid.NewGuid();

            var employees = new List<EmployeeEntity>
            {
                new EmployeeEntity { Name = "Alice", AppUserId = appUserId, Email = "alice@example.com", Phone = "222222222", EmployeeListId = null },
                new EmployeeEntity { Name = "John", AppUserId = appUserId, Email = "john@example.com", Phone = "333333333", EmployeeListId = null },
                new EmployeeEntity { Name = "Tom", AppUserId = appUserId, Email = "tom@example.com", Phone = "444444444", EmployeeListId = null },
                new EmployeeEntity { Name = "Bob", AppUserId = appUserId, Email = "bob@example.com", Phone = "555555555", EmployeeListId = listId },
                new EmployeeEntity { Name = "Bob", AppUserId = "user-id-999", Email = "bob@example.com", Phone = "555555555", EmployeeListId =  null  }
            };

            _context.Employees.AddRange(employees);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetUserEmployeesForListAsync(appUserId, "");

            // Assert
            Assert.AreEqual(3, result.Count());
            Assert.AreEqual(employees[0].Name, result.First().Name);
        }

        [TestMethod]
        public async Task GetUserEmployeesForListAsync_BySearchTerm_Returns_Employees()
        {
            // Arrange
            var appUserId = "user-id-123";
            var listId = Guid.NewGuid();
            var searchTerm = "John";

            var employees = new List<EmployeeEntity>
            {
                new EmployeeEntity { Name = "John", AppUserId = appUserId, Email = "john@example.com", Phone = "333333333", EmployeeListId = null },
                new EmployeeEntity { Name = "Alice", AppUserId = appUserId, Email = "alice@example.com", Phone = "222222222", EmployeeListId = null },
                new EmployeeEntity { Name = "John1", AppUserId = appUserId, Email = "john@example.com", Phone = "333333333", EmployeeListId = null },
                new EmployeeEntity { Name = "Tom", AppUserId = appUserId, Email = "tom@example.com", Phone = "444444444", EmployeeListId = null },
                new EmployeeEntity { Name = "John2", AppUserId = appUserId, Email = "john2@example.com", Phone = "555555555", EmployeeListId = listId },
                new EmployeeEntity { Name = "Bob", AppUserId = "user-id-999", Email = "bob@example.com", Phone = "555555555", EmployeeListId =  null  }
            };

            _context.Employees.AddRange(employees);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetUserEmployeesForListAsync(appUserId, searchTerm);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual(employees[0].Name, result.First().Name);       
        }

        [TestMethod]
        public async Task GetUserEmployeesForListAsync_When_EmployeeesDoesNotExist_Returns_EmptyList()
        {
            // Arrange
            var appUserId = "user-id-123";
            var listId = Guid.NewGuid();
            var searchTerm = "nonexistent";

            var employees = new List<EmployeeEntity>
            {
                new EmployeeEntity { Name = "John", AppUserId = appUserId, Email = "john@example.com", Phone = "333333333", EmployeeListId = null },
                new EmployeeEntity { Name = "Alice", AppUserId = appUserId, Email = "alice@example.com", Phone = "222222222", EmployeeListId = null },
                new EmployeeEntity { Name = "John1", AppUserId = appUserId, Email = "john@example.com", Phone = "333333333", EmployeeListId = null },
                new EmployeeEntity { Name = "Tom", AppUserId = appUserId, Email = "tom@example.com", Phone = "444444444", EmployeeListId = null },
                new EmployeeEntity { Name = "John2", AppUserId = appUserId, Email = "john2@example.com", Phone = "555555555", EmployeeListId = listId },
                new EmployeeEntity { Name = "Bob", AppUserId = "user-id-999", Email = "bob@example.com", Phone = "555555555", EmployeeListId =  null  }
            };

            _context.Employees.AddRange(employees);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetUserEmployeesForListAsync(appUserId, searchTerm);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public async Task DeleteEmployeeListsAsync_When_EmployeeLists_Returns_True()
        {
            // Arrange
            var appUserId = "user-id-123";

            var employeeList = new List<EmployeeListsEntity> {
                new EmployeeListsEntity { Name = "Dev Team", AppUserId = appUserId,  },
                new EmployeeListsEntity { Name = "QA Team", AppUserId = appUserId },
                new EmployeeListsEntity { Name = "Truck drivers", AppUserId = appUserId },
            };

            var employees = new List<EmployeeEntity>
            {
                new EmployeeEntity { Name = "John", AppUserId = appUserId, Email = "john@example.com", Phone = "333333333", EmployeeListId = employeeList[0].Id },
                new EmployeeEntity { Name = "Alice", AppUserId = appUserId, Email = "alice@example.com", Phone = "222222222", EmployeeListId = employeeList[0].Id },
                new EmployeeEntity { Name = "John1", AppUserId = appUserId, Email = "john@example.com", Phone = "333333333", EmployeeListId = employeeList[1].Id },
                new EmployeeEntity { Name = "Tom", AppUserId = appUserId, Email = "tom@example.com", Phone = "444444444", EmployeeListId = employeeList[2].Id },
            };

            _context.EmployeeLists.AddRange(employeeList);
            _context.Employees.AddRange(employees);
            await _context.SaveChangesAsync();

            var emplyeeListCountBefore = _context.EmployeeLists.Count();

            // Act
            var result = await _repository.DeleteEmployeeListsAsync(employeeList[0].Id);

            var emplyeeListCountAtfer = _context.EmployeeLists.Count();

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(emplyeeListCountBefore - 1, emplyeeListCountAtfer);
            Assert.AreEqual(2, _context.EmployeeLists.Count());
            Assert.IsNull(_context.EmployeeLists.FirstOrDefault(x => x.Id == employeeList[0].Id));
            Assert.IsTrue(employees.All(x => x.EmployeeListId != employeeList[0].Id));
        }

        [TestMethod]
        public async Task DeleteEmployeeListsAsync_When_EmployeeListsDoesNotExist__Returns_False()
        {
            // Act
            var result = await _repository.DeleteEmployeeListsAsync(Guid.NewGuid());

            // Assert
            Assert.IsFalse(result);
        }
    }
}