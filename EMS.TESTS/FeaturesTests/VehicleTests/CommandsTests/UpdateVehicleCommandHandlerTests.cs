using EMS.APPLICATION.Dtos;
using EMS.APPLICATION.Features.Vehicle.Commands;
using EMS.CORE.Entities;
using EMS.CORE.Enums;
using EMS.CORE.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EMS.TESTS.FeaturesTests.VehicleTests.CommandsTests
{
    [TestClass]
    public class UpdateVehicleCommandHandlerTests
    {
        private Mock<IVehicleRepository> _mockVehicleRepository;
        private Mock<UserManager<AppUserEntity>> _mockUserManager;
        private Mock<ILogsRepository> _mockLogsRepository;
        private UpdateVehicleCommandHandler _handler;

        [TestInitialize]
        public void Setup()
        {
            _mockVehicleRepository = new Mock<IVehicleRepository>();
            _mockLogsRepository = new Mock<ILogsRepository>();
            var store = new Mock<IUserStore<AppUserEntity>>();
            _mockUserManager = new Mock<UserManager<AppUserEntity>>(store.Object, null, null, null, null, null, null, null, null);
            _handler = new UpdateVehicleCommandHandler(_mockVehicleRepository.Object, _mockUserManager.Object, _mockLogsRepository.Object);
        }

        [TestMethod]
        public async Task Handle_UpdateVehicle_Returns_UpdatedVehicle()
        {
            // Arrange
            var vehicleId = Guid.NewGuid();
            var user = new AppUserEntity
            {
                Id = "user-id-123",
                UserName = "test-user"
            };

            _mockUserManager.Setup(x => x.FindByNameAsync(user.UserName)).ReturnsAsync(user);

            var vehicleDto = new VehicleCreateDto
            {
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
                IsAvailable = true
            };

            var updatedVehicle = new VehicleEntity
            {
                Id = vehicleId,
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
                AppUserId = user.Id
            };

            _mockVehicleRepository.Setup(x => x.UpdateVehicleAsync(vehicleId, user.Id, It.Is<VehicleEntity>(x => x.Brand == vehicleDto.Brand && x.Model == vehicleDto.Model && x.Name == vehicleDto.Name && x.RegistrationNumber == vehicleDto.RegistrationNumber && x.Mileage == vehicleDto.Mileage && x.VehicleType == vehicleDto.VehicleType && x.InsuranceOcCost == vehicleDto.InsuranceOcCost && x.IsAvailable == vehicleDto.IsAvailable)))
                .ReturnsAsync(updatedVehicle);

            var command = new UpdateVehicleCommand(vehicleId, user.UserName, vehicleDto);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Value);
            Assert.AreEqual(updatedVehicle.Id, result.Value.Id);
            Assert.AreEqual(updatedVehicle.Brand, result.Value.Brand);
            Assert.AreEqual(updatedVehicle.Model, result.Value.Model);
            Assert.AreEqual(updatedVehicle.Name, result.Value.Name);
            Assert.AreEqual(updatedVehicle.RegistrationNumber, result.Value.RegistrationNumber);
            Assert.AreEqual(updatedVehicle.Mileage, result.Value.Mileage);
            Assert.AreEqual(updatedVehicle.VehicleType, result.Value.VehicleType);
            Assert.AreEqual(updatedVehicle.InsuranceOcCost, result.Value.InsuranceOcCost);
            Assert.AreEqual(updatedVehicle.IsAvailable, result.Value.IsAvailable);
            _mockVehicleRepository.Verify(x => x.UpdateVehicleAsync(vehicleId, user.Id, It.IsAny<VehicleEntity>()), Times.Once);
            _mockUserManager.Verify(x => x.FindByNameAsync(user.UserName), Times.Once);
        }
    }
}