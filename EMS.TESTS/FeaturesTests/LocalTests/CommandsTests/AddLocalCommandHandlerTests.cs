using EMS.APPLICATION.Features.Local.Commands;
using EMS.CORE.Entities;
using EMS.INFRASTRUCTURE.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EMS.TESTS.FeaturesTests.LocalTests.CommandsTests
{
    [TestClass]
    public class AddLocalCommandHandlerTests
    {
        private Mock<ILocalRepository> _mockLocalRepository;
        private AddLocalCommandHandler _handler;

        [TestInitialize]
        public void Setup()
        {
            _mockLocalRepository = new Mock<ILocalRepository>();
            _handler = new AddLocalCommandHandler(_mockLocalRepository.Object);
        }

        [TestMethod]
        public async Task Handle_AddLocal_And_Returns_Local()
        {
            // Arrange
            var expectedLocal = new LocalEntity
            {
                Description = "Test",
                LocalNumber = 1,
                Surface = 250.0,
                NeedsRepair = false,
                AppUserId = "user-id-123"
            };

            _mockLocalRepository.Setup(x => x.AddLocalAsync(expectedLocal))
                .ReturnsAsync(expectedLocal);

            var command = new AddLocalCommand(expectedLocal);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedLocal, result);
            _mockLocalRepository.Verify(x => x.AddLocalAsync(expectedLocal), Times.Once);
        }
    }
}