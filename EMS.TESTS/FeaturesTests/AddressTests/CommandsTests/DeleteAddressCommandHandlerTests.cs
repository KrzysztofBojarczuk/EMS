using EMS.APPLICATION.Features.Address.Commands;
using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EMS.TESTS.Features.AddressTests.CommandsTests
{
    [TestClass]
    public class DeleteAddressCommandHandlerTests
    {
        private Mock<IAddressRepository> _mockAddressRepository;
        private Mock<UserManager<AppUserEntity>> _mockUserManager;
        private Mock<ILogsRepository> _mockLogsRepository;
        private DeleteAddressCommandHandler _handler;

        [TestInitialize]
        public void Setup()
        {
            _mockAddressRepository = new Mock<IAddressRepository>();
            _mockLogsRepository = new Mock<ILogsRepository>();
            var store = new Mock<IUserStore<AppUserEntity>>();
            _mockUserManager = new Mock<UserManager<AppUserEntity>>(store.Object, null, null, null, null, null, null, null, null);
            _handler = new DeleteAddressCommandHandler(_mockAddressRepository.Object, _mockUserManager.Object, _mockLogsRepository.Object);
        }

        [TestMethod]
        public async Task Handle_ShouldReturnTrue_When_AddressIsDeletedSuccessfully()
        {
            // Arrange
            var addressId = Guid.NewGuid();
            var userName = "test-user";
            var appUserId = "user-id-123";
            var expectedResult = true;

            var user = new AppUserEntity
            {
                Id = appUserId,
                UserName = userName
            };

            _mockUserManager.Setup(x => x.FindByNameAsync(userName))
                .ReturnsAsync(user);

            _mockAddressRepository.Setup(x => x.DeleteAddressAsync(addressId, appUserId))
                .ReturnsAsync(expectedResult);

            var command = new DeleteAddressCommand(addressId, userName);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsTrue(result);
            _mockAddressRepository.Verify(x => x.DeleteAddressAsync(addressId, appUserId), Times.Once);
        }

        [TestMethod]
        public async Task Handle_ShouldReturnFalse_When_AddressDeletionFails()
        {
            // Arrange
            var addressId = Guid.NewGuid();
            var userName = "test-user";
            var appUserId = "user-id-123";
            var expectedResult = false;

            var user = new AppUserEntity
            {
                Id = appUserId,
                UserName = userName
            };

            _mockUserManager.Setup(x => x.FindByNameAsync(userName))
                .ReturnsAsync(user);

            _mockAddressRepository.Setup(x => x.DeleteAddressAsync(addressId, appUserId))
                .ReturnsAsync(expectedResult);

            var command = new DeleteAddressCommand(addressId, userName);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsFalse(result);
            _mockAddressRepository.Verify(x => x.DeleteAddressAsync(addressId, appUserId), Times.Once);
        }
    }
}