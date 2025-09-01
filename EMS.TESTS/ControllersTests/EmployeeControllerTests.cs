using AutoMapper;
using EMS.API.Controllers;
using EMS.APPLICATION.Dtos;
using EMS.APPLICATION.Features.Employee.Commands;
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
        public async Task GetUserEmployeesAsync_ReturnsOkResult_BySearchTerm_WithEmployeeDtos()
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

            var paginatedResult = new PaginatedList<EmployeeEntity>(employeesEntities, employeesEntities.Count, pageNumber, pageSize);

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
                .ReturnsAsync(paginatedResult);

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

            Assert.AreEqual(paginatedResult.TotalItems, totalItemsProperty.GetValue(value));
            Assert.AreEqual(paginatedResult.TotalPages, totalPagesProperty.GetValue(value));
            Assert.AreEqual(paginatedResult.PageIndex, pageIndexProperty.GetValue(value));
        }

        [TestMethod]
        public async Task GetUserEmployeesAsync_ReturnsOkResult_NotFound_WithEmptyList()
        {
            // Arrange
            var appUserId = "user-id-123";
            var username = "testuser";
            var pageNumber = 1;
            var pageSize = 10;
            var searchTerm = "nonexistent";

            var appUser = new AppUserEntity { Id = appUserId, UserName = username };

            var employeesEntities = new List<EmployeeEntity>();

            var paginatedResult = new PaginatedList<EmployeeEntity>(employeesEntities, 0, pageNumber, pageSize);

            var expectedDtos = new List<EmployeeGetDto>();

            _userManagerMock.Setup(x => x.FindByNameAsync(username))
               .ReturnsAsync(appUser);

            _senderMock.Setup(x => x.Send(
              It.Is<GetUserEmployeesQuery>(x =>
                  x.appUserId == appUserId &&
                  x.pageNumber == pageNumber &&
                  x.pageSize == pageSize &&
                  x.searchTerm == searchTerm),
              It.IsAny<CancellationToken>()))
              .ReturnsAsync(paginatedResult);

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

            Assert.AreEqual(paginatedResult.TotalItems, totalItemsProperty.GetValue(value));
            Assert.AreEqual(paginatedResult.TotalPages, totalPagesProperty.GetValue(value));
            Assert.AreEqual(paginatedResult.PageIndex, pageIndexProperty.GetValue(value));
        }

        [TestMethod]
        public async Task GetUserEmployeesForListAsync_ReturnsOk_BySearchTerm_WithEmployeeDtos()
        {
            // Arrange
            var appUserId = "user-id-123";
            var username = "testuser";
            var searchTerm = "anna";

            var appUser = new AppUserEntity { Id = appUserId, UserName = username };

            var employeesEntity = new List<EmployeeEntity>
            {
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Anna1" },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Anna2" }
            };

            var expectedDtos = new List<EmployeeGetDto>
            {
                new EmployeeGetDto { Id = employeesEntity[0].Id, Name = "Anna1" },
                new EmployeeGetDto { Id = employeesEntity[1].Id, Name = "Anna2" }
            };

            _userManagerMock.Setup(x => x.FindByNameAsync(username))
                .ReturnsAsync(appUser);

            _senderMock.Setup(x => x.Send(It.Is<GetUserEmployeesForListQuery>(x =>
                    x.appUserId == appUserId &&
                    x.searchTerm == searchTerm),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(employeesEntity);

            _mapperMock.Setup(x => x.Map<IEnumerable<EmployeeGetDto>>(employeesEntity))
                .Returns(expectedDtos);

            // Act
            var result = await _controller.GetUserEmployeesForListAsync(searchTerm);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var returnedDtos = okResult.Value as IEnumerable<EmployeeGetDto>;
            Assert.IsNotNull(returnedDtos);
            Assert.AreEqual(expectedDtos.Count(), returnedDtos.Count());
        }

        [TestMethod]
        public async Task AddEmployeeAsync_ReturnsOkResult_WithEmployeeGetDto()
        {
            // Arrange
            var appUserId = "user-id-123";
            var username = "testuser";

            var appUser = new AppUserEntity { Id = appUserId, UserName = username };

            var createDto = new EmployeeCreateDto
            {
                Name = "John Doe",
                Email = "john.doe@example.com",
                Phone = "123456789",
                Salary = 5000m
            };

            var employeeEntity = new EmployeeEntity
            {
                Id = Guid.NewGuid(),
                Name = createDto.Name,
                Email = createDto.Email,
                Phone = createDto.Phone,
                Salary = createDto.Salary,
                AppUserId = appUser.Id,
                AppUserEntity = appUser
            };

            var resultEntity = new EmployeeEntity
            {
                Id = employeeEntity.Id,
                Name = employeeEntity.Name,
                Email = employeeEntity.Email,
                Phone = employeeEntity.Phone,
                Salary = employeeEntity.Salary,
                AppUserId = appUser.Id,
                AppUserEntity = appUser
            };

            var expectedDto = new EmployeeGetDto
            {
                Id = resultEntity.Id,
                Name = resultEntity.Name,
                Email = resultEntity.Email,
                Phone = resultEntity.Phone,
                Salary = resultEntity.Salary
            };

            _userManagerMock.Setup(x => x.FindByNameAsync(username))
                .ReturnsAsync(appUser);

            _mapperMock.Setup(x => x.Map<EmployeeEntity>(createDto))
                .Returns(employeeEntity);

            _senderMock.Setup(x => x.Send(It.IsAny<AddEmployeeCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(resultEntity);

            _mapperMock.Setup(x => x.Map<EmployeeGetDto>(resultEntity))
                .Returns(expectedDto);

            // Act
            var result = await _controller.AddEmployeeAsync(createDto);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var returnedDto = okResult.Value as EmployeeGetDto;
            Assert.IsNotNull(returnedDto);
            Assert.AreEqual(expectedDto.Id, returnedDto.Id);
            Assert.AreEqual(expectedDto.Name, returnedDto.Name);
            Assert.AreEqual(expectedDto.Email, returnedDto.Email);
            Assert.AreEqual(expectedDto.Phone, returnedDto.Phone);
            Assert.AreEqual(expectedDto.Salary, returnedDto.Salary);
        }

        [TestMethod]
        public async Task UpdateEmployeeAsync_ReturnsOkResult_WithUpdatedEmployeeDto()
        {
            // Arrange
            var employeeId = Guid.NewGuid();
            var username = "testuser";

            var updateDto = new EmployeeCreateDto
            {
                Name = "Updated John Doe",
                Email = "updated.john.doe@example.com",
                Phone = "987654321",
                Salary = 6000m
            };

            var employeeEntity = new EmployeeEntity
            {
                Id = employeeId,
                Name = updateDto.Name,
                Email = updateDto.Email,
                Phone = updateDto.Phone,
                Salary = updateDto.Salary
            };

            var updatedEntity = new EmployeeEntity
            {
                Id = employeeId,
                Name = updateDto.Name,
                Email = updateDto.Email,
                Phone = updateDto.Phone,
                Salary = updateDto.Salary
            };

            var expectedDto = new EmployeeGetDto
            {
                Id = employeeId,
                Name = updateDto.Name,
                Email = updateDto.Email,
                Phone = updateDto.Phone,
                Salary = updateDto.Salary
            };

            _mapperMock.Setup(m => m.Map<EmployeeEntity>(updateDto))
                .Returns(employeeEntity);

            _senderMock.Setup(s => s.Send(
                It.Is<UpdateEmployeeCommand>(x =>
                    x.EmployeeId == employeeId &&
                    x.Employee == employeeEntity),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(updatedEntity);

            _mapperMock.Setup(m => m.Map<EmployeeGetDto>(updatedEntity))
                .Returns(expectedDto);

            // Act
            var result = await _controller.UpdateEmployeeAsync(employeeId, updateDto);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var returnedDto = okResult.Value as EmployeeGetDto;
            Assert.IsNotNull(returnedDto);
            Assert.AreEqual(expectedDto.Id, returnedDto.Id);
            Assert.AreEqual(expectedDto.Name, returnedDto.Name);
            Assert.AreEqual(expectedDto.Email, returnedDto.Email);
            Assert.AreEqual(expectedDto.Phone, returnedDto.Phone);
            Assert.AreEqual(expectedDto.Salary, returnedDto.Salary);
        }

        [TestMethod]
        public async Task DeleteEmployeeAsync_ReturnsOkResult_WithTrue_When_DeletedSuccessfully()
        {
            // Arrange
            var employeeId = Guid.NewGuid();
            var expectedResult = true;

            _senderMock.Setup(x => x.Send(It.Is<DeleteEmployeeCommand>(x=> x.employeeId == employeeId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.DeleteEmployeeAsync(employeeId);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(expectedResult, okResult.Value);
        }
    }
}