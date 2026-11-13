using EMS.APPLICATION.Dtos;
using EMS.APPLICATION.Features.Address.Commands;
using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EMS.TESTS.Features.AddressTests.CommandsTests
{
    [TestClass]
    public class UpdateAddressCommandHandlerTests
    {
        private Mock<IAddressRepository> _mockAddressRepository;
        private Mock<UserManager<AppUserEntity>> _mockUserManager;
        private Mock<ILogsRepository> _mockLogsRepository;
        private UpdateAddressCommandHandler _handler;

        [TestInitialize]
        public void Setup()
        {
            _mockAddressRepository = new Mock<IAddressRepository>();
            _mockLogsRepository = new Mock<ILogsRepository>();
            var store = new Mock<IUserStore<AppUserEntity>>();
            _mockUserManager = new Mock<UserManager<AppUserEntity>>(store.Object, null, null, null, null, null, null, null, null);
            _handler = new UpdateAddressCommandHandler(_mockAddressRepository.Object, _mockUserManager.Object, _mockLogsRepository.Object);
        }

        [TestMethod]
        public async Task Handle_UpdateAddress_Returns_UpdatedAddress()
        {
            // Arrange
            var addressId = Guid.NewGuid();

            var user = new AppUserEntity
            {
                Id = "user-id-123",
                UserName = "test-user"
            };

            _mockUserManager.Setup(x => x.FindByNameAsync(user.UserName)).ReturnsAsync(user);

            var addressDto = new AddressCreateDto
            {
                City = "Address",
                Street = "Address",
                Number = "1",
                ZipCode = "00-001"
            };

            var updatedAddress = new AddressEntity
            {
                City = "Address",
                Street = "Address",
                Number = "1",
                ZipCode = "00-001"
            };

            _mockAddressRepository.Setup(x => x.UpdateAddressAsync(addressId, user.Id, It.IsAny<AddressEntity>()))
                .ReturnsAsync(updatedAddress);

            var command = new UpdateAddressCommand(addressId, user.UserName, addressDto);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(updatedAddress.City, result.Value.City);
            Assert.AreEqual(updatedAddress.Street, result.Value.Street);
            Assert.AreEqual(updatedAddress.Number, result.Value.Number);
            Assert.AreEqual(updatedAddress.ZipCode, result.Value.ZipCode);
            _mockAddressRepository.Verify(x => x.UpdateAddressAsync(addressId, user.Id, It.IsAny<AddressEntity>()), Times.Once);
            _mockUserManager.Verify(x => x.FindByNameAsync(user.UserName), Times.Once);
        }
    }
}