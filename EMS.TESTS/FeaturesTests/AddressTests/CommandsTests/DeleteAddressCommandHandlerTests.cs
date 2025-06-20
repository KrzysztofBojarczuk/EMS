using EMS.APPLICATION.Features.Address.Commands;
using EMS.CORE.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EMS.TESTS.Features.AddressTests.CommandsTests
{
    [TestClass]
    public class DeleteAddressCommandHandlerTests
    {
        private Mock<IAddressRepository> _mockRepository;
        private DeleteAddressCommandHandler _handler;

        [TestInitialize]
        public void Setup()
        {
            _mockRepository = new Mock<IAddressRepository>();
            _handler = new DeleteAddressCommandHandler(_mockRepository.Object);
        }

        [TestMethod]
        public async Task Handle_ShouldReturnTrue_WhenAddressIsDeletedSuccessfully()
        {
            // Arrange
            var addressId = Guid.NewGuid();
            _mockRepository.Setup(repo => repo.DeleteAddressAsync(addressId))
                           .ReturnsAsync(true);

            var command = new DeleteAddressCommand(addressId);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsTrue(result);
            _mockRepository.Verify(repo => repo.DeleteAddressAsync(addressId), Times.Once);
        }

        [TestMethod]
        public async Task Handle_ShouldReturnFalse_WhenAddressDeletionFails()
        {
            // Arrange
            var addressId = Guid.NewGuid();
            _mockRepository.Setup(repo => repo.DeleteAddressAsync(addressId))
                           .ReturnsAsync(false);

            var command = new DeleteAddressCommand(addressId);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsFalse(result);
            _mockRepository.Verify(repo => repo.DeleteAddressAsync(addressId), Times.Once);
        }
    }
}