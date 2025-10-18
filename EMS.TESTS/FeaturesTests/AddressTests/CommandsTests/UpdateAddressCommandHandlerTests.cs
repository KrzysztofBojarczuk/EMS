using EMS.APPLICATION.Features.Address.Commands;
using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EMS.TESTS.Features.AddressTests.CommandsTests
{
    [TestClass]
    public class UpdateAddressCommandHandlerTests
    {
        private Mock<IAddressRepository> _mockAddressRepository;
        private UpdateAddressCommandHandler _handler;

        [TestInitialize]
        public void Setup()
        {
            _mockAddressRepository = new Mock<IAddressRepository>();
            _handler = new UpdateAddressCommandHandler(_mockAddressRepository.Object);
        }

        [TestMethod]
        public async Task Handle_UpdateAddress_Returns_UpdatedAddress()
        {
            // Arrange
            var addressId = Guid.NewGuid();
            var appUserId = "user-id-123";
            var updatedAddress = new AddressEntity
            {
                City = "Updated City",
                Street = "Updated Street",
                Number = "456",
                ZipCode = "11-111",
                AppUserId = appUserId
            };

            _mockAddressRepository.Setup(x => x.UpdateAddressAsync(addressId, appUserId, updatedAddress))
                .ReturnsAsync(updatedAddress);

            var command = new UpdateAddressCommand(addressId, appUserId, updatedAddress);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(updatedAddress, result);
            _mockAddressRepository.Verify(x => x.UpdateAddressAsync(addressId, appUserId, updatedAddress), Times.Once);
        }
    }
}