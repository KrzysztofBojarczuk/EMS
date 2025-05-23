using EMS.APPLICATION.Features.Account.Commands;
using EMS.CORE.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EMS.TESTS.CommandTests
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
        public async Task Handle_ShouldReturnTrue_WhenUserIsDeletedSuccessfully()
        {
            // Arrange
            var userId = "user123";
            _mockUserRepository.Setup(repo => repo.DeleteUserAsync(userId)).ReturnsAsync(true);

            var command = new DeleteUserCommand(userId);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsTrue(result);
            _mockUserRepository.Verify(repo => repo.DeleteUserAsync(userId), Times.Once);
        }
    }
}