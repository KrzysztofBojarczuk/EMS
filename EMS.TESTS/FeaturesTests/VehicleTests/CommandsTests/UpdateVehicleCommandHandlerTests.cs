using EMS.APPLICATION.Features.Vehicle.Commands;
using EMS.CORE.Entities;
using EMS.CORE.Enums;
using EMS.CORE.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EMS.TESTS.FeaturesTests.VehicleTests.CommandsTests
{
    [TestClass]
    public class UpdateVehicleCommandHandlerTests
    {
        private Mock<IVehicleRepository> _mockVehicleRepository;
        private UpdateVehicleCommandHandler _handler;

        [TestInitialize]
        public void Setup()
        {
            _mockVehicleRepository = new Mock<IVehicleRepository>();
            _handler = new UpdateVehicleCommandHandler(_mockVehicleRepository.Object);
        }

        [TestMethod]
        public async Task Handle_UpdateVehicle_Returns_UpdatedVehicle()
        {
            // Arrange
            var vehicleId = Guid.NewGuid();
            var appUserId = "user-id-123";

            var updatedVehicle = new VehicleEntity
            {
                Id = Guid.NewGuid(),
                Brand = "Vehicle",
                Model = "Vehicle",
                Name = "Vehicle",
                RegistrationNumber = "ABC1111",
                Mileage = 2000,
                VehicleType = VehicleType.Car,
                DateOfProduction = new DateTime(2020, 1, 1),
                InsuranceOcValidUntil = new DateTime(2020, 1, 1),
                InsuranceOcCost = 2000,
                TechnicalInspectionValidUntil = new DateTime(2020, 1, 1),
                IsAvailable = true,
                AppUserId = appUserId
            };

            _mockVehicleRepository.Setup(x => x.UpdateVehicleAsync(vehicleId, appUserId, updatedVehicle))
                .ReturnsAsync(updatedVehicle);

            var command = new UpdateVehicleCommand(vehicleId, appUserId, updatedVehicle);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(updatedVehicle, result);
            _mockVehicleRepository.Verify(x => x.UpdateVehicleAsync(vehicleId, appUserId, updatedVehicle), Times.Once);
        }
    }
}