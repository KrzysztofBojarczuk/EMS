using EMS.APPLICATION.Features.Vehicle.Queries;
using EMS.CORE.Entities;
using EMS.CORE.Enums;
using EMS.CORE.Interfaces;
using EMS.INFRASTRUCTURE.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EMS.TESTS.FeaturesTests.VehicleTests.QueriesTests
{

    [TestClass]
    public class GetUserVehiclesQueryHandlerTests
    {
        private Mock<IVehicleRepository> _mockVehicleRepository;
        private Mock<UserManager<AppUserEntity>> _mockUserManager;
        private GetUserVehiclesQueryHandler _handler;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockVehicleRepository = new Mock<IVehicleRepository>();
            _mockUserManager = new Mock<UserManager<AppUserEntity>>(Mock.Of<IUserStore<AppUserEntity>>(), null, null, null, null, null, null, null, null);
            _handler = new GetUserVehiclesQueryHandler(_mockVehicleRepository.Object, _mockUserManager.Object);
        }


        [TestMethod]
        public async Task Handle_Returns_Vehicles_BySearchTerm()
        {
            // Arrange
            var username = "test-user";
            var appUserId = "user-id-123";
            var pageNumber = 1;
            var pageSize = 10;
            var searchTerm = "test";

            var appUser = new AppUserEntity
            {
                Id = appUserId,
                UserName = username
            };

            var expectedVehicles = new List<VehicleEntity>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Brand = "Vehicle 1 Test",
                    Model = "Model 1",
                    Name = "Vehicle",
                    RegistrationNumber = "ABC1111",
                    Mileage = 1000,
                    VehicleType = VehicleType.Car,
                    DateOfProduction = new DateTime(2020, 1, 1),
                    InsuranceOcValidUntil = new DateTime(2020, 1, 1),
                    InsuranceOcCost = 1000,
                    TechnicalInspectionValidUntil = new DateTime(2020, 1, 1),
                    IsAvailable = true,
                    AppUserId = appUserId
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Brand = "Vehicle 2 Test",
                    Model = "Model 2",
                    Name = "Vehicle",
                    RegistrationNumber = "ABC1112",
                    Mileage = 2000,
                    VehicleType = VehicleType.Car,
                    DateOfProduction = new DateTime(2020, 1, 1),
                    InsuranceOcValidUntil = new DateTime(2020, 1, 1),
                    InsuranceOcCost = 1000,
                    TechnicalInspectionValidUntil = new DateTime(2020, 1, 1),
                    IsAvailable = true,
                    AppUserId = appUserId
                }
            };

            var paginatedList = new PaginatedList<VehicleEntity>(expectedVehicles, expectedVehicles.Count(), pageNumber, pageSize);

            _mockUserManager.Setup(x => x.FindByNameAsync(username))
                .ReturnsAsync(appUser);

            _mockVehicleRepository.Setup(x => x.GetUserVehiclesAsync(appUserId, pageNumber, pageSize, searchTerm, null, null, null, null))
                .ReturnsAsync(paginatedList);

            var query = new GetUserVehiclesQuery(username, pageNumber, pageSize, searchTerm, null, null, null, null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsSuccess);
            Assert.IsFalse(result.IsFailure);
            Assert.IsNull(result.Error);
            Assert.IsNotNull(result.Value);
            Assert.AreEqual(expectedVehicles.Count(), result.Value.Items.Count());
            Assert.AreEqual(expectedVehicles[0].Brand, result.Value.Items[0].Brand);
            Assert.AreEqual(expectedVehicles[1].Brand, result.Value.Items[1].Brand);
            _mockUserManager.Verify(x => x.FindByNameAsync(username), Times.Once);
            _mockVehicleRepository.Verify(x => x.GetUserVehiclesAsync(appUserId, pageNumber, pageSize, searchTerm, null, null, null, null), Times.Once);
        }

        [TestMethod]
        public async Task Handle_ReturnsFailure_WhenUserDoesNotExist()
        {
            // Arrange
            var username = "unknown-user";

            _mockUserManager.Setup(x => x.FindByNameAsync(username))
                .ReturnsAsync((AppUserEntity)null);

            var query = new GetUserVehiclesQuery(username, 1, 10, null, null, null, null, null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsSuccess);
            Assert.IsTrue(result.IsFailure);
            Assert.IsNull(result.Value);
            Assert.AreEqual("User not found.", result.Error);
            _mockUserManager.Verify(x => x.FindByNameAsync(username), Times.Once);
            _mockVehicleRepository.Verify(x => x.GetUserVehiclesAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<List<VehicleType>>(), It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), It.IsAny<string>()), Times.Never);
        }
    }
}