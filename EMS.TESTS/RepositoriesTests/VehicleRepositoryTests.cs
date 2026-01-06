using EMS.CORE.Entities;
using EMS.CORE.Enums;
using EMS.CORE.Interfaces;
using EMS.INFRASTRUCTURE.Data;
using EMS.INFRASTRUCTURE.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EMS.TESTS.RepositoriesTests
{
    [TestClass]
    public class VehicleRepositoryTests
    {
        private AppDbContext _context;
        private IVehicleRepository _repository;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
            _repository = new VehicleRepository(_context);
        }

        [TestMethod]
        public async Task AddVehicleAsync_Returns_Vehicle()
        {
            // Arrange
            var vehicle = new VehicleEntity
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

            // Act
            var result = await _repository.AddVehicleAsync(vehicle);

            var vehicleCount = await _context.Vehicles.CountAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(vehicle.Brand, result.Brand);
            Assert.AreEqual(vehicle.Model, result.Model);
            Assert.AreEqual(vehicle.Name, result.Name);
            Assert.AreEqual(vehicle.RegistrationNumber, result.RegistrationNumber);
            Assert.AreEqual(vehicle.Mileage, result.Mileage);
            Assert.AreEqual(vehicle.VehicleType, result.VehicleType);
            Assert.AreEqual(vehicle.DateOfProduction, result.DateOfProduction);
            Assert.AreEqual(vehicle.InsuranceOcValidUntil, result.InsuranceOcValidUntil);
            Assert.AreEqual(vehicle.InsuranceOcCost, result.InsuranceOcCost);
            Assert.AreEqual(vehicle.TechnicalInspectionValidUntil, result.TechnicalInspectionValidUntil);
            Assert.AreEqual(vehicle.IsAvailable, result.IsAvailable);
            Assert.AreEqual(vehicle.AppUserId, result.AppUserId);
            Assert.AreNotEqual(Guid.Empty, result.Id);
            Assert.AreEqual(1, vehicleCount);
        }
    }
}