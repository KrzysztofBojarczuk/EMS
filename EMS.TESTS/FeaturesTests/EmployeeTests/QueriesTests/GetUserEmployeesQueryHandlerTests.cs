﻿using EMS.APPLICATION.Features.Employee.Queries;
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
        public async Task Handle_Returns_BySearchTerm_Employees()
        {
            // Arrange
            var appUserId = "user-id-123";
            var pageNumber = 1;
            var pageSize = 10;
            var searchTerm = "John";

            var expectedEmployees = new List<EmployeeEntity>
            {
                new EmployeeEntity { Name = "John", Email = "john@example.com", Phone = "123", AppUserId = appUserId },
                new EmployeeEntity { Name = "Johnny", Email = "johnny@example.com", Phone = "456", AppUserId = appUserId }
            };

            var paginatedList = new PaginatedList<EmployeeEntity>(expectedEmployees, expectedEmployees.Count, pageNumber, pageSize);

            _mockEmployeeRepository.Setup(x => x.GetUserEmployeesAsync(appUserId, pageNumber, pageSize, searchTerm))
                .ReturnsAsync(paginatedList);

            var query = new GetUserEmployeesQuery(appUserId, pageNumber, pageSize, searchTerm);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedEmployees.Count(), result.Items.Count());
            CollectionAssert.AreEqual(expectedEmployees, result.Items.ToList());
            _mockEmployeeRepository.Verify(x => x.GetUserEmployeesAsync(appUserId, pageNumber, pageSize, searchTerm), Times.Once);
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

            _mockEmployeeRepository.Setup(x => x.GetUserEmployeesAsync(appUserId, pageNumber, pageSize, searchTerm))
                .ReturnsAsync(paginatedList);

            var query = new GetUserEmployeesQuery(appUserId, pageNumber, pageSize, searchTerm);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Items.Count);
            _mockEmployeeRepository.Verify(x => x.GetUserEmployeesAsync(appUserId, pageNumber, pageSize, searchTerm), Times.Once);
        }
    }
}