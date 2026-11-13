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
    public class AddAddressCommandHandlerTests
    {
        private Mock<IAddressRepository> _mockAddressRepository;
        private Mock<UserManager<AppUserEntity>> _mockUserManager;
        private Mock<ILogsRepository> _mockLogsRepository;
        private AddAddressCommandHandler _handler;

        [TestInitialize]
        public void Setup()
        {
            _mockAddressRepository = new Mock<IAddressRepository>();
            _mockLogsRepository = new Mock<ILogsRepository>();
            var store = new Mock<IUserStore<AppUserEntity>>();
            _mockUserManager = new Mock<UserManager<AppUserEntity>>(store.Object, null, null, null, null, null, null, null, null);
            _handler = new AddAddressCommandHandler(_mockAddressRepository.Object, _mockUserManager.Object, _mockLogsRepository.Object);
        }

        [TestMethod]
        public async Task Handle_AddAddress_And_Returns_Address()
        {
            // Arrange
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
                ZipCode = "00-001",
            };

            _mockAddressRepository.Setup(x => x.AddAddressAsync(It.IsAny<AddressEntity>())).ReturnsAsync((AddressEntity address) => address);

            var command = new AddAddressCommand(addressDto, user.UserName);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(addressDto.City, result.Value.City);
            Assert.AreEqual(addressDto.Street, result.Value.Street);
            Assert.AreEqual(addressDto.Number, result.Value.Number);
            Assert.AreEqual(addressDto.ZipCode, result.Value.ZipCode);
            Assert.IsTrue(result.IsSuccess);
            _mockAddressRepository.Verify(x => x.AddAddressAsync(It.IsAny<AddressEntity>()), Times.Once);
            _mockUserManager.Verify(x => x.FindByNameAsync(user.UserName), Times.Once);
        }
    }
}