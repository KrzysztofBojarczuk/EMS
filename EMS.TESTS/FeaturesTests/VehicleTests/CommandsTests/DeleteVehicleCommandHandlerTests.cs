using EMS.APPLICATION.Features.Vehicle.Commands;
using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EMS.TESTS.FeaturesTests.VehicleTests.CommandsTests
{
    [TestClass]
    public class DeleteVehicleCommandHandlerTests
    {
        private Mock<IVehicleRepository> _mockVehicleRepository;
        private Mock<UserManager<AppUserEntity>> _mockUserManager;
        private Mock<ILogsRepository> _mockLogsRepository;
        private DeleteVehicleCommandHandler _handler;

        [TestInitialize]
        public void Setup()
        {
            _mockVehicleRepository = new Mock<IVehicleRepository>();
            _mockLogsRepository = new Mock<ILogsRepository>();
            var store = new Mock<IUserStore<AppUserEntity>>();
            _mockUserManager = new Mock<UserManager<AppUserEntity>>(store.Object, null, null, null, null, null, null, null, null);
            _handler = new DeleteVehicleCommandHandler(_mockVehicleRepository.Object, _mockUserManager.Object, _mockLogsRepository.Object);
        }

        [TestMethod]
        public async Task Handle_ShouldReturnTrue_When_VehicleIsDeletedSuccessfully()
        {
            // Arrange
            var vehicleId = Guid.NewGuid();
            var username = "Tomasz";
            var appUserId = "user-id-123";
            var expectedResult = true;

            var appUser = new AppUserEntity
            {
                Id = appUserId,
                UserName = username
            };

            _mockUserManager.Setup(x => x.FindByNameAsync(username))
                .ReturnsAsync(appUser);

            _mockVehicleRepository.Setup(x => x.DeleteVehicleAsync(vehicleId, appUserId))
                .ReturnsAsync(expectedResult);

            var command = new DeleteVehicleCommand(vehicleId, username);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsTrue(result);
            _mockVehicleRepository.Verify(x => x.DeleteVehicleAsync(vehicleId, appUserId), Times.Once);
        }

        [TestMethod]
        public async Task Handle_ShouldReturnFalse_When_VehicleDeletionFails()
        {
            // Arrange
            var vehicleId = Guid.NewGuid();
            var username = "Tomasz";
            var appUserId = "user-id-123";
            var expectedResult = false;

            var appUser = new AppUserEntity
            {
                Id = appUserId,
                UserName = username
            };

            _mockUserManager.Setup(x => x.FindByNameAsync(username))
                .ReturnsAsync(appUser);

            _mockVehicleRepository.Setup(x => x.DeleteVehicleAsync(vehicleId, appUserId))
                .ReturnsAsync(expectedResult);

            var command = new DeleteVehicleCommand(vehicleId, username);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsFalse(result);
            _mockVehicleRepository.Verify(x => x.DeleteVehicleAsync(vehicleId, appUserId), Times.Once);
        }
    }
}