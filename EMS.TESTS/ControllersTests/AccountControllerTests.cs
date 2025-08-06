using EMS.API.Controllers;
using EMS.APPLICATION.Features.Account.Commands;
using EMS.APPLICATION.Features.Account.Queries;
using EMS.CORE.Entities;
using EMS.INFRASTRUCTURE.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EMS.TESTS.ControllersTests
{
    [TestClass]
    public class AccountControllerTests
    {
        private Mock<ISender> _mockSender;
        private AccountController _controller;

        [TestInitialize]
        public void Setup()
        {
            _mockSender = new Mock<ISender>();
            _controller = new AccountController(null, null, null, _mockSender.Object);
        }

        [TestMethod]
        public async Task DeleteUserAsync_ReturnsOkResult_WithTrue_When_DeletedSuccessfully()
        {
            // Arrange
            var appUserId = "user-id-123";
            var expectedResult = true;

            _mockSender.Setup(x => x.Send(It.Is<DeleteUserCommand>(x => x.appUserId == appUserId), It.IsAny<CancellationToken>()))
                       .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.DeleteUserAsync(appUserId);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(expectedResult, okResult.Value);
        }

        [TestMethod]
        public async Task DeleteUserAsync_ReturnsOkResult_WithFalse_When_DeletionFails()
        {
            // Arrange
            var appUserId = "user-id-123";
            var expectedResult = false;

            _mockSender.Setup(x => x.Send(It.Is<DeleteUserCommand>(x => x.appUserId == appUserId), It.IsAny<CancellationToken>()))
                       .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.DeleteUserAsync(appUserId);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(expectedResult, okResult.Value);
        }

        [TestMethod]
        public async Task GetNumberOfUsersAsync_ReturnsOkResult_WithNumber()
        {
            // Arrange
            var expectedNumberOfUsers = 42;
            _mockSender.Setup(x => x.Send(It.IsAny<GetNumberOfUsersQuery>(), It.IsAny<CancellationToken>()))
                       .ReturnsAsync(expectedNumberOfUsers);

            // Act
            var result = await _controller.GetNumberOfUsersAsync();

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(expectedNumberOfUsers, okResult.Value);
        }

        [TestMethod]
        public async Task GetAllUserAsync_ReturnsOkResult_BySearchTerm_WithUsers()
        {
            // Arrange
            var pageNumber = 1;
            var pageSize = 10;
            var searchTerm = "John";

            var expectedUsers = new List<AppUserEntity>
            {
                new AppUserEntity { UserName = "John", Email = "john@example.com" },
                new AppUserEntity { UserName = "Johnny", Email = "johnny@example.com" }
            };

            var expectedResult = new PaginatedList<AppUserEntity>(expectedUsers, expectedUsers.Count, pageNumber, pageSize);

            _mockSender.Setup(x => x.Send(
                It.Is<GetAllUserQuery>(x =>
                    x.pageNumber == pageNumber &&
                    x.pageSize == pageSize &&
                    x.searchTerm == searchTerm),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.GetAllUserAsync(pageNumber, pageSize, searchTerm);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var value = okResult.Value;
            var userGetProperty = value.GetType().GetProperty("userGet");
            var totalItemsProperty = value.GetType().GetProperty("TotalItems");
            var totalPagesProperty = value.GetType().GetProperty("TotalPages");
            var pageIndexProperty = value.GetType().GetProperty("PageIndex");

            Assert.IsNotNull(userGetProperty);
            var users = userGetProperty.GetValue(value) as IEnumerable<AppUserEntity>;
            Assert.IsNotNull(users);
            Assert.AreEqual(expectedUsers.Count, users.Count());

            Assert.AreEqual(expectedResult.TotalItems, totalItemsProperty.GetValue(value));
            Assert.AreEqual(expectedResult.TotalPages, totalPagesProperty.GetValue(value));
            Assert.AreEqual(expectedResult.PageIndex, pageIndexProperty.GetValue(value));
        }

        [TestMethod]
        public async Task GetAllUserAsync_ReturnsOkResult_NotFound_WithEmptyList()
        {
            // Arrange
            var pageNumber = 1;
            var pageSize = 10;
            var searchTerm = "nonexistent";

            var expectedUsers = new List<AppUserEntity>(); 
            var expectedResult = new PaginatedList<AppUserEntity>(expectedUsers, 0, pageNumber, pageSize);

            _mockSender.Setup(x => x.Send(
                It.Is<GetAllUserQuery>(x =>
                    x.pageNumber == pageNumber &&
                    x.pageSize == pageSize &&
                    x.searchTerm == searchTerm),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.GetAllUserAsync(pageNumber, pageSize, searchTerm);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var value = okResult.Value;
            var userGetProperty = value.GetType().GetProperty("userGet");
            var totalItemsProperty = value.GetType().GetProperty("TotalItems");
            var totalPagesProperty = value.GetType().GetProperty("TotalPages");
            var pageIndexProperty = value.GetType().GetProperty("PageIndex");

            Assert.IsNotNull(userGetProperty);
            var users = userGetProperty.GetValue(value) as IEnumerable<AppUserEntity>;
            Assert.IsNotNull(users);
            Assert.AreEqual(expectedUsers.Count, users.Count());

            Assert.AreEqual(expectedResult.TotalItems, totalItemsProperty.GetValue(value));
            Assert.AreEqual(expectedResult.TotalPages, totalPagesProperty.GetValue(value));
            Assert.AreEqual(expectedResult.PageIndex, pageIndexProperty.GetValue(value));
        }
    }
}