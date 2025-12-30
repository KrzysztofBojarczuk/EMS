using EMS.APPLICATION.Features.Employee.Queries;
using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using EMS.INFRASTRUCTURE.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EMS.TESTS.FeaturesTests.EmployeeTests.QueriesTests
{
    [TestClass]
    public class GetUserEmployeesQueryHandlerTests
    {
        private Mock<IEmployeeRepository> _mockEmployeeRepository;
        private GetUserEmployeesQueryHandler _handler;

        public GetUserEmployeesQueryHandlerTests()
        {
            _mockEmployeeRepository = new Mock<IEmployeeRepository>();
            _handler = new GetUserEmployeesQueryHandler(_mockEmployeeRepository.Object);
        }

        [TestMethod]
        public async Task Handle_Returns_AllEmployees()
        {
            // Arrange
            var appUserId = "user-id-123";
            var pageNumber = 1;
            var pageSize = 10;

            var expectedEmployees = new List<EmployeeEntity>
            {
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "John", Email = "john@example.com", Phone = "123-456-789", Salary = 1000, DateOfBirth = new DateTime(1990, 1, 1), EmploymentDate = new DateTime(2021, 1, 1), MedicalCheckValidUntil = new DateTime(2021, 1, 1), AppUserId = appUserId },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Bob", Email = "bob@example.com", Phone = "123-456-789", Salary = 2000, DateOfBirth = new DateTime(1991, 1, 1), EmploymentDate = new DateTime(2022, 1, 1), MedicalCheckValidUntil = new DateTime(2022, 1, 1), AppUserId = appUserId },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Tom", Email = "tom@example.com", Phone = "123-456-789", Salary = 3000, DateOfBirth = new DateTime(1992, 1, 1), EmploymentDate = new DateTime(2023, 1, 1), MedicalCheckValidUntil = new DateTime(2023, 1, 1), AppUserId = appUserId },
            };

            var paginatedList = new PaginatedList<EmployeeEntity>(expectedEmployees, expectedEmployees.Count(), pageNumber, pageSize);

            _mockEmployeeRepository.Setup(x => x.GetUserEmployeesAsync(appUserId, pageNumber, pageSize, null, null))
                .ReturnsAsync(paginatedList);

            var query = new GetUserEmployeesQuery(appUserId, pageNumber, pageSize, null, null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedEmployees.Count(), result.Items.Count());
            CollectionAssert.AreEqual(expectedEmployees, result.Items.ToList());
            _mockEmployeeRepository.Verify(x => x.GetUserEmployeesAsync(appUserId, pageNumber, pageSize, null, null), Times.Once);
        }

        [TestMethod]
        public async Task Handle_Returns_BySearchTerm_Employees()
        {
            // Arrange
            var appUserId = "user-id-123";
            var pageNumber = 1;
            var pageSize = 10;
            var searchTerm = "John";

            var expectedEmployees = new List<EmployeeEntity>
            {
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "John", Email = "john@example.com", Phone = "123-456-789", Salary = 5000, DateOfBirth = new DateTime(1990, 1, 1), EmploymentDate = new DateTime(2021, 1, 1), MedicalCheckValidUntil = new DateTime(2021, 1, 1), AppUserId = appUserId },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Bob", Email = "bob@example.com", Phone = "123-456-789", Salary = 5000, DateOfBirth = new DateTime(1991, 1, 1), EmploymentDate = new DateTime(2022, 1, 1), MedicalCheckValidUntil = new DateTime(2022, 1, 1), AppUserId = appUserId }
            };

            var paginatedList = new PaginatedList<EmployeeEntity>(expectedEmployees, expectedEmployees.Count(), pageNumber, pageSize);

            _mockEmployeeRepository.Setup(x => x.GetUserEmployeesAsync(appUserId, pageNumber, pageSize, searchTerm, null))
                .ReturnsAsync(paginatedList);

            var query = new GetUserEmployeesQuery(appUserId, pageNumber, pageSize, searchTerm, null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedEmployees.Count(), result.Items.Count());
            CollectionAssert.AreEqual(expectedEmployees, result.Items.ToList());
            _mockEmployeeRepository.Verify(x => x.GetUserEmployeesAsync(appUserId, pageNumber, pageSize, searchTerm, null), Times.Once);
        }

        [TestMethod]
        public async Task Handle_Returns_EmptyList_When_Employees_NotFound()
        {
            // Arrange
            var appUserId = "user-id-123";
            var pageNumber = 1;
            var pageSize = 10;
            var searchTerm = "nonexistent";

            var paginatedList = new PaginatedList<EmployeeEntity>(new List<EmployeeEntity>(), 0, pageNumber, pageSize);

            _mockEmployeeRepository.Setup(x => x.GetUserEmployeesAsync(appUserId, pageNumber, pageSize, searchTerm, null))
                .ReturnsAsync(paginatedList);

            var query = new GetUserEmployeesQuery(appUserId, pageNumber, pageSize, searchTerm, null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Items.Count());
            _mockEmployeeRepository.Verify(x => x.GetUserEmployeesAsync(appUserId, pageNumber, pageSize, searchTerm, null), Times.Once);
        }

        [TestMethod]
        public async Task Handle_Returns_SortedBySalaryAscending_Employees()
        {
            // Arrange
            var appUserId = "user-id-123";
            var pageNumber = 1;
            var pageSize = 10;
            var sortOrder = "salary_asc";

            var expectedEmployees = new List<EmployeeEntity>
            {
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "John", Email = "john@example.com", Phone = "123-456-789", Salary = 1000, DateOfBirth = new DateTime(1990, 1, 1), EmploymentDate = new DateTime(2021, 1, 1), MedicalCheckValidUntil = new DateTime(2021, 1, 1), AppUserId = appUserId },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Bob", Email = "bob@example.com", Phone = "123-456-789", Salary = 2000, DateOfBirth = new DateTime(1991, 1, 1), EmploymentDate = new DateTime(2022, 1, 1), MedicalCheckValidUntil = new DateTime(2022, 1, 1), AppUserId = appUserId },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Tom", Email = "tom@example.com", Phone = "123-456-789", Salary = 3000, DateOfBirth = new DateTime(1992, 1, 1), EmploymentDate = new DateTime(2023, 1, 1), MedicalCheckValidUntil = new DateTime(2023, 1, 1), AppUserId = appUserId },
            };

            var paginatedList = new PaginatedList<EmployeeEntity>(expectedEmployees, expectedEmployees.Count(), pageNumber, pageSize);

            _mockEmployeeRepository.Setup(x => x.GetUserEmployeesAsync(appUserId, pageNumber, pageSize, null, sortOrder))
                .ReturnsAsync(paginatedList);

            var query = new GetUserEmployeesQuery(appUserId, pageNumber, pageSize, null, sortOrder);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedEmployees.Count(), result.Items.Count());
            CollectionAssert.AreEqual(expectedEmployees, result.Items.ToList());
            _mockEmployeeRepository.Verify(x => x.GetUserEmployeesAsync(appUserId, pageNumber, pageSize, null, sortOrder), Times.Once);
        }

        [TestMethod]
        public async Task Handle_Returns_SortedBySalaryDescending_Employees()
        {
            // Arrange
            var appUserId = "user-id-123";
            var pageNumber = 1;
            var pageSize = 10;
            var sortOrder = "salary_desc";

            var expectedEmployees = new List<EmployeeEntity>
            {
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "John", Email = "john@example.com", Phone = "123-456-789", Salary = 3000, DateOfBirth = new DateTime(1990, 1, 1), EmploymentDate = new DateTime(2021, 1, 1), MedicalCheckValidUntil = new DateTime(2021, 1, 1), AppUserId = appUserId },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Bob", Email = "bob@example.com", Phone = "123-456-789", Salary = 2000, DateOfBirth = new DateTime(1991, 1, 1), EmploymentDate = new DateTime(2022, 1, 1), MedicalCheckValidUntil = new DateTime(2022, 1, 1), AppUserId = appUserId },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Tom", Email = "tom@example.com", Phone = "123-456-789", Salary = 1000, DateOfBirth = new DateTime(1992, 1, 1), EmploymentDate = new DateTime(2023, 1, 1), MedicalCheckValidUntil = new DateTime(2023, 1, 1), AppUserId = appUserId },
            };

            var paginatedList = new PaginatedList<EmployeeEntity>(expectedEmployees, expectedEmployees.Count(), pageNumber, pageSize);

            _mockEmployeeRepository.Setup(x => x.GetUserEmployeesAsync(appUserId, pageNumber, pageSize, null, sortOrder))
                .ReturnsAsync(paginatedList);

            var query = new GetUserEmployeesQuery(appUserId, pageNumber, pageSize, null, sortOrder);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedEmployees.Count(), result.Items.Count());
            CollectionAssert.AreEqual(expectedEmployees, result.Items.ToList());
            _mockEmployeeRepository.Verify(x => x.GetUserEmployeesAsync(appUserId, pageNumber, pageSize, null, sortOrder), Times.Once);
        }

        [TestMethod]
        public async Task Handle_Returns_SortedByDateOfBirthAscending_Employees()
        {
            // Arrange
            var appUserId = "user-id-123";
            var pageNumber = 1;
            var pageSize = 10;
            var sortOrder = "birthDate_asc";

            var expectedEmployees = new List<EmployeeEntity>
            {
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "John", Email = "john@example.com", Phone = "123-456-789", Salary = 3000, DateOfBirth = new DateTime(1990, 1, 1), EmploymentDate = new DateTime(2021, 1, 1), MedicalCheckValidUntil = new DateTime(2021, 1, 1), AppUserId = appUserId },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Bob", Email = "bob@example.com", Phone = "123-456-789", Salary = 2000, DateOfBirth = new DateTime(1991, 1, 1), EmploymentDate = new DateTime(2022, 1, 1), MedicalCheckValidUntil = new DateTime(2022, 1, 1), AppUserId = appUserId },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Tom", Email = "tom@example.com", Phone = "123-456-789", Salary = 1000, DateOfBirth = new DateTime(1992, 1, 1), EmploymentDate = new DateTime(2023, 1, 1), MedicalCheckValidUntil = new DateTime(2023, 1, 1), AppUserId = appUserId },
            };

            var paginatedList = new PaginatedList<EmployeeEntity>(expectedEmployees, expectedEmployees.Count(), pageNumber, pageSize);

            _mockEmployeeRepository.Setup(x => x.GetUserEmployeesAsync(appUserId, pageNumber, pageSize, null, sortOrder))
                .ReturnsAsync(paginatedList);

            var query = new GetUserEmployeesQuery(appUserId, pageNumber, pageSize, null, sortOrder);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedEmployees.Count(), result.Items.Count());
            CollectionAssert.AreEqual(expectedEmployees, result.Items.ToList());
            _mockEmployeeRepository.Verify(x => x.GetUserEmployeesAsync(appUserId, pageNumber, pageSize, null, sortOrder), Times.Once);
        }

        [TestMethod]
        public async Task Handle_Returns_SortedByDateOfBirthDescending_Employees()
        {
            // Arrange
            var appUserId = "user-id-123";
            var pageNumber = 1;
            var pageSize = 10;
            var sortOrder = "birthDate_desc";

            var expectedEmployees = new List<EmployeeEntity>
            {
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "John", Email = "John@example.com", Phone = "123-456-789", Salary = 3000, DateOfBirth = new DateTime(1992, 1, 1), EmploymentDate = new DateTime(2021, 1, 2), MedicalCheckValidUntil = new DateTime(2021, 1, 1), AppUserId = appUserId },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Bob", Email = "bob@example.com", Phone = "123-456-789", Salary = 2000, DateOfBirth = new DateTime(1991, 1, 1), EmploymentDate = new DateTime(2022, 1, 1), MedicalCheckValidUntil = new DateTime(2022, 1, 1), AppUserId = appUserId },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Tom", Email = "tom@example.com", Phone = "123-456-789", Salary = 1000, DateOfBirth = new DateTime(1990, 1, 1), EmploymentDate = new DateTime(2023, 1, 1), MedicalCheckValidUntil = new DateTime(2023, 1, 1), AppUserId = appUserId },
            };

            var paginatedList = new PaginatedList<EmployeeEntity>(expectedEmployees, expectedEmployees.Count(), pageNumber, pageSize);

            _mockEmployeeRepository.Setup(x => x.GetUserEmployeesAsync(appUserId, pageNumber, pageSize, null, sortOrder))
                .ReturnsAsync(paginatedList);

            var query = new GetUserEmployeesQuery(appUserId, pageNumber, pageSize, null, sortOrder);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedEmployees.Count(), result.Items.Count());
            CollectionAssert.AreEqual(expectedEmployees, result.Items.ToList());
            _mockEmployeeRepository.Verify(x => x.GetUserEmployeesAsync(appUserId, pageNumber, pageSize, null, sortOrder), Times.Once);
        }

        [TestMethod]
        public async Task Handle_Returns_SortedByEmploymentDateAscending_Employees()
        {
            // Arrange
            var appUserId = "user-id-123";
            var pageNumber = 1;
            var pageSize = 10;
            var sortOrder = "employmentDate_asc";

            var expectedEmployees = new List<EmployeeEntity>
            {
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "John", Email = "John@example.com", Phone = "123-456-789", Salary = 3000, DateOfBirth = new DateTime(1992, 1, 1), EmploymentDate = new DateTime(2021, 1, 1), MedicalCheckValidUntil = new DateTime(2021, 1, 1), AppUserId = appUserId },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Bob", Email = "bob@example.com", Phone = "123-456-789", Salary = 2000, DateOfBirth = new DateTime(1991, 1, 1), EmploymentDate = new DateTime(2022, 1, 1), MedicalCheckValidUntil = new DateTime(2022, 1, 1), AppUserId = appUserId },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Tom", Email = "tom@example.com", Phone = "123-456-789", Salary = 1000, DateOfBirth = new DateTime(1990, 1, 1), EmploymentDate = new DateTime(2023, 1, 1), MedicalCheckValidUntil = new DateTime(2023, 1, 1), AppUserId = appUserId },
            };

            var paginatedList = new PaginatedList<EmployeeEntity>(expectedEmployees, expectedEmployees.Count(), pageNumber, pageSize);

            _mockEmployeeRepository.Setup(x => x.GetUserEmployeesAsync(appUserId, pageNumber, pageSize, null, sortOrder))
                .ReturnsAsync(paginatedList);

            var query = new GetUserEmployeesQuery(appUserId, pageNumber, pageSize, null, sortOrder);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedEmployees.Count(), result.Items.Count());
            CollectionAssert.AreEqual(expectedEmployees, result.Items.ToList());
            _mockEmployeeRepository.Verify(x => x.GetUserEmployeesAsync(appUserId, pageNumber, pageSize, null, sortOrder), Times.Once);
        }

        [TestMethod]
        public async Task Handle_Returns_SortedByEmploymentDateDescending_Employees()
        {
            // Arrange
            var appUserId = "user-id-123";
            var pageNumber = 1;
            var pageSize = 10;
            var sortOrder = "employmentDate_desc";

            var expectedEmployees = new List<EmployeeEntity>
            {
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "John", Email = "John@example.com", Phone = "123-456-789", Salary = 3000, DateOfBirth = new DateTime(1992, 1, 1), EmploymentDate = new DateTime(2023, 1, 1), MedicalCheckValidUntil = new DateTime(2021, 1, 1), AppUserId = appUserId },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Bob", Email = "bob@example.com", Phone = "123-456-789", Salary = 2000, DateOfBirth = new DateTime(1991, 1, 1), EmploymentDate = new DateTime(2022, 1, 1), MedicalCheckValidUntil = new DateTime(2022, 1, 1), AppUserId = appUserId },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Tom", Email = "tom@example.com", Phone = "123-456-789", Salary = 1000, DateOfBirth = new DateTime(1990, 1, 1), EmploymentDate = new DateTime(2021, 1, 1), MedicalCheckValidUntil = new DateTime(2023, 1, 1), AppUserId = appUserId },
            };

            var paginatedList = new PaginatedList<EmployeeEntity>(expectedEmployees, expectedEmployees.Count(), pageNumber, pageSize);

            _mockEmployeeRepository.Setup(x => x.GetUserEmployeesAsync(appUserId, pageNumber, pageSize, null, sortOrder))
                .ReturnsAsync(paginatedList);

            var query = new GetUserEmployeesQuery(appUserId, pageNumber, pageSize, null, sortOrder);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedEmployees.Count(), result.Items.Count());
            CollectionAssert.AreEqual(expectedEmployees, result.Items.ToList());
            _mockEmployeeRepository.Verify(x => x.GetUserEmployeesAsync(appUserId, pageNumber, pageSize, null, sortOrder), Times.Once);
        }

        [TestMethod]
        public async Task Handle_Returns_SortedByMedicalCheckValidUntilAscending_Employees()
        {
            // Arrange
            var appUserId = "user-id-123";
            var pageNumber = 1;
            var pageSize = 10;
            var sortOrder = "medicalCheckValidUntil_asc";

            var expectedEmployees = new List<EmployeeEntity>
            {
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "John", Email = "John@example.com", Phone = "123-456-789", Salary = 3000, DateOfBirth = new DateTime(1992, 1, 1), EmploymentDate = new DateTime(2023, 1, 1), MedicalCheckValidUntil = new DateTime(2021, 1, 1), AppUserId = appUserId },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Bob", Email = "bob@example.com", Phone = "123-456-789", Salary = 2000, DateOfBirth = new DateTime(1991, 1, 1), EmploymentDate = new DateTime(2022, 1, 1), MedicalCheckValidUntil = new DateTime(2022, 1, 1), AppUserId = appUserId },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Tom", Email = "tom@example.com", Phone = "123-456-789", Salary = 1000, DateOfBirth = new DateTime(1990, 1, 1), EmploymentDate = new DateTime(2021, 1, 1), MedicalCheckValidUntil = new DateTime(2023, 1, 1), AppUserId = appUserId },
            };

            var paginatedList = new PaginatedList<EmployeeEntity>(expectedEmployees, expectedEmployees.Count(), pageNumber, pageSize);

            _mockEmployeeRepository.Setup(x => x.GetUserEmployeesAsync(appUserId, pageNumber, pageSize, null, sortOrder))
                .ReturnsAsync(paginatedList);

            var query = new GetUserEmployeesQuery(appUserId, pageNumber, pageSize, null, sortOrder);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedEmployees.Count(), result.Items.Count());
            CollectionAssert.AreEqual(expectedEmployees, result.Items.ToList());
            _mockEmployeeRepository.Verify(x => x.GetUserEmployeesAsync(appUserId, pageNumber, pageSize, null, sortOrder), Times.Once);
        }

        [TestMethod]
        public async Task Handle_Returns_Employees_When_SortOrderDoesNotExist()
        {
            // Arrange
            var appUserId = "user-id-123";
            var pageNumber = 1;
            var pageSize = 10;
            var sortOrder = "nonexistent";

            var expectedEmployees = new List<EmployeeEntity>
            {
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "John", Email = "john@example.com", Phone = "123-456-789", Salary = 2000, DateOfBirth = new DateTime(1990, 1, 1), EmploymentDate = new DateTime(2021, 1, 1), MedicalCheckValidUntil = new DateTime(2021, 1, 1), AppUserId = appUserId },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Bob", Email = "Bob@example.com", Phone = "123-456-789", Salary = 2000,  DateOfBirth = new DateTime(1991, 1, 1), EmploymentDate = new DateTime(2022, 1, 1), MedicalCheckValidUntil = new DateTime(2022, 1, 1), AppUserId = appUserId },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Tom", Email = "tom@example.com", Phone = "123-456-789", Salary = 1000, DateOfBirth = new DateTime(1992, 1, 1), EmploymentDate = new DateTime(2023, 1, 1), MedicalCheckValidUntil = new DateTime(2023, 1, 1), AppUserId = appUserId },
            };

            var paginatedList = new PaginatedList<EmployeeEntity>(expectedEmployees, expectedEmployees.Count(), pageNumber, pageSize);

            _mockEmployeeRepository.Setup(x => x.GetUserEmployeesAsync(appUserId, pageNumber, pageSize, null, sortOrder))
                .ReturnsAsync(paginatedList);

            var query = new GetUserEmployeesQuery(appUserId, pageNumber, pageSize, null, sortOrder);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedEmployees.Count(), result.Items.Count());
            CollectionAssert.AreEqual(expectedEmployees, result.Items.ToList());
            _mockEmployeeRepository.Verify(x => x.GetUserEmployeesAsync(appUserId, pageNumber, pageSize, null, sortOrder), Times.Once);
        }
    }
}