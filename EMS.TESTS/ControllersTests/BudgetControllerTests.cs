using AutoMapper;
using EMS.API.Controllers;
using EMS.APPLICATION.Dtos;
using EMS.APPLICATION.Features.Budget.Commands;
using EMS.APPLICATION.Features.Budget.Queries;
using EMS.CORE.Entities;
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
    public class BudgetControllerTests
    {
        private Mock<ISender> _mockSender;
        private Mock<UserManager<AppUserEntity>> _mockUserManager;
        private Mock<IMapper> _mockMapper;
        private BudgetController _controller;

        [TestInitialize]
        public void Setup()
        {
            _mockSender = new Mock<ISender>();
            _mockMapper = new Mock<IMapper>();

            var store = new Mock<IUserStore<AppUserEntity>>();
            _mockUserManager = new Mock<UserManager<AppUserEntity>>(store.Object, null, null, null, null, null, null, null, null);

            _controller = new BudgetController(_mockSender.Object, _mockUserManager.Object, _mockMapper.Object);

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
        public async Task AddUserBudgetAsync_ReturnOkResult_WithBudgetGetDto()
        {
            // Arrange
            var appUserId = "user-id-123";
            var username = "testuser";

            var appUser = new AppUserEntity { Id = appUserId, UserName = username };

            var createDto = new BudgetCreateDto
            {
                Budget = 3000.00m
            };

            var budgetEntity = new BudgetEntity
            {
                Id = Guid.NewGuid(),
                Budget = createDto.Budget,
                AppUserId = appUserId,
                AppUserEntity = appUser
            };

            var resultEntity = new BudgetEntity
            {
                Id = budgetEntity.Id,
                Budget = budgetEntity.Budget,
                AppUserId = budgetEntity.AppUserId,
                AppUserEntity = appUser
            };

            var expectedDto = new BudgetGetDto
            {
                Id = resultEntity.Id,
                Budget = resultEntity.Budget
            };

            _mockUserManager.Setup(x => x.FindByNameAsync(username))
                .ReturnsAsync(appUser);

            _mockMapper.Setup(x => x.Map<BudgetEntity>(createDto))
                .Returns(budgetEntity);

            _mockSender.Setup(x => x.Send(It.IsAny<AddBudgetCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(resultEntity);

            _mockMapper.Setup(x => x.Map<BudgetGetDto>(resultEntity))
                .Returns(expectedDto);

            // Act
            var result = await _controller.AddBudgetAsync(createDto);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var returnedDto = okResult.Value as BudgetGetDto;
            Assert.IsNotNull(returnedDto);
            Assert.AreEqual(expectedDto.Id, returnedDto.Id);
            Assert.AreEqual(expectedDto.Budget, returnedDto.Budget);
        }

        [TestMethod]
        public async Task GetUserBudgetAsync_ReturnsOkResult_WithBudgetGetDto()
        {
            // Arrange
            var appUserId = "user-id-123";
            var username = "testuser";

            var appUser = new AppUserEntity { Id = appUserId, UserName = username };

            var budgetEntity = new BudgetEntity
            {
                Id = Guid.NewGuid(),
                Budget = 4200.00m,
                AppUserId = appUserId,
                AppUserEntity = appUser
            };

            var expectedDto = new BudgetGetDto
            {
                Id = budgetEntity.Id,
                Budget = budgetEntity.Budget
            };

            _mockUserManager.Setup(x => x.FindByNameAsync(username))
                .ReturnsAsync(appUser);

            _mockSender.Setup(x => x.Send(It.Is<GetUserBudgetQuery>(x => x.appUserId == appUserId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(budgetEntity);

            _mockMapper.Setup(x => x.Map<BudgetGetDto>(budgetEntity))
                .Returns(expectedDto);

            // Act
            var result = await _controller.GetUserBudgetAsync();

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var returnedDto = okResult.Value as BudgetGetDto;
            Assert.IsNotNull(returnedDto);
            Assert.AreEqual(expectedDto.Id, returnedDto.Id);
            Assert.AreEqual(expectedDto.Budget, returnedDto.Budget);
        }

        [TestMethod]
        public async Task DeleteBudgetAsync_ReturnsOkResult_WithTrue_When_DeletedSuccessfully()
        {
            // Arrange
            var budgetId = Guid.NewGuid();
            var expectedResult = true;
            var appUserId = "user-id-123";
            var username = "testuser";

            var appUser = new AppUserEntity { Id = appUserId, UserName = username };

            _mockUserManager.Setup(x => x.FindByNameAsync(username))
               .ReturnsAsync(appUser);

            _mockSender.Setup(x => x.Send(It.Is<DeleteBudgetCommand>(x => x.budgetId == budgetId && x.appUserId == appUserId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.DeleteBudgetAsync(budgetId);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(expectedResult, okResult.Value);
        }

        [TestMethod]
        public async Task DeleteBudgetAsync_ReturnsNotFoundResult_WithFalse_When_DeletionFails()
        {
            // Arrange
            var budgetId = Guid.NewGuid();
            var expectedResult = false;
            var appUserId = "user-id-123";
            var username = "testuser";

            var appUser = new AppUserEntity { Id = appUserId, UserName = username };

            _mockUserManager.Setup(x => x.FindByNameAsync(username))
               .ReturnsAsync(appUser);

            _mockSender.Setup(x => x.Send(It.Is<DeleteBudgetCommand>(x => x.budgetId == budgetId && x.appUserId == appUserId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.DeleteBudgetAsync(budgetId);

            // Assert
            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);
            Assert.AreEqual(expectedResult, notFoundResult.Value);
        }
    }
}