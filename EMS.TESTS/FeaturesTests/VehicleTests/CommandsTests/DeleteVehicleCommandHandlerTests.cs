using EMS.APPLICATION.Features.Vehicle.Commands;
using EMS.CORE.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EMS.TESTS.FeaturesTests.VehicleTests.CommandsTests
{
    [TestClass]
    public class DeleteVehicleCommandHandlerTests
    {
        private Mock<IVehicleRepository> _mockVehicleRepository;
        private DeleteVehicleCommandHandler _handler;

        [TestInitialize]
        public void Setup()
        {
            _mockVehicleRepository = new Mock<IVehicleRepository>();
            _handler = new DeleteVehicleCommandHandler(_mockVehicleRepository.Object);
        }

        [TestMethod]
        public async Task Handle_ShouldReturnTrue_When_VehicleIsDeletedSuccessfully()
        {
            // Arrange
            var vehicleId = Guid.NewGuid();
            var expectedResult = true;
            var appUserId = "user-id-123";

            _mockVehicleRepository.Setup(x => x.DeleteVehicleAsync(vehicleId, appUserId))
                .ReturnsAsync(expectedResult);

            var command = new DeleteVehicleCommand(vehicleId, appUserId);

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
            var expectedResult = false;
            var appUserId = "user-id-123";

            _mockVehicleRepository.Setup(x => x.DeleteVehicleAsync(vehicleId, appUserId))
                .ReturnsAsync(expectedResult);

            var command = new DeleteVehicleCommand(vehicleId, appUserId);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsFalse(result);
            _mockVehicleRepository.Verify(x => x.DeleteVehicleAsync(vehicleId, appUserId), Times.Once);
        }
    }
}