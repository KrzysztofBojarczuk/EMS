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
        private Mock<IEmployeeRepository> _mockEmployeeRepository;
        private GetAllEmployeesQueryHandler _handler;

        public GetAllEmployeesQueryHandlerTests()
        {
            _mockEmployeeRepository = new Mock<IEmployeeRepository>();
            _handler = new GetAllEmployeesQueryHandler(_mockEmployeeRepository.Object);
        }

        [TestMethod]
        public async Task Handle_Returns_AllEmployees()
        {
            // Arrange
            var pageNumber = 1;
            var pageSize = 10;

            var expectedEmployees = new List<EmployeeEntity>
            {
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 1", Email = "employee1@example.com", Phone = "123-456-789", Salary = 5000, DateOfBirth = new DateTime(1990, 1, 1), EmploymentDate = new DateTime(2022, 1, 1), MedicalCheckValidUntil = new DateTime(2025, 1, 1) },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 2", Email = "employee2@example.com", Phone = "123-456-789", Salary = 5000, DateOfBirth = new DateTime(1991, 1, 1), EmploymentDate = new DateTime(2022, 2, 2), MedicalCheckValidUntil = new DateTime(2025, 2, 2) },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 3", Email = "employee3@example.com", Phone = "123-456-789", Salary = 5000, DateOfBirth = new DateTime(1992, 1, 1), EmploymentDate = new DateTime(2022, 3, 3), MedicalCheckValidUntil = new DateTime(2025, 3, 3) }
            };

            var paginatedList = new PaginatedList<EmployeeEntity>(expectedEmployees, expectedEmployees.Count(), pageNumber, pageSize);

            _mockEmployeeRepository.Setup(x => x.GetAllEmployeesAsync(pageNumber, pageSize, null, null))
                .ReturnsAsync(paginatedList);

            var query = new GetAllEmployeesQuery(pageNumber, pageSize, null, null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedEmployees.Count(), result.Items.Count());
            CollectionAssert.AreEqual(expectedEmployees, result.Items.ToList());
            _mockEmployeeRepository.Verify(x => x.GetAllEmployeesAsync(pageNumber, pageSize, null, null), Times.Once);
        }

        [TestMethod]
        public async Task Handle_Returns_BySearchTerm_Employees()
        {
            // Arrange
            var pageNumber = 1;
            var pageSize = 10;
            var searchTerm = "test";

            var expectedEmployees = new List<EmployeeEntity>
            {
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 1 Test", Email = "employee1@example.com", Phone = "123-456-789", Salary = 5000, DateOfBirth = new DateTime(1990, 1, 1), EmploymentDate = new DateTime(2022, 1, 1), MedicalCheckValidUntil = new DateTime(2025, 1, 1) },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 2 Test", Email = "employee2@example.com", Phone = "123-456-789", Salary = 5000, DateOfBirth = new DateTime(1990, 1, 1), EmploymentDate = new DateTime(2022, 2, 2), MedicalCheckValidUntil = new DateTime(2025, 2, 2) }
            };

            var paginatedList = new PaginatedList<EmployeeEntity>(expectedEmployees, expectedEmployees.Count(), pageNumber, pageSize);

            _mockEmployeeRepository.Setup(x => x.GetAllEmployeesAsync(pageNumber, pageSize, searchTerm, null))
                .ReturnsAsync(paginatedList);

            var query = new GetAllEmployeesQuery(pageNumber, pageSize, searchTerm, null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedEmployees.Count(), result.Items.Count());
            CollectionAssert.AreEqual(expectedEmployees, result.Items.ToList());
            _mockEmployeeRepository.Verify(x => x.GetAllEmployeesAsync(pageNumber, pageSize, searchTerm, null), Times.Once);
        }

        [TestMethod]
        public async Task Handle_Returns_EmptyList_When_Employees_NotFound()
        {
            // Arrange
            var pageNumber = 1;
            var pageSize = 10;
            var searchTerm = "nonexistent";

            var paginatedList = new PaginatedList<EmployeeEntity>(new List<EmployeeEntity>(), 0, pageNumber, pageSize);

            _mockEmployeeRepository.Setup(x => x.GetAllEmployeesAsync(pageNumber, pageSize, searchTerm, null))
                .ReturnsAsync(paginatedList);

            var query = new GetAllEmployeesQuery(pageNumber, pageSize, searchTerm, null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Items.Count());
            _mockEmployeeRepository.Verify(x => x.GetAllEmployeesAsync(pageNumber, pageSize, searchTerm, null), Times.Once);
        }

        [TestMethod]
        public async Task Handle_Returns_SortedBySalaryAscending_Employees()
        {
            // Arrange
            var pageNumber = 1;
            var pageSize = 10;
            var sortOrder = "salary_asc";

            var expectedEmployees = new List<EmployeeEntity>
            {
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 1", Email = "employee1@example.com", Phone = "123-456-789", Salary = 1000, DateOfBirth = new DateTime(1990, 1, 1), EmploymentDate = new DateTime(2021, 1, 1), MedicalCheckValidUntil = new DateTime(2021, 1, 1) },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 2", Email = "employee2@example.com", Phone = "123-456-789", Salary = 2000, DateOfBirth = new DateTime(1991, 1, 1), EmploymentDate = new DateTime(2022, 1, 1), MedicalCheckValidUntil = new DateTime(2022, 1, 1) },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 3", Email = "employee3@example.com", Phone = "123-456-789", Salary = 3000, DateOfBirth = new DateTime(1992, 1, 1), EmploymentDate = new DateTime(2023, 1, 1), MedicalCheckValidUntil = new DateTime(2023, 1, 1) }
            };

            var paginatedList = new PaginatedList<EmployeeEntity>(expectedEmployees, expectedEmployees.Count(), pageNumber, pageSize);

            _mockEmployeeRepository.Setup(x => x.GetAllEmployeesAsync(pageNumber, pageSize, null, sortOrder))
                .ReturnsAsync(paginatedList);

            var query = new GetAllEmployeesQuery(pageNumber, pageSize, null, sortOrder);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedEmployees.Count(), result.Items.Count());
            CollectionAssert.AreEqual(expectedEmployees, result.Items.ToList());
            _mockEmployeeRepository.Verify(x => x.GetAllEmployeesAsync(pageNumber, pageSize, null, sortOrder), Times.Once);
        }

        [TestMethod]
        public async Task Handle_Returns_SortedBySalaryDescending_Employees()
        {
            // Arrange
            var pageNumber = 1;
            var pageSize = 10;
            var sortOrder = "salary_desc";

            var expectedEmployees = new List<EmployeeEntity>
            {
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 1", Email = "employee1@example.com", Phone = "123-456-789", Salary = 3000, DateOfBirth = new DateTime(1990, 1, 1), EmploymentDate = new DateTime(2021, 1, 1), MedicalCheckValidUntil = new DateTime(2021, 1, 1) },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 2", Email = "employee2@example.com", Phone = "123-456-789", Salary = 2000, DateOfBirth = new DateTime(1991, 1, 1), EmploymentDate = new DateTime(2022, 1, 1), MedicalCheckValidUntil = new DateTime(2022, 1, 1) },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 3", Email = "employee3@example.com", Phone = "123-456-789", Salary = 1000, DateOfBirth = new DateTime(1992, 1, 1), EmploymentDate = new DateTime(2023, 1, 1), MedicalCheckValidUntil = new DateTime(2023, 1, 1) }
            };

            var paginatedList = new PaginatedList<EmployeeEntity>(expectedEmployees, expectedEmployees.Count(), pageNumber, pageSize);

            _mockEmployeeRepository.Setup(x => x.GetAllEmployeesAsync(pageNumber, pageSize, null, sortOrder))
                .ReturnsAsync(paginatedList);

            var query = new GetAllEmployeesQuery(pageNumber, pageSize, null, sortOrder);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedEmployees.Count(), result.Items.Count());
            CollectionAssert.AreEqual(expectedEmployees, result.Items.ToList());
            _mockEmployeeRepository.Verify(x => x.GetAllEmployeesAsync(pageNumber, pageSize, null, sortOrder), Times.Once);
        }

        [TestMethod]
        public async Task Handle_Returns_SortedByDateOfBirthAscending_Employees()
        {
            // Arrange
            var pageNumber = 1;
            var pageSize = 10;
            var sortOrder = "birthDate_asc";

            var expectedEmployees = new List<EmployeeEntity>
            {
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 1", Email = "employee1@example.com", Phone = "123-456-789", Salary = 3000, DateOfBirth = new DateTime(1990, 1, 1), EmploymentDate = new DateTime(2021, 1, 1), MedicalCheckValidUntil = new DateTime(2021, 1, 1) },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 2", Email = "employee2@example.com", Phone = "123-456-789", Salary = 2000, DateOfBirth = new DateTime(1991, 1, 1), EmploymentDate = new DateTime(2022, 1, 1), MedicalCheckValidUntil = new DateTime(2022, 1, 1) },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 3", Email = "employee3@example.com", Phone = "123-456-789", Salary = 1000, DateOfBirth = new DateTime(1992, 1, 1), EmploymentDate = new DateTime(2023, 1, 1), MedicalCheckValidUntil = new DateTime(2023, 1, 1) }
            };

            var paginatedList = new PaginatedList<EmployeeEntity>(expectedEmployees, expectedEmployees.Count(), pageNumber, pageSize);

            _mockEmployeeRepository.Setup(x => x.GetAllEmployeesAsync(pageNumber, pageSize, null, sortOrder))
                .ReturnsAsync(paginatedList);

            var query = new GetAllEmployeesQuery(pageNumber, pageSize, null, sortOrder);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedEmployees.Count(), result.Items.Count());
            CollectionAssert.AreEqual(expectedEmployees, result.Items.ToList());
            _mockEmployeeRepository.Verify(x => x.GetAllEmployeesAsync(pageNumber, pageSize, null, sortOrder), Times.Once);
        }

        [TestMethod]
        public async Task Handle_Returns_SortedByDateOfBirthDescending_Employees()
        {
            // Arrange
            var pageNumber = 1;
            var pageSize = 10;
            var sortOrder = "birthDate_desc";

            var expectedEmployees = new List<EmployeeEntity>
            {
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 1", Email = "employee1@example.com", Phone = "123-456-789", Salary = 3000, DateOfBirth = new DateTime(1992, 1, 1), EmploymentDate = new DateTime(2021, 1, 2), MedicalCheckValidUntil = new DateTime(2021, 1, 1) },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 2", Email = "employee2@example.com", Phone = "123-456-789", Salary = 2000, DateOfBirth = new DateTime(1991, 1, 1), EmploymentDate = new DateTime(2022, 1, 1), MedicalCheckValidUntil = new DateTime(2022, 1, 1) },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 3", Email = "employee3@example.com", Phone = "123-456-789", Salary = 1000, DateOfBirth = new DateTime(1990, 1, 1), EmploymentDate = new DateTime(2023, 1, 1), MedicalCheckValidUntil = new DateTime(2023, 1, 1) }
            };

            var paginatedList = new PaginatedList<EmployeeEntity>(expectedEmployees, expectedEmployees.Count(), pageNumber, pageSize);

            _mockEmployeeRepository.Setup(x => x.GetAllEmployeesAsync(pageNumber, pageSize, null, sortOrder))
                .ReturnsAsync(paginatedList);

            var query = new GetAllEmployeesQuery(pageNumber, pageSize, null, sortOrder);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedEmployees.Count(), result.Items.Count());
            CollectionAssert.AreEqual(expectedEmployees, result.Items.ToList());
            _mockEmployeeRepository.Verify(x => x.GetAllEmployeesAsync(pageNumber, pageSize, null, sortOrder), Times.Once);
        }

        [TestMethod]
        public async Task Handle_Returns_SortedByEmploymentDateAscending_Employees()
        {
            // Arrange
            var pageNumber = 1;
            var pageSize = 10;
            var sortOrder = "employmentDate_asc";

            var expectedEmployees = new List<EmployeeEntity>
            {
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 1", Email = "employee1@example.com", Phone = "123-456-789", Salary = 3000, DateOfBirth = new DateTime(1992, 1, 1), EmploymentDate = new DateTime(2021, 1, 1), MedicalCheckValidUntil = new DateTime(2021, 1, 1) },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 2", Email = "employee2@example.com", Phone = "123-456-789", Salary = 2000, DateOfBirth = new DateTime(1991, 1, 1), EmploymentDate = new DateTime(2022, 1, 1), MedicalCheckValidUntil = new DateTime(2022, 1, 1) },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 3", Email = "employee3@example.com", Phone = "123-456-789", Salary = 1000, DateOfBirth = new DateTime(1990, 1, 1), EmploymentDate = new DateTime(2023, 1, 1), MedicalCheckValidUntil = new DateTime(2023, 1, 1) }
            };

            var paginatedList = new PaginatedList<EmployeeEntity>(expectedEmployees, expectedEmployees.Count(), pageNumber, pageSize);

            _mockEmployeeRepository.Setup(x => x.GetAllEmployeesAsync(pageNumber, pageSize, null, sortOrder))
                .ReturnsAsync(paginatedList);

            var query = new GetAllEmployeesQuery(pageNumber, pageSize, null, sortOrder);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedEmployees.Count(), result.Items.Count());
            CollectionAssert.AreEqual(expectedEmployees, result.Items.ToList());
            _mockEmployeeRepository.Verify(x => x.GetAllEmployeesAsync(pageNumber, pageSize, null, sortOrder), Times.Once);
        }
    }
}