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
    public class AddVehicleCommandHandlerTests
    {
        private Mock<IVehicleRepository> _mockVehicleRepository;
        private Mock<UserManager<AppUserEntity>> _mockUserManager;
        private Mock<ILogsRepository> _mockLogsRepository;
        private AddVehicleCommandHandler _handler;

        [TestInitialize]
        public void Setup()
        {
            _mockVehicleRepository = new Mock<IVehicleRepository>();
            _mockLogsRepository = new Mock<ILogsRepository>();

            var store = new Mock<IUserStore<AppUserEntity>>();
            _mockUserManager = new Mock<UserManager<AppUserEntity>>(store.Object, null, null, null, null, null, null, null, null);

            _handler = new AddVehicleCommandHandler(_mockVehicleRepository.Object, _mockUserManager.Object, _mockLogsRepository.Object);
        }

        [TestMethod]
        public async Task Handle_AddVehicle_Returns_VehicleGetDto()
        {
            // Arrange
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
                Mileage = 1000,
                VehicleType = VehicleType.Car,
                DateOfProduction = new DateTime(2020, 1, 1),
                InsuranceOcValidUntil = new DateTime(2020, 1, 1),
                InsuranceOcCost = 1000,
                TechnicalInspectionValidUntil = new DateTime(2020, 1, 1)
            };

            VehicleEntity savedVehicle = null;

            _mockVehicleRepository.Setup(x => x.AddVehicleAsync(It.IsAny<VehicleEntity>())).Callback<VehicleEntity>(x => savedVehicle = x).ReturnsAsync((VehicleEntity x) => x);

            var command = new AddVehicleCommand(vehicleDto, user.UserName);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(vehicleDto.Brand, result.Value.Brand);
            Assert.AreEqual(vehicleDto.Model, result.Value.Model);
            Assert.AreEqual(vehicleDto.Name, result.Value.Name);
            Assert.AreEqual(vehicleDto.RegistrationNumber, result.Value.RegistrationNumber);
            Assert.AreEqual(vehicleDto.Mileage, result.Value.Mileage);
            Assert.AreEqual(vehicleDto.VehicleType, result.Value.VehicleType);
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(user.Id, savedVehicle.AppUserId);
            _mockVehicleRepository.Verify(x => x.AddVehicleAsync(It.IsAny<VehicleEntity>()), Times.Once);
            _mockUserManager.Verify(x => x.FindByNameAsync(user.UserName), Times.Once);
        }
    }
}
