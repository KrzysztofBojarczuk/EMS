using EMS.APPLICATION.Features.Userss.Commands;
using EMS.CORE.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EMS.TESTS.Features.AccountTests.CommandsTests
{
    [TestClass]
    public class DeleteUserCommandHandlerTests
    {
        private Mock<IUserRepository> _mockUserRepository;
        private DeleteUserCommandHandler _handler;

        [TestInitialize]
        public void Setup()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _handler = new DeleteUserCommandHandler(_mockUserRepository.Object);
        }

        [TestMethod]
        public async Task Handle_ShouldReturnTrue_When_UserIsDeletedSuccessfully()
        {
            // Arrange
            var appUserId = "user-id-123";
            var expectedResult = true;

            _mockUserRepository.Setup(x => x.DeleteUserAsync(appUserId))
                .ReturnsAsync(expectedResult);

            var command = new DeleteUserCommand(appUserId);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsTrue(result);
            _mockUserRepository.Verify(x => x.DeleteUserAsync(appUserId), Times.Once);
        }


        [TestMethod]
        public async Task Handle_ShouldReturnFalse_When_UserDeletionFails()
        {
            // Arrange
            var appUserId = "user-id-123";
            var expectedResult = false;

            _mockUserRepository.Setup(x => x.DeleteUserAsync(appUserId))
                .ReturnsAsync(expectedResult);

            var command = new DeleteUserCommand(appUserId);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsFalse(result);
            _mockUserRepository.Verify(x => x.DeleteUserAsync(appUserId), Times.Once);
        }
    }
}