using EMS.APPLICATION.Features.Vehicle.Commands;
using EMS.CORE.Entities;
using EMS.CORE.Enums;
using EMS.CORE.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EMS.TESTS.FeaturesTests.VehicleTests.CommandsTests
{
    [TestClass]
    public class AddVehicleCommandHandlerTests
    {
        private Mock<IVehicleRepository> _mockVehicleRepository;
        private AddVehicleCommandHandler _handler;

        [TestInitialize]
        public void Setup()
        {
            _mockVehicleRepository = new Mock<IVehicleRepository>();
            _handler = new AddVehicleCommandHandler(_mockVehicleRepository.Object);
        }

        [TestMethod]
        public async Task Handle_AddVehicle_And_Returns_Vehicle()
        {
            // Arrange
            var expectedVehicle = new VehicleEntity
            {
                Brand = "Vehicle",
                Model = "Vehicle",
                Name = "Vehicle",
                RegistrationNumber = "ABC1111",
                Mileage = 1000,
                VehicleType = VehicleType.Car,
                DateOfProduction = new DateTime(2020, 1, 1),
                InsuranceOcValidUntil = new DateTime(2020, 1, 1),
                InsuranceOcCost = 1000,
                TechnicalInspectionValidUntil = new DateTime(2020, 1, 1),
                IsAvailable = true,
                AppUserId = "user-id-123",
            };

            _mockVehicleRepository.Setup(x => x.AddVehicleAsync(expectedVehicle))
                .ReturnsAsync(expectedVehicle);

            var command = new AddVehicleCommand(expectedVehicle);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedVehicle, result);
            _mockVehicleRepository.Verify(x => x.AddVehicleAsync(expectedVehicle), Times.Once);
        }
    }
}
