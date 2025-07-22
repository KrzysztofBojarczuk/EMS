using EMS.APPLICATION.Features.Address.Commands;
using EMS.CORE.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EMS.TESTS.Features.AddressTests.CommandsTests
{
    [TestClass]
    public class DeleteAddressCommandHandlerTests
    {
        private Mock<IAddressRepository> _mockAddressRepository;
        private DeleteAddressCommandHandler _handler;

        [TestInitialize]
        public void Setup()
        {
            _mockAddressRepository = new Mock<IAddressRepository>();
            _handler = new DeleteAddressCommandHandler(_mockAddressRepository.Object);
        }

        [TestMethod]
        public async Task Handle_ShouldReturnTrue_When_AddressIsDeletedSuccessfully()
        {
            // Arrange
            var addressId = Guid.NewGuid();
            var expectedResult = true;

            _mockAddressRepository.Setup(x => x.DeleteAddressAsync(addressId))
                .ReturnsAsync(expectedResult);

            var command = new DeleteAddressCommand(addressId);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsTrue(result);
            _mockAddressRepository.Verify(x => x.DeleteAddressAsync(addressId), Times.Once);
        }

        [TestMethod]
        public async Task Handle_ShouldReturnFalse_When_AddressDeletionFails()
        {
            // Arrange
            var addressId = Guid.NewGuid();
            var expectedResult = false;

            _mockAddressRepository.Setup(x => x.DeleteAddressAsync(addressId))
                .ReturnsAsync(expectedResult);

            var command = new DeleteAddressCommand(addressId);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsFalse(result);
            _mockAddressRepository.Verify(x => x.DeleteAddressAsync(addressId), Times.Once);
        }
    }
}