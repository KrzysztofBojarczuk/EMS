using EMS.APPLICATION.Features.Local.Commands;
using EMS.INFRASTRUCTURE.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EMS.TESTS.FeaturesTests.LocalTests.CommandsTests
{
    [TestClass]
    public class DeleteLocalCommandHandlerTests
    {
        private Mock<ILocalRepository> _mockLocalRepository;
        private DeleteLocalCommandHandler _handler;

        [TestInitialize]
        public void Setup()
        {
            _mockLocalRepository = new Mock<ILocalRepository>();
            _handler = new DeleteLocalCommandHandler(_mockLocalRepository.Object);
        }

        [TestMethod]
        public async Task Handle_ShouldReturnTrue_When_LocalIsDeletedSuccessfully()
        {
            // Arrange
            var localId = Guid.NewGuid();
            var expectedResult = true;
            var appUserId = "user-id-123";

            _mockLocalRepository.Setup(x => x.DeleteLocalAsync(localId, appUserId))
                .ReturnsAsync(expectedResult);

            var command = new DeleteLocalCommand(localId, appUserId);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsTrue(result);
            _mockLocalRepository.Verify(x => x.DeleteLocalAsync(localId, appUserId), Times.Once);
        }
    }
}