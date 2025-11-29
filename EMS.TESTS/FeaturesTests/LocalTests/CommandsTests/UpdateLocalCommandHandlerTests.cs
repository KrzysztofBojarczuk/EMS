using EMS.APPLICATION.Features.Local.Commands;
using EMS.CORE.Entities;
using EMS.INFRASTRUCTURE.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EMS.TESTS.FeaturesTests.LocalTests.CommandsTests
{
    [TestClass]
    public class UpdateLocalCommandHandlerTests
    {
        private Mock<ILocalRepository> _mockLocalRepository;
        private UpdateLocalCommandHandler _handler;

        [TestInitialize]
        public void Setup()
        {
            _mockLocalRepository = new Mock<ILocalRepository>();
            _handler = new UpdateLocalCommandHandler(_mockLocalRepository.Object);
        }

        [TestMethod]
        public async Task Handle_UpdateLocal_Returns_UpdatedLocal()
        {
            // Arrange
            var localId = Guid.NewGuid();
            var appUserId = "user-id-123";

            var updatedLocal = new LocalEntity
            {
                Description = "Local 1",
                LocalNumber = 1,
                Surface = 250.0,
                NeedsRepair = false,
                AppUserId = appUserId
            };

            _mockLocalRepository.Setup(x => x.UpdateLocalAsync(localId, appUserId, updatedLocal))
                .ReturnsAsync(updatedLocal);

            var command = new UpdateLocalCommand(localId, appUserId, updatedLocal);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(updatedLocal, result);
            _mockLocalRepository.Verify(x => x.UpdateLocalAsync(localId, appUserId, updatedLocal), Times.Once);
        }
    }
}