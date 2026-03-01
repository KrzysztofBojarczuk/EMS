using AutoMapper;
using EMS.API.Controllers;
using EMS.APPLICATION.Dtos;
using EMS.APPLICATION.Features.Employee.Commands;
using EMS.APPLICATION.Features.Employee.Queries;
using EMS.CORE.Common;
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
        private Mock<ISender> _mockSender;
        private Mock<UserManager<AppUserEntity>> _mockUserManager;
        private Mock<IMapper> _mockMapper;
        private EmployeeController _controller;

        [TestInitialize]
        public void Setup()
        {
            _mockSender = new Mock<ISender>();
            _mockMapper = new Mock<IMapper>();

            var store = new Mock<IUserStore<AppUserEntity>>();
            _mockUserManager = new Mock<UserManager<AppUserEntity>>(store.Object, null, null, null, null, null, null, null, null);

            _controller = new EmployeeController(_mockSender.Object, _mockUserManager.Object, _mockMapper.Object);

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
        public async Task AddEmployeeAsync_ReturnsOkResult_WithEmployeeGetDto()
        {
            // Arrange
            var appUserId = "user-id-123";
            var username = "testuser";

            var appUser = new AppUserEntity { Id = appUserId, UserName = username };

            var createDto = new EmployeeCreateDto
            {
                Name = "Employee",
                Email = "employee@example.com",
                Phone = "123-456-789",
                Salary = 5000m,
                DateOfBirth = new DateTime(1990, 1, 1),
                EmploymentDate = new DateTime(2022, 1, 1),
                MedicalCheckValidUntil = new DateTime(2025, 1, 1)
            };

            var employeeEntity = new EmployeeEntity
            {
                Id = Guid.NewGuid(),
                Name = createDto.Name,
                Email = createDto.Email,
                Phone = createDto.Phone,
                Salary = createDto.Salary,
                DateOfBirth = createDto.DateOfBirth,
                EmploymentDate = createDto.EmploymentDate,
                MedicalCheckValidUntil = createDto.MedicalCheckValidUntil,
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
                DateOfBirth = createDto.DateOfBirth,
                EmploymentDate = employeeEntity.EmploymentDate,
                MedicalCheckValidUntil = employeeEntity.MedicalCheckValidUntil,
                AppUserId = appUser.Id,
                AppUserEntity = appUser
            };

            var expectedDto = new EmployeeGetDto
            {
                Id = resultEntity.Id,
                Name = resultEntity.Name,
                Email = resultEntity.Email,
                Phone = resultEntity.Phone,
                Salary = resultEntity.Salary,
                DateOfBirth = resultEntity.DateOfBirth,
                EmploymentDate = resultEntity.EmploymentDate,
                MedicalCheckValidUntil = resultEntity.MedicalCheckValidUntil
            };

            _mockUserManager.Setup(x => x.FindByNameAsync(username))
                .ReturnsAsync(appUser);

            _mockMapper.Setup(x => x.Map<EmployeeEntity>(createDto))
                .Returns(employeeEntity);

            _mockSender.Setup(x => x.Send(It.IsAny<AddEmployeeCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(resultEntity);

            _mockMapper.Setup(x => x.Map<EmployeeGetDto>(resultEntity))
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
            Assert.AreEqual(expectedDto.DateOfBirth, returnedDto.DateOfBirth);
            Assert.AreEqual(expectedDto.EmploymentDate, returnedDto.EmploymentDate);
            Assert.AreEqual(expectedDto.MedicalCheckValidUntil, returnedDto.MedicalCheckValidUntil);
        }

        [TestMethod]
        public async Task AddEmployeeListAsync_ReturnsOkResult_WithEmployeeListsGetDto()
        {
            // Arrange
            var appUserId = "user-id-123";
            var username = "testuser";

            var appUser = new AppUserEntity { Id = appUserId, UserName = username };

            var createDto = new EmployeeListsCreateDto
            {
                Name = "EmployeeList",
                EmployeeIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() }
            };

            var employeeListsEntity = new EmployeeListsEntity
            {
                Id = Guid.NewGuid(),
                Name = createDto.Name,
                AppUserId = appUserId
            };

            var resultEntity = new EmployeeListsEntity
            {
                Id = employeeListsEntity.Id,
                Name = employeeListsEntity.Name,
                AppUserId = appUserId
            };

            var expectedDto = new EmployeeListsGetDto
            {
                Id = resultEntity.Id,
                Name = resultEntity.Name,
                Employees = new List<EmployeeGetDto>()
            };

            _mockUserManager.Setup(x => x.FindByNameAsync(username))
                .ReturnsAsync(appUser);

            _mockMapper.Setup(x => x.Map<EmployeeListsEntity>(createDto))
                .Returns(employeeListsEntity);

            _mockSender.Setup(x => x.Send(It.Is<AddEmployeeListCommand>(x =>
                x.employeeList == employeeListsEntity &&
                x.employeeIds.SequenceEqual(createDto.EmployeeIds)),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<EmployeeListsEntity>.Success(resultEntity));

            _mockMapper.Setup(x => x.Map<EmployeeListsGetDto>(resultEntity))
                .Returns(expectedDto);

            // Act
            var result = await _controller.AddEmployeeListAsync(createDto);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var returnedDto = okResult.Value as EmployeeListsGetDto;
            Assert.IsNotNull(returnedDto);
            Assert.AreEqual(expectedDto.Id, returnedDto.Id);
            Assert.AreEqual(expectedDto.Name, returnedDto.Name);
        }

        [TestMethod]
        public async Task AddEmployeeListAsync_ReturnsBadRequest_WhenResultIsFailure()
        {
            // Arrange
            var appUserId = "user-id-123";
            var username = "testuser";

            var appUser = new AppUserEntity { Id = appUserId, UserName = username };

            var createDto = new EmployeeListsCreateDto
            {
                Name = "EmployeeList",
                EmployeeIds = new List<Guid> { Guid.NewGuid() }
            };

            var employeeListsEntity = new EmployeeListsEntity
            {
                Id = Guid.NewGuid(),
                Name = createDto.Name,
                AppUserId = appUserId
            };

            var expectedError = "A list with that name already exists.";

            _mockUserManager.Setup(x => x.FindByNameAsync(username))
               .ReturnsAsync(appUser);

            _mockMapper.Setup(x => x.Map<EmployeeListsEntity>(createDto))
                .Returns(employeeListsEntity);

            _mockSender.Setup(x => x.Send(It.IsAny<AddEmployeeListCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<EmployeeListsEntity>.Failure(expectedError));

            // Act
            var result = await _controller.AddEmployeeListAsync(createDto);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);
            Assert.AreEqual(expectedError, badRequestResult.Value);
        }

        [TestMethod]
        public async Task GetUserEmployeesAsync_ReturnsOkResult_WithEmployeeDtos()
        {
            // Arrange
            var appUserId = "user-id-123";
            var username = "testuser";
            var pageNumber = 1;
            var pageSize = 10;
            var searchTerm = "test";

            var appUser = new AppUserEntity { Id = appUserId, UserName = username };

            var employeesEntities = new List<EmployeeEntity>
            {
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 1 Test", Email = "employee1@example.com", Phone = "123-456-789", Salary = 5000, DateOfBirth = new DateTime(1990, 1, 1), EmploymentDate = new DateTime(2021, 1, 1), MedicalCheckValidUntil = new DateTime(2021, 1, 1) },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 2 Test", Email = "employee2@example.com", Phone = "123-456-789", Salary = 5000, DateOfBirth = new DateTime(1991, 1, 1), EmploymentDate = new DateTime(2022, 1, 1), MedicalCheckValidUntil = new DateTime(2022, 1, 1) }
            };

            var paginatedResult = new PaginatedList<EmployeeEntity>(employeesEntities, employeesEntities.Count(), pageNumber, pageSize);

            var expectedDtos = new List<EmployeeGetDto>
            {
                new EmployeeGetDto { Id = employeesEntities[0].Id, Name = "Employee 1 Test", Email = "employee1@example.com", Phone = "123-456-789", Salary = 5000, DateOfBirth = new DateTime(1990, 1, 1), EmploymentDate = new DateTime(2021, 1, 1), MedicalCheckValidUntil = new DateTime(2021, 1, 1) },
                new EmployeeGetDto { Id = employeesEntities[1].Id, Name = "Employee 2 Test", Email = "employee2@example.com", Phone = "123-456-789", Salary = 5000, DateOfBirth = new DateTime(1991, 1, 1), EmploymentDate = new DateTime(2022, 1, 1), MedicalCheckValidUntil = new DateTime(2022, 1, 1) }
            };

            _mockUserManager.Setup(x => x.FindByNameAsync(username))
               .ReturnsAsync(appUser);

            _mockSender.Setup(x => x.Send(It.Is<GetUserEmployeesQuery>(x =>
                x.appUserId == appUserId &&
                x.pageNumber == pageNumber &&
                x.pageSize == pageSize &&
                x.searchTerm == searchTerm &&
                x.sortOrder == null),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(paginatedResult);

            _mockMapper.Setup(x => x.Map<IEnumerable<EmployeeGetDto>>(employeesEntities))
                .Returns(expectedDtos);

            // Act
            var result = await _controller.GetUserEmployeesAsync(pageNumber, pageSize, searchTerm, null);

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

            _mockUserManager.Setup(x => x.FindByNameAsync(username))
               .ReturnsAsync(appUser);

            _mockSender.Setup(x => x.Send(It.Is<GetUserEmployeesQuery>(x =>
                x.appUserId == appUserId &&
                x.pageNumber == pageNumber &&
                x.pageSize == pageSize &&
                x.searchTerm == searchTerm &&
                x.sortOrder == null),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(paginatedResult);

            _mockMapper.Setup(x => x.Map<IEnumerable<EmployeeGetDto>>(employeesEntities))
                .Returns(expectedDtos);

            // Act
            var result = await _controller.GetUserEmployeesAsync(pageNumber, pageSize, searchTerm, null);

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
        public async Task GetUserEmployeesAsync_ReturnsOkResult_WithEmployeesDtosSortedBySalaryAscending()
        {
            // Arrange
            var appUserId = "user-id-123";
            var username = "testuser";
            var pageNumber = 1;
            var pageSize = 10;
            var sortOrder = "salary_asc";

            var appUser = new AppUserEntity { Id = appUserId, UserName = username };

            var employeesEntities = new List<EmployeeEntity>
            {
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 1", Email = "employee1@example.com", Phone = "123-456-789", Salary = 1000, DateOfBirth = new DateTime(1990, 1, 1), EmploymentDate = new DateTime(2021, 1, 1), MedicalCheckValidUntil = new DateTime(2021, 1, 1) },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 2", Email = "employee2@example.com", Phone = "123-456-789", Salary = 5000, DateOfBirth = new DateTime(1991, 1, 1), EmploymentDate = new DateTime(2022, 1, 1), MedicalCheckValidUntil = new DateTime(2022, 1, 1) },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 3", Email = "employee3@example.com", Phone = "123-456-789", Salary = 7000, DateOfBirth = new DateTime(1992, 1, 1), EmploymentDate = new DateTime(2023, 1, 1), MedicalCheckValidUntil = new DateTime(2023, 1, 1) }
            };

            var paginatedResult = new PaginatedList<EmployeeEntity>(employeesEntities, employeesEntities.Count(), pageNumber, pageSize);

            var expectedDtos = new List<EmployeeGetDto>
            {
                new  EmployeeGetDto { Id = employeesEntities[0].Id, Name = "Employee 1", Email = "employee1@example.com", Phone = "123-456-789", Salary = 1000, DateOfBirth = new DateTime(1990, 1, 1), EmploymentDate = new DateTime(2021, 1, 1), MedicalCheckValidUntil = new DateTime(2021, 1, 1) },
                new  EmployeeGetDto { Id = employeesEntities[1].Id, Name = "Employee 2", Email = "employee2@example.com", Phone = "123-456-789", Salary = 5000, DateOfBirth = new DateTime(1991, 1, 1), EmploymentDate = new DateTime(2022, 1, 1), MedicalCheckValidUntil = new DateTime(2021, 1, 1) },
                new  EmployeeGetDto { Id = employeesEntities[0].Id, Name = "Employee 3", Email = "employee3@example.com", Phone = "123-456-789", Salary = 7000, DateOfBirth = new DateTime(1992, 1, 1), EmploymentDate = new DateTime(2023, 1, 1), MedicalCheckValidUntil = new DateTime(2023, 1, 1) }
            };

            _mockUserManager.Setup(x => x.FindByNameAsync(username))
                .ReturnsAsync(appUser);

            _mockSender.Setup(x => x.Send(It.Is<GetUserEmployeesQuery>(x =>
                x.appUserId == appUserId &&
                x.pageNumber == pageNumber &&
                x.pageSize == pageSize &&
                x.searchTerm == null &&
                x.sortOrder == sortOrder),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(paginatedResult);

            _mockMapper.Setup(x => x.Map<IEnumerable<EmployeeGetDto>>(employeesEntities))
                .Returns(expectedDtos);

            // Act
            var result = await _controller.GetUserEmployeesAsync(pageNumber, pageSize, null, sortOrder);

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
        public async Task GetUserEmployeesAsync_ReturnsOkResult_WithEmployeesDtosSortedBySalaryDescending()
        {
            // Arrange
            var appUserId = "user-id-123";
            var username = "testuser";
            var pageNumber = 1;
            var pageSize = 10;
            var sortOrder = "salary_desc";

            var appUser = new AppUserEntity { Id = appUserId, UserName = username };

            var employeesEntities = new List<EmployeeEntity>
            {
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 1", Email = "employee1@example.com", Phone = "123-456-789", Salary = 1000, DateOfBirth = new DateTime(1990, 1, 1), EmploymentDate = new DateTime(2021, 1, 1), MedicalCheckValidUntil = new DateTime(2021, 1, 1) },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 2", Email = "employee2@example.com", Phone = "123-456-789", Salary = 5000, DateOfBirth = new DateTime(1991, 1, 1), EmploymentDate = new DateTime(2022, 1, 1), MedicalCheckValidUntil = new DateTime(2022, 1, 1) },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 3", Email = "employee3@example.com", Phone = "123-456-789", Salary = 7000, DateOfBirth = new DateTime(1992, 1, 1), EmploymentDate = new DateTime(2023, 1, 1), MedicalCheckValidUntil = new DateTime(2023, 3, 1) }
            };

            var paginatedResult = new PaginatedList<EmployeeEntity>(employeesEntities, employeesEntities.Count(), pageNumber, pageSize);

            var expectedDtos = new List<EmployeeGetDto>
            {
                new  EmployeeGetDto { Id = employeesEntities[0].Id, Name = "Employee 1", Email = "employee1@example.com", Phone = "123-456-789", Salary = 1000, DateOfBirth = new DateTime(1990, 1, 1), EmploymentDate = new DateTime(2021, 1, 1), MedicalCheckValidUntil = new DateTime(2021, 1, 1) },
                new  EmployeeGetDto { Id = employeesEntities[1].Id, Name = "Employee 2", Email = "employee2@example.com", Phone = "123-456-789", Salary = 5000, DateOfBirth = new DateTime(1991, 1, 1), EmploymentDate = new DateTime(2022, 1, 1), MedicalCheckValidUntil = new DateTime(2022, 1, 1) },
                new  EmployeeGetDto { Id = employeesEntities[0].Id, Name = "Employee 3", Email = "employee3@example.com", Phone = "123-456-789", Salary = 7000, DateOfBirth = new DateTime(1992, 1, 1), EmploymentDate = new DateTime(2023, 1, 1), MedicalCheckValidUntil = new DateTime(2023, 1, 1) }
            };

            _mockUserManager.Setup(x => x.FindByNameAsync(username))
                .ReturnsAsync(appUser);

            _mockSender.Setup(x => x.Send(It.Is<GetUserEmployeesQuery>(x =>
                x.appUserId == appUserId &&
                x.pageNumber == pageNumber &&
                x.pageSize == pageSize &&
                x.searchTerm == null &&
                x.sortOrder == sortOrder),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(paginatedResult);

            _mockMapper.Setup(x => x.Map<IEnumerable<EmployeeGetDto>>(employeesEntities))
                .Returns(expectedDtos);

            // Act
            var result = await _controller.GetUserEmployeesAsync(pageNumber, pageSize, null, sortOrder);

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
        public async Task GetUserEmployeesAsync_ReturnsOkResult_WithEmployeesDtosSortedBySalaryDescending_When_Sorted_NotFound()
        {
            // Arrange
            var appUserId = "user-id-123";
            var username = "testuser";
            var pageNumber = 1;
            var pageSize = 10;
            var sortOrder = "nonexistent";

            var appUser = new AppUserEntity { Id = appUserId, UserName = username };

            var employeesEntities = new List<EmployeeEntity>
            {
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 1", Email = "employee1@example.com", Phone = "123-456-789", Salary = 7000, DateOfBirth = new DateTime(1990, 1, 1), EmploymentDate = new DateTime(2021, 1, 1), MedicalCheckValidUntil = new DateTime(2021, 1, 1) },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 2", Email = "employee2@example.com", Phone = "123-456-789", Salary = 5000, DateOfBirth = new DateTime(1991, 1, 1), EmploymentDate = new DateTime(2022, 1, 1), MedicalCheckValidUntil = new DateTime(2022, 1, 1) },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 3", Email = "employee3@example.com", Phone = "123-456-789", Salary = 1000, DateOfBirth = new DateTime(1992, 1, 1), EmploymentDate = new DateTime(2023, 1, 1), MedicalCheckValidUntil = new DateTime(2023, 1, 1) }
            };

            var paginatedResult = new PaginatedList<EmployeeEntity>(employeesEntities, employeesEntities.Count(), pageNumber, pageSize);

            var expectedDtos = new List<EmployeeGetDto>
            {
                new  EmployeeGetDto { Id = employeesEntities[0].Id, Name = "Employee 1", Email = "employee1@example.com", Phone = "123-456-789", Salary = 7000, DateOfBirth = new DateTime(1990, 1, 1), EmploymentDate = new DateTime(2021, 1, 1), MedicalCheckValidUntil = new DateTime(2021, 1, 1) },
                new  EmployeeGetDto { Id = employeesEntities[1].Id, Name = "Employee 2", Email = "employee2@example.com", Phone = "123-456-789", Salary = 5000, DateOfBirth = new DateTime(1991, 1, 1), EmploymentDate = new DateTime(2022, 1, 1), MedicalCheckValidUntil = new DateTime(2022, 1, 1) },
                new  EmployeeGetDto { Id = employeesEntities[0].Id, Name = "Employee 3", Email = "employee3@example.com", Phone = "123-456-789", Salary = 1000, DateOfBirth = new DateTime(1992, 1, 1), EmploymentDate = new DateTime(2023, 1, 1), MedicalCheckValidUntil = new DateTime(2023, 1, 1) }
            };

            _mockUserManager.Setup(x => x.FindByNameAsync(username))
                .ReturnsAsync(appUser);

            _mockSender.Setup(x => x.Send(It.Is<GetUserEmployeesQuery>(x =>
                x.appUserId == appUserId &&
                x.pageNumber == pageNumber &&
                x.pageSize == pageSize &&
                x.searchTerm == null &&
                x.sortOrder == sortOrder),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(paginatedResult);

            _mockMapper.Setup(x => x.Map<IEnumerable<EmployeeGetDto>>(employeesEntities))
                .Returns(expectedDtos);

            // Act
            var result = await _controller.GetUserEmployeesAsync(pageNumber, pageSize, null, sortOrder);

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
        public async Task GetUserEmployeeListAsync_ReturnsOkResult_WithEmployeeListsDtos()
        {
            // Arrange
            var appUserId = "user-id-123";
            var username = "testuser";
            var searchTerm = "test";

            var appUser = new AppUserEntity { Id = appUserId, UserName = username };

            var employeeListsEntities = new List<EmployeeListsEntity>
            {
                new EmployeeListsEntity
                {
                     Id = Guid.NewGuid(),
                     Name = "EmployeeList 1 Test",
                     AppUserId = appUserId,
                     EmployeesEntities = new List<EmployeeEntity>
                     {
                         new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 1", Email = "employee1@example.com", Phone = "123-456-789", Salary = 5000, DateOfBirth = new DateTime(1990, 1, 1), EmploymentDate = new DateTime(2021, 1, 1), MedicalCheckValidUntil = new DateTime(2021, 1, 1) },
                         new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 2", Email = "employee2@example.com", Phone = "123-456-789", Salary = 5000, DateOfBirth = new DateTime(1991, 1, 1), EmploymentDate = new DateTime(2022, 1, 1), MedicalCheckValidUntil = new DateTime(2021, 1, 1) }
                     }
                },
                new EmployeeListsEntity
                {
                    Id = Guid.NewGuid(),
                    Name = "EmployeeList 2 Test",
                    AppUserId = appUserId,
                    EmployeesEntities = new List<EmployeeEntity>
                    {
                        new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 3", Email = "employee3@example.com", Phone = "123-456-789", Salary = 5000, DateOfBirth = new DateTime(1990, 1, 1), EmploymentDate = new DateTime(2019, 5, 10), MedicalCheckValidUntil = new DateTime(2021, 1, 1) }
                    }
                }
            };

            var expectedDtos = new List<EmployeeListsGetDto>
            {
                new EmployeeListsGetDto
                {
                    Id = employeeListsEntities[0].Id,
                    Name = "EmployeeList 1 Test",
                    Employees = new List<EmployeeGetDto>
                    {
                        new EmployeeGetDto { Id = employeeListsEntities[0].EmployeesEntities.First().Id, Name = "Employee 1", Email = "employee1@example.com", Phone = "123-456-789", Salary = 5000, DateOfBirth = new DateTime(1990, 1, 1), EmploymentDate = new DateTime(2021, 1, 1), MedicalCheckValidUntil = new DateTime(2021, 1, 1) },
                        new EmployeeGetDto { Id = employeeListsEntities[0].EmployeesEntities.Last().Id, Name = "Employee 2", Email = "employee2@example.com", Phone = "123-456-789", Salary = 5000, DateOfBirth = new DateTime(1991, 1, 1), EmploymentDate = new DateTime(2022, 1, 1), MedicalCheckValidUntil = new DateTime(2022, 1, 1) }
                    }
                },
                 new EmployeeListsGetDto
                 {
                     Id = employeeListsEntities[1].Id,
                     Name = "EmployeeList 2 Test",
                     Employees = new List<EmployeeGetDto>
                     {
                         new EmployeeGetDto { Id = employeeListsEntities[1].EmployeesEntities.First().Id, Name = "Employee 3", Email = "employee3@example.com", Phone = "123-456-789", Salary = 5000, DateOfBirth = new DateTime(1990, 1, 1), EmploymentDate = new DateTime(2021, 1, 1), MedicalCheckValidUntil = new DateTime(2021, 1, 1) }
                     }
                  }
             };

            _mockUserManager.Setup(x => x.FindByNameAsync(username))
                .ReturnsAsync(appUser);

            _mockSender.Setup(x => x.Send(It.Is<GetUserEmployeeListsQuery>(x =>
                x.appUserId == appUserId &&
                x.searchTerm == searchTerm),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(employeeListsEntities);

            _mockMapper.Setup(x => x.Map<IEnumerable<EmployeeListsGetDto>>(employeeListsEntities))
                .Returns(expectedDtos);

            // Act
            var result = await _controller.GetUserEmployeeListAsync(searchTerm);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var returnedDtos = okResult.Value as IEnumerable<EmployeeListsGetDto>;
            Assert.IsNotNull(returnedDtos);
            Assert.AreEqual(expectedDtos.Count(), returnedDtos.Count());
            Assert.AreEqual(expectedDtos[0].Name, returnedDtos.First().Name);
            Assert.AreEqual(expectedDtos.First().Employees.First().Name, returnedDtos.First().Employees.First().Name);
        }

        [TestMethod]
        public async Task GetUserEmployeeListAsync_ReturnsOkResult_NotFound_WithEmptyList()
        {
            // Arrange
            var appUserId = "user-id-123";
            var username = "testuser";
            var searchTerm = "nonexistent";

            var appUser = new AppUserEntity { Id = appUserId, UserName = username };

            var employeeListsEntities = new List<EmployeeListsEntity>();

            var expectedDtos = new List<EmployeeListsGetDto>();

            _mockUserManager.Setup(x => x.FindByNameAsync(username))
                .ReturnsAsync(appUser);

            _mockSender.Setup(x => x.Send(It.Is<GetUserEmployeeListsQuery>(x =>
                x.appUserId == appUserId &&
                x.searchTerm == searchTerm),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(employeeListsEntities);

            _mockMapper.Setup(x => x.Map<IEnumerable<EmployeeListsGetDto>>(employeeListsEntities))
                .Returns(expectedDtos);

            // Act
            var result = await _controller.GetUserEmployeeListAsync(searchTerm);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var returnedDtos = okResult.Value as IEnumerable<EmployeeListsGetDto>;
            Assert.IsNotNull(returnedDtos);
            Assert.AreEqual(expectedDtos.Count(), returnedDtos.Count());
        }

        [TestMethod]
        public async Task GetUserEmployeeListForTaskAsync_ReturnsOkResult_WithEmployeeListsDtos()
        {
            // Arrange
            var appUserId = "user-id-123";
            var username = "testuser";
            var searchTerm = "test";

            var appUser = new AppUserEntity { Id = appUserId, UserName = username };

            var employeeListsEntities = new List<EmployeeListsEntity>
            {
                new EmployeeListsEntity
                {
                     Id = Guid.NewGuid(),
                     Name = "EmployeeList 1 Test",
                     AppUserId = appUserId,
                      EmployeesEntities = new List<EmployeeEntity>
                      {
                          new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 1", Email = "employee1@example.com", Phone = "123-456-789", Salary = 5000, DateOfBirth = new DateTime(1990, 1, 1), EmploymentDate = new DateTime(2021, 1, 1), MedicalCheckValidUntil = new DateTime(2025, 1, 10) },
                          new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 2", Email = "rmployee2@example.com", Phone = "123-456-789", Salary = 5000, DateOfBirth = new DateTime(1991, 1, 1), EmploymentDate = new DateTime(2022, 1, 1), MedicalCheckValidUntil = new DateTime(2025, 1, 1) }
                      }
                },
                new EmployeeListsEntity
                {
                    Id = Guid.NewGuid(),
                    Name = "EmployeeList 2 Test",
                    AppUserId = appUserId,
                    EmployeesEntities = new List<EmployeeEntity>
                    {
                       new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 3", Email = "employee3@example.com", Phone = "123-456-789", Salary = 5000, DateOfBirth = new DateTime(1990, 1, 1), EmploymentDate = new DateTime(2022, 1, 1), MedicalCheckValidUntil = new DateTime(2021, 1, 1) }
                    }
                }
            };

            var expectedDtos = new List<EmployeeListsGetDto>
            {
                new EmployeeListsGetDto
                {
                    Id = employeeListsEntities[0].Id,
                    Name = "EmployeeList 1 Test",
                    Employees = new List<EmployeeGetDto>
                    {
                        new EmployeeGetDto { Id = employeeListsEntities[0].EmployeesEntities.First().Id, Name = "Employee 1", Email = "employee1@example.com", Phone = "123-456-789", Salary = 5000, DateOfBirth = new DateTime(1990, 1, 1), EmploymentDate = new DateTime(2021, 1, 1), MedicalCheckValidUntil = new DateTime(2021, 1, 1) },
                        new EmployeeGetDto { Id = employeeListsEntities[0].EmployeesEntities.Last().Id, Name = "Employee 2", Email = "employee2@example.com", Phone = "123-456-789", Salary = 5000, DateOfBirth = new DateTime(1991, 1, 1), EmploymentDate = new DateTime(2012, 1, 1), MedicalCheckValidUntil = new DateTime(2022, 1, 1) }
                    }
                 },
                 new EmployeeListsGetDto
                 {
                     Id = employeeListsEntities[1].Id,
                     Name = "EmployeeList 2 Test",
                     Employees = new List<EmployeeGetDto>
                     {
                         new EmployeeGetDto { Id = employeeListsEntities[1].EmployeesEntities.First().Id, Name = "Employee 3", Email = "employee3@example.com", Phone = "123-456-789", Salary = 5000, DateOfBirth = new DateTime(1990, 1, 1), EmploymentDate = new DateTime(2019, 1, 1), MedicalCheckValidUntil = new DateTime(2021, 1, 1) }
                     }
                  }
             };

            _mockUserManager.Setup(x => x.FindByNameAsync(username))
                .ReturnsAsync(appUser);

            _mockSender.Setup(x => x.Send(It.Is<GetUserEmployeeListsForTaskQuery>(x =>
                x.appUserId == appUserId &&
                x.searchTerm == searchTerm),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(employeeListsEntities);

            _mockMapper.Setup(x => x.Map<IEnumerable<EmployeeListsGetDto>>(employeeListsEntities))
                .Returns(expectedDtos);

            // Act
            var result = await _controller.GetUserEmployeeListForTaskAsync(searchTerm);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var returnedDtos = okResult.Value as IEnumerable<EmployeeListsGetDto>;
            Assert.IsNotNull(returnedDtos);
            Assert.AreEqual(expectedDtos.Count(), returnedDtos.Count());
            Assert.AreEqual(expectedDtos[0].Name, returnedDtos.First().Name);
            Assert.AreEqual(expectedDtos.First().Employees.First().Name, returnedDtos.First().Employees.First().Name);
        }

        [TestMethod]
        public async Task GetUserEmployeeListForTaskAsync_ReturnsOkResult_NotFound_WithEmptyList()
        {
            // Arrange
            var appUserId = "user-id-123";
            var username = "testuser";
            var searchTerm = "nonexistent";

            var appUser = new AppUserEntity { Id = appUserId, UserName = username };

            var employeeListsEntities = new List<EmployeeListsEntity>();

            var expectedDtos = new List<EmployeeListsGetDto>();

            _mockUserManager.Setup(x => x.FindByNameAsync(username))
                .ReturnsAsync(appUser);

            _mockSender.Setup(x => x.Send(It.Is<GetUserEmployeeListsForTaskQuery>(x =>
                x.appUserId == appUserId &&
                x.searchTerm == searchTerm),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(employeeListsEntities);

            _mockMapper.Setup(x => x.Map<IEnumerable<EmployeeListsGetDto>>(employeeListsEntities))
                .Returns(expectedDtos);

            // Act
            var result = await _controller.GetUserEmployeeListForTaskAsync(searchTerm);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var returnedDtos = okResult.Value as IEnumerable<EmployeeListsGetDto>;
            Assert.IsNotNull(returnedDtos);
            Assert.AreEqual(expectedDtos.Count(), returnedDtos.Count());
        }

        [TestMethod]
        public async Task GetUserEmployeesForListAddAsync_ReturnsOkResult_WithEmployeeDtos()
        {
            // Arrange
            var appUserId = "user-id-123";
            var username = "testuser";
            var searchTerm = "test";

            var appUser = new AppUserEntity { Id = appUserId, UserName = username };

            var employeesEntities = new List<EmployeeEntity>
            {
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 1 Test", Email = "employee1@example.com", Phone = "123-456-789", Salary = 5000, DateOfBirth = new DateTime(1990, 1, 1), EmploymentDate = new DateTime(2021, 1, 1), MedicalCheckValidUntil = new DateTime(2021, 1, 1) },
                new EmployeeEntity { Id = Guid.NewGuid(), Name = "Employee 2 Test", Email = "employee2@example.com", Phone = "123-456-789", Salary = 5000, DateOfBirth = new DateTime(1991, 1, 1), EmploymentDate = new DateTime(2022, 1, 1), MedicalCheckValidUntil = new DateTime(2022, 1, 1) }
            };

            var expectedDtos = new List<EmployeeGetDto>
            {
                new EmployeeGetDto { Id = employeesEntities[0].Id, Name = "Employee 1 Test", Email = "employee1@example.com", Phone = "123-456-789", Salary = 5000, DateOfBirth = new DateTime(1990, 1, 1), EmploymentDate = new DateTime(2021, 1, 1), MedicalCheckValidUntil = new DateTime(2021, 1, 1) },
                new EmployeeGetDto { Id = employeesEntities[1].Id, Name = "Employee 2 Test", Email = "employee2@example.com", Phone = "123-456-789", Salary = 5000, DateOfBirth = new DateTime(1991, 1, 1), EmploymentDate = new DateTime(2022, 1, 1), MedicalCheckValidUntil = new DateTime(2022, 1, 1) }
            };

            _mockUserManager.Setup(x => x.FindByNameAsync(username))
                .ReturnsAsync(appUser);

            _mockSender.Setup(x => x.Send(It.Is<GetUserEmployeesForListAddQuery>(x =>
                x.appUserId == appUserId &&
                x.searchTerm == searchTerm),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(employeesEntities);

            _mockMapper.Setup(x => x.Map<IEnumerable<EmployeeGetDto>>(employeesEntities))
                .Returns(expectedDtos);

            // Act
            var result = await _controller.GetUserEmployeesForListAddAsync(searchTerm);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var returnedDtos = okResult.Value as IEnumerable<EmployeeGetDto>;
            Assert.IsNotNull(returnedDtos);
            Assert.AreEqual(expectedDtos.Count(), returnedDtos.Count());
        }

        [TestMethod]
        public async Task GetUserEmployeesForListAddAsync_ReturnsOkResult_NotFound_WithEmptyList()
        {
            // Arrange
            var appUserId = "user-id-123";
            var username = "testuser";
            var searchTerm = "nonexistent";

            var appUser = new AppUserEntity { Id = appUserId, UserName = username };

            var employeesEntity = new List<EmployeeEntity>();

            var expectedDtos = new List<EmployeeGetDto>();

            _mockUserManager.Setup(x => x.FindByNameAsync(username))
                .ReturnsAsync(appUser);

            _mockSender.Setup(x => x.Send(It.Is<GetUserEmployeesForListAddQuery>(x =>
                x.appUserId == appUserId &&
                x.searchTerm == searchTerm),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(employeesEntity);

            _mockMapper.Setup(x => x.Map<IEnumerable<EmployeeGetDto>>(employeesEntity))
                .Returns(expectedDtos);

            // Act
            var result = await _controller.GetUserEmployeesForListAddAsync(searchTerm);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var returnedDtos = okResult.Value as IEnumerable<EmployeeGetDto>;
            Assert.IsNotNull(returnedDtos);
            Assert.AreEqual(expectedDtos.Count(), returnedDtos.Count());
        }

        [TestMethod]
        public async Task UpdateEmployeeAsync_ReturnsOkResult_WithUpdatedEmployeeDto()
        {
            // Arrange
            var employeeId = Guid.NewGuid();
            var appUserId = "user-id-123";
            var username = "testuser";

            var appUser = new AppUserEntity { Id = appUserId, UserName = username };

            _mockUserManager.Setup(x => x.FindByNameAsync(username))
               .ReturnsAsync(appUser);

            var updateDto = new EmployeeCreateDto
            {
                Name = "Employee",
                Email = "employee@example.com",
                Phone = "123-456-789",
                Salary = 6000m,
                DateOfBirth = new DateTime(1990, 1, 1),
                EmploymentDate = new DateTime(2022, 1, 1),
                MedicalCheckValidUntil = new DateTime(2025, 1, 1)
            };

            var employeeEntity = new EmployeeEntity
            {
                Id = employeeId,
                Name = updateDto.Name,
                Email = updateDto.Email,
                Phone = updateDto.Phone,
                Salary = updateDto.Salary,
                DateOfBirth = updateDto.DateOfBirth,
                EmploymentDate = updateDto.EmploymentDate,
                MedicalCheckValidUntil = updateDto.MedicalCheckValidUntil
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
                Salary = updateDto.Salary,
                DateOfBirth = updateDto.DateOfBirth,
                EmploymentDate = updateDto.EmploymentDate,
                MedicalCheckValidUntil = updateDto.MedicalCheckValidUntil
            };

            _mockMapper.Setup(x => x.Map<EmployeeEntity>(updateDto))
                .Returns(employeeEntity);

            _mockSender.Setup(x => x.Send(It.Is<UpdateEmployeeCommand>(x =>
                x.employeeId == employeeId &&
                x.appUserId == appUserId &&
                x.employee == employeeEntity),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(updatedEntity);

            _mockMapper.Setup(x => x.Map<EmployeeGetDto>(updatedEntity))
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
            var appUserId = "user-id-123";
            var username = "testuser";

            var appUser = new AppUserEntity { Id = appUserId, UserName = username };

            _mockUserManager.Setup(x => x.FindByNameAsync(username))
               .ReturnsAsync(appUser);

            _mockSender.Setup(x => x.Send(It.Is<DeleteEmployeeCommand>(x => x.employeeId == employeeId && x.appUserId == appUserId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.DeleteEmployeeAsync(employeeId);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(expectedResult, okResult.Value);
        }

        [TestMethod]
        public async Task DeleteEmployeeAsync_ReturnsNotFoundResult_WithFalse_When_DeletionFails()
        {
            // Arrange
            var employeeId = Guid.NewGuid();
            var expectedResult = false;
            var appUserId = "user-id-123";
            var username = "testuser";

            var appUser = new AppUserEntity { Id = appUserId, UserName = username };

            _mockUserManager.Setup(x => x.FindByNameAsync(username))
               .ReturnsAsync(appUser);

            _mockSender.Setup(x => x.Send(It.Is<DeleteEmployeeCommand>(x => x.employeeId == employeeId && x.appUserId == appUserId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.DeleteEmployeeAsync(employeeId);

            // Assert
            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);
            Assert.AreEqual(expectedResult, notFoundResult.Value);
        }

        [TestMethod]
        public async Task DeleteEmployeeListAsync_ReturnsOkResult_WithTrue_When_DeletedSuccessfully()
        {
            // Arrange
            var employeeListId = Guid.NewGuid();
            var expectedResult = true;
            var appUserId = "user-id-123";
            var username = "testuser";

            var appUser = new AppUserEntity { Id = appUserId, UserName = username };

            _mockUserManager.Setup(x => x.FindByNameAsync(username))
               .ReturnsAsync(appUser);

            _mockSender.Setup(x => x.Send(It.Is<DeleteEmployeeListCommand>(x => x.employeeListId == employeeListId && x.appUserId == appUserId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.DeleteEmployeeListAsync(employeeListId);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(expectedResult, okResult.Value);
        }

        [TestMethod]
        public async Task DeleteEmployeeListAsync_ReturnsNotFoundResult_WithFalse_When_DeletionFails()
        {
            // Arrange
            var employeeListId = Guid.NewGuid();
            var expectedResult = false;
            var appUserId = "user-id-123";
            var username = "testuser";

            var appUser = new AppUserEntity { Id = appUserId, UserName = username };

            _mockUserManager.Setup(x => x.FindByNameAsync(username))
               .ReturnsAsync(appUser);

            _mockSender.Setup(x => x.Send(It.Is<DeleteEmployeeListCommand>(x => x.employeeListId == employeeListId && x.appUserId == appUserId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.DeleteEmployeeListAsync(employeeListId);

            // Assert
            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);
            Assert.AreEqual(expectedResult, notFoundResult.Value);
        }
    }
}