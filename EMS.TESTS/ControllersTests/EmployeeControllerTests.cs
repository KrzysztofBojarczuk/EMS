using AutoMapper;
using EMS.API.Controllers;
using EMS.APPLICATION.Dtos;
using EMS.APPLICATION.Features.Employee.Queries;
using EMS.CORE.Entities;
using EMS.INFRASTRUCTURE.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Security.Claims;

namespace EMS.TESTS.ControllersTests
{
    [TestClass]
    public class EmployeeControllerTests
    {
        private Mock<ISender> _senderMock;
        private Mock<UserManager<AppUserEntity>> _userManagerMock;
        private Mock<IMapper> _mapperMock;
        private EmployeeController _controller;

        [TestInitialize]
        public void Setup()
        {
            _senderMock = new Mock<ISender>();
            _mapperMock = new Mock<IMapper>();

            var store = new Mock<IUserStore<AppUserEntity>>();
            _userManagerMock = new Mock<UserManager<AppUserEntity>>(store.Object, null, null, null, null, null, null, null, null);

            _controller = new EmployeeController(_senderMock.Object, _userManagerMock.Object, _mapperMock.Object);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname", "testuser")
            }, "mock"));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }

        [TestMethod]
        public async Task GetUserEmployeesAsync_ReturnsOkResult_BySearchTerm_WithEmployeeDto()
        {
            // Arrange
            var appUserId = "user-id-123";
            var username = "testuser";
            var pageNumber = 1;
            var pageSize = 10;
            var searchTerm = "anna";

            var appUser = new AppUserEntity { Id = appUserId, UserName = username };

            var employeesEntities = new List<EmployeeEntity>
            {
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Anna1" },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Anna2" }
            };

            var paginatedEmployees = new PaginatedList<EmployeeEntity>(employeesEntities, employeesEntities.Count, pageNumber, pageSize);

            var expectedDtos = new List<EmployeeGetDto>
            {
                new EmployeeGetDto { Id = employeesEntities[0].Id, Name = "Anna1" },
                new EmployeeGetDto { Id = employeesEntities[1].Id, Name = "Anna2" }
            };

            _userManagerMock.Setup(x => x.FindByNameAsync(username))
                .ReturnsAsync(appUser);

            _senderMock.Setup(x => x.Send(
                It.Is<GetUserEmployeesQuery>(x =>
                    x.appUserId == appUserId &&
                    x.pageNumber == pageNumber &&
                    x.pageSize == pageSize &&
                    x.searchTerm == searchTerm),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(paginatedEmployees);

            _mapperMock.Setup(x => x.Map<IEnumerable<EmployeeGetDto>>(employeesEntities))
                .Returns(expectedDtos);

            // Act
            var result = await _controller.GetUserEmployeesAsync(pageNumber, pageSize, searchTerm);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var value = okResult.Value;
            var employeeGetProperty = value.GetType().GetProperty("EmployeeGet");
            var totalItemsProperty = value.GetType().GetProperty("TotalItems");
            var totalPagesProperty = value.GetType().GetProperty("TotalPages");
            var pageIndexProperty = value.GetType().GetProperty("PageIndex");

            Assert.IsNotNull(employeeGetProperty);
            var returnedDtos = employeeGetProperty.GetValue(value) as IEnumerable<EmployeeGetDto>;
            Assert.IsNotNull(returnedDtos);
            Assert.AreEqual(expectedDtos.Count(), returnedDtos.Count());

            Assert.AreEqual(paginatedEmployees.TotalItems, totalItemsProperty.GetValue(value));
            Assert.AreEqual(paginatedEmployees.TotalPages, totalPagesProperty.GetValue(value));
            Assert.AreEqual(paginatedEmployees.PageIndex, pageIndexProperty.GetValue(value));
        }
    }
}