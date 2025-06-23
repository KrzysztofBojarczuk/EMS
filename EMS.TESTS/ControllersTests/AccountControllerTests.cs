using EMS.API.Controllers;
using EMS.APPLICATION.Features.Account.Commands;
using EMS.APPLICATION.Features.Account.Queries;
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
        public async Task DeleteEmployeeAsync_ReturnsOkResult_When_DeletedSuccessfully()
        {
            // Arrange
            var userId = "test-user-id";
            var expectedResult = true;

            _mockSender.Setup(x => x.Send(It.Is<DeleteUserCommand>(x => x.appUserId == userId), It.IsAny<CancellationToken>()))
                       .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.DeleteEmployeeAsync(userId);

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
    }
}