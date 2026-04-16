using EMS.APPLICATION.Features.Vehicle.Queries;
using EMS.CORE.Entities;
using EMS.CORE.Enums;
using EMS.CORE.Interfaces;
using EMS.INFRASTRUCTURE.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EMS.TESTS.FeaturesTests.VehicleTests.QueriesTests
{
    [TestClass]
    public class GetUserVehiclesQueryHandlerHandlerTests
    {
        private Mock<IVehicleRepository> _mockVehicleRepository;
        private GetUserVehiclesQueryHandler _handler;

        public GetUserVehiclesQueryHandlerHandlerTests()
        {
            _mockVehicleRepository = new Mock<IVehicleRepository>();
            _handler = new GetUserVehiclesQueryHandler(_mockVehicleRepository.Object);
        }

        [TestMethod]
        public async Task Handle_Returns_AllVehicles()
        {
            // Arrange
            var appUserId = "user-id-123";
            var pageNumber = 1;
            var pageSize = 10;

            var expectedVehicles = new List<VehicleEntity>
            {
                new VehicleEntity { Id = Guid.NewGuid(), Brand = "Vehicle 1", Model = "Vehicle", Name = "Vehicle", RegistrationNumber = "ABC1111", Mileage = 1000, VehicleType = VehicleType.Car, DateOfProduction = new DateTime(2020, 1, 1), InsuranceOcValidUntil = new DateTime(2020, 1, 1), InsuranceOcCost = 1000, TechnicalInspectionValidUntil = new DateTime(2020, 1, 1), IsAvailable = true, AppUserId = appUserId },
                new VehicleEntity { Id = Guid.NewGuid(), Brand = "Vehicle 2", Model = "Vehicle", Name = "Vehicle", RegistrationNumber = "ABC1111", Mileage = 1000, VehicleType = VehicleType.Car, DateOfProduction = new DateTime(2020, 1, 1), InsuranceOcValidUntil = new DateTime(2020, 1, 1), InsuranceOcCost = 1000, TechnicalInspectionValidUntil = new DateTime(2020, 1, 1), IsAvailable = true, AppUserId = appUserId },
                new VehicleEntity { Id = Guid.NewGuid(), Brand = "Vehicle 3", Model = "Vehicle", Name = "Vehicle", RegistrationNumber = "ABC1111", Mileage = 1000, VehicleType = VehicleType.Car, DateOfProduction = new DateTime(2020, 1, 1), InsuranceOcValidUntil = new DateTime(2020, 1, 1), InsuranceOcCost = 1000, TechnicalInspectionValidUntil = new DateTime(2020, 1, 1), IsAvailable = true, AppUserId = appUserId }
            };

            var paginatedList = new PaginatedList<VehicleEntity>(expectedVehicles, expectedVehicles.Count(), pageNumber, pageSize);

            _mockVehicleRepository.Setup(x => x.GetUserVehiclesAsync(appUserId, pageNumber, pageSize, null, null, null, null, null))
                .ReturnsAsync(paginatedList);

            var query = new GetUserVehiclesQuery(appUserId, pageNumber, pageSize, null, null, null, null, null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedVehicles.Count(), result.Items.Count());
            CollectionAssert.AreEqual(expectedVehicles, result.Items.ToList());
            _mockVehicleRepository.Verify(x => x.GetUserVehiclesAsync(appUserId, pageNumber, pageSize, null, null, null, null, null), Times.Once);
        }

        [TestMethod]
        public async Task Handle_Returns_BySearchTerm_Vehicles()
        {
            // Arrange
            var appUserId = "user-id-123";
            var pageNumber = 1;
            var pageSize = 10;
            var searchTerm = "test";

            var expectedVehicles = new List<VehicleEntity>
            {
                new VehicleEntity { Id = Guid.NewGuid(), Brand = "Vehicle 1 Test", Model = "Vehicle", Name = "Vehicle", RegistrationNumber = "ABC1111", Mileage = 1000, VehicleType = VehicleType.Car, DateOfProduction = new DateTime(2020, 1, 1), InsuranceOcValidUntil = new DateTime(2020, 1, 1), InsuranceOcCost = 1000, TechnicalInspectionValidUntil = new DateTime(2020, 1, 1), IsAvailable = true, AppUserId = appUserId },
                new VehicleEntity { Id = Guid.NewGuid(), Brand = "Vehicle 2 Test", Model = "Vehicle", Name = "Vehicle", RegistrationNumber = "ABC1111", Mileage = 1000, VehicleType = VehicleType.Car, DateOfProduction = new DateTime(2020, 1, 1), InsuranceOcValidUntil = new DateTime(2020, 1, 1), InsuranceOcCost = 1000, TechnicalInspectionValidUntil = new DateTime(2020, 1, 1), IsAvailable = true, AppUserId = appUserId },
            };

            var paginatedList = new PaginatedList<VehicleEntity>(expectedVehicles, expectedVehicles.Count(), pageNumber, pageSize);

            _mockVehicleRepository.Setup(x => x.GetUserVehiclesAsync(appUserId, pageNumber, pageSize, searchTerm, null, null, null, null))
                .ReturnsAsync(paginatedList);

            var query = new GetUserVehiclesQuery(appUserId, pageNumber, pageSize, searchTerm, null, null, null, null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedVehicles.Count(), result.Items.Count());
            CollectionAssert.AreEqual(expectedVehicles, result.Items.ToList());
            _mockVehicleRepository.Verify(x => x.GetUserVehiclesAsync(appUserId, pageNumber, pageSize, searchTerm, null, null, null, null), Times.Once);
        }
    }
}