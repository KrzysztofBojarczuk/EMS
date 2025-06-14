using AutoMapper;
using EMS.API.Controllers;
using EMS.APPLICATION.Dtos;
using EMS.APPLICATION.Features.Budget.Commands;
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
            var userId = "user-123";
            var appUser = new AppUserEntity { Id = userId, UserName = "testuser" };

            var createDto = new BudgetCreateDto
            {
                Budget = 3000.00m
            };

            var budgetEntity = new BudgetEntity
            {
                Id = Guid.NewGuid(),
                Budget = createDto.Budget,
                AppUserId = userId,
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

            _mockUserManager
                .Setup(x => x.FindByNameAsync("testuser"))
                .ReturnsAsync(appUser);

            _mockMapper
                .Setup(m => m.Map<BudgetEntity>(createDto))
                .Returns(budgetEntity);

            _mockSender
                .Setup(s => s.Send(It.IsAny<AddBudgetCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(resultEntity);

            _mockMapper
                .Setup(m => m.Map<BudgetGetDto>(resultEntity))
                .Returns(expectedDto);

            // Act
            var result = await _controller.AddUserBudgetAsync(createDto);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var returnedDto = okResult.Value as BudgetGetDto;
            Assert.IsNotNull(returnedDto);
            Assert.AreEqual(expectedDto.Id, returnedDto.Id);
            Assert.AreEqual(expectedDto.Budget, returnedDto.Budget);
        }
    }
}