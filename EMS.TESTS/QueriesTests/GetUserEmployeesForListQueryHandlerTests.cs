using EMS.APPLICATION.Features.Employee.Queries;
using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace EMS.TESTS.QueriesTests
{
    [TestClass]
    public class GetUserEmployeesForListQueryHandlerTests
    {
        private readonly Mock<IEmployeeRepository> _mockEmployeeRepository;
        private readonly GetUserEmployeesForListQueryHandler _handler;

        public GetUserEmployeesForListQueryHandlerTests()
        {
            _mockEmployeeRepository = new Mock<IEmployeeRepository>();
            _handler = new GetUserEmployeesForListQueryHandler(_mockEmployeeRepository.Object);
        }

        [TestMethod]
        public async Task Handle_ReturnsEmployees()
        {
            // Arrange
            var appUserId = "user1";
            var searchTerm = "John";

            var employees = new List<EmployeeEntity>
            {
                new EmployeeEntity { Name = "John", Email = "john@example.com", Phone = "123", AppUserId = appUserId },
                new EmployeeEntity { Name = "Jane", Email = "jane@example.com", Phone = "456", AppUserId = appUserId }
             };

            _mockEmployeeRepository.Setup(repo => repo.GetUserEmployeesForListAsync(appUserId, searchTerm))
                .ReturnsAsync(employees.Where(e => e.Name.Contains(searchTerm)).ToList());

            var query = new GetUserEmployeesForListQuery(appUserId, searchTerm);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count()); 
            Assert.AreEqual(searchTerm, result.First().Name);
        }

        [TestMethod]
        public async Task Handle_ReturnsEmptyListBySearchTerm()
        {
            // Arrange
            var appUserId = "user1";
            var searchTerm = "NonExistentName";

            var employees = new List<EmployeeEntity>
            {
               new EmployeeEntity { Name = "John", Email = "john@example.com", Phone = "123", AppUserId = appUserId },
               new EmployeeEntity { Name = "Jane", Email = "jane@example.com", Phone = "456", AppUserId = appUserId }
            };

            _mockEmployeeRepository.Setup(repo => repo.GetUserEmployeesForListAsync(appUserId, searchTerm))
                .ReturnsAsync(employees.Where(e => e.Name.Contains(searchTerm)).ToList());

            var query = new GetUserEmployeesForListQuery(appUserId, searchTerm);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }
    }
}
