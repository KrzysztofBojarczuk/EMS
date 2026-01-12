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

        [TestMethod]
        public async Task GetVehicleByIdAsync_When_VehicleExists_Returns_Vehicle()
        {
            // Arrange
            var vehicleId = Guid.NewGuid();

            var vehicle = new VehicleEntity
            {
                Id = vehicleId,
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

            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetVehicleByIdAsync(vehicleId);

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
        }

        [TestMethod]
        public async Task GetVehicleByIdAsync_When_VehicleDoesNotExist_Returns_Null()
        {
            // Act
            var result = await _repository.GetVehicleByIdAsync(Guid.NewGuid());

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetUserVehiclesAsync_Returns_AllVehicles()
        {
            // Arrange
            var appUserId = "user-id-123";

            var vehicles = new List<VehicleEntity>
            {
                new VehicleEntity { Id = Guid.NewGuid(), Brand = "Vehicle 1", Model = "Vehicle", Name = "Vehicle", RegistrationNumber = "ABC1111", Mileage = 1000, VehicleType = VehicleType.Car, DateOfProduction = new DateTime(2020, 1, 1), InsuranceOcValidUntil = new DateTime(2020, 1, 1),  InsuranceOcCost = 1000, TechnicalInspectionValidUntil = new DateTime(2020, 1, 1), IsAvailable = true, AppUserId = appUserId },
                new VehicleEntity { Id = Guid.NewGuid(), Brand = "Vehicle 2", Model = "Vehicle", Name = "Vehicle", RegistrationNumber = "ABC1111", Mileage = 1000, VehicleType = VehicleType.Car, DateOfProduction = new DateTime(2020, 1, 1), InsuranceOcValidUntil = new DateTime(2020, 1, 1),  InsuranceOcCost = 1000, TechnicalInspectionValidUntil = new DateTime(2020, 1, 1), IsAvailable = true, AppUserId = appUserId },
                new VehicleEntity { Id = Guid.NewGuid(), Brand = "Vehicle 3", Model = "Vehicle", Name = "Vehicle", RegistrationNumber = "ABC1111", Mileage = 1000, VehicleType = VehicleType.Car, DateOfProduction = new DateTime(2020, 1, 1), InsuranceOcValidUntil = new DateTime(2020, 1, 1),  InsuranceOcCost = 1000, TechnicalInspectionValidUntil = new DateTime(2020, 1, 1), IsAvailable = true, AppUserId = appUserId },
            };

            _context.Vehicles.AddRange(vehicles);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetUserVehiclesAsync(appUserId, 1, 10, null, null, null, null, null);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Items.Count());
        }

        [TestMethod]
        public async Task GetUserVehiclesAsync_BySearchTerm_Returns_Vehicles()
        {
            // Arrange
            var appUserId = "user-id-123";
            var searchTerm = "test";

            var vehicles = new List<VehicleEntity>
            {
                new VehicleEntity { Id = Guid.NewGuid(), Brand = "Vehicle 1 Test", Model = "Vehicle", Name = "Vehicle", RegistrationNumber = "ABC1111", Mileage = 1000, VehicleType = VehicleType.Car, DateOfProduction = new DateTime(2020, 1, 1), InsuranceOcValidUntil = new DateTime(2020, 1, 1),  InsuranceOcCost = 1000, TechnicalInspectionValidUntil = new DateTime(2020, 1, 1), IsAvailable = true, AppUserId = appUserId },
                new VehicleEntity { Id = Guid.NewGuid(), Brand = "Vehicle 2", Model = "Vehicle", Name = "Vehicle", RegistrationNumber = "ABC1111", Mileage = 1000, VehicleType = VehicleType.Car, DateOfProduction = new DateTime(2020, 1, 1), InsuranceOcValidUntil = new DateTime(2020, 1, 1),  InsuranceOcCost = 1000, TechnicalInspectionValidUntil = new DateTime(2020, 1, 1), IsAvailable = true, AppUserId = appUserId },
                new VehicleEntity { Id = Guid.NewGuid(), Brand = "Vehicle 3", Model = "Vehicle", Name = "Vehicle", RegistrationNumber = "ABC1111", Mileage = 1000, VehicleType = VehicleType.Car, DateOfProduction = new DateTime(2020, 1, 1), InsuranceOcValidUntil = new DateTime(2020, 1, 1),  InsuranceOcCost = 1000, TechnicalInspectionValidUntil = new DateTime(2020, 1, 1), IsAvailable = true, AppUserId = appUserId },
            };

            _context.Vehicles.AddRange(vehicles);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetUserVehiclesAsync(appUserId, 1, 10, searchTerm, null, null, null, null);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Items.Count());
            Assert.AreEqual(vehicles[0].Brand, result.Items.First().Brand);
        }

        [TestMethod]
        public async Task GetUserVehiclesAsync_When_VehicleDoesNotExist_Returns_EmptyList()
        {
            // Arrange
            var appUserId = "user-id-123";
            var searchTerm = "nonexistent";

            var vehicles = new List<VehicleEntity>
            {
                new VehicleEntity { Id = Guid.NewGuid(), Brand = "Vehicle 1", Model = "Vehicle", Name = "Vehicle", RegistrationNumber = "ABC1111", Mileage = 1000, VehicleType = VehicleType.Car, DateOfProduction = new DateTime(2020, 1, 1), InsuranceOcValidUntil = new DateTime(2020, 1, 1),  InsuranceOcCost = 1000, TechnicalInspectionValidUntil = new DateTime(2020, 1, 1), IsAvailable = true, AppUserId = appUserId },
                new VehicleEntity { Id = Guid.NewGuid(), Brand = "Vehicle 2", Model = "Vehicle", Name = "Vehicle", RegistrationNumber = "ABC1111", Mileage = 1000, VehicleType = VehicleType.Car, DateOfProduction = new DateTime(2020, 1, 1), InsuranceOcValidUntil = new DateTime(2020, 1, 1),  InsuranceOcCost = 1000, TechnicalInspectionValidUntil = new DateTime(2020, 1, 1), IsAvailable = true, AppUserId = appUserId },
                new VehicleEntity { Id = Guid.NewGuid(), Brand = "Vehicle 3", Model = "Vehicle", Name = "Vehicle", RegistrationNumber = "ABC1111", Mileage = 1000, VehicleType = VehicleType.Car, DateOfProduction = new DateTime(2020, 1, 1), InsuranceOcValidUntil = new DateTime(2020, 1, 1),  InsuranceOcCost = 1000, TechnicalInspectionValidUntil = new DateTime(2020, 1, 1), IsAvailable = true, AppUserId = appUserId },
            };

            _context.Vehicles.AddRange(vehicles);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetUserVehiclesAsync(appUserId, 1, 10, searchTerm, null, null, null, null);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Items.Count());
        }

        [TestMethod]
        public async Task GetUserVehiclesAsync_ByVehicleType_Returns_Vehicles()
        {
            // Arrange
            var appUserId = "user-id-123";
            var vehicleType = new List<VehicleType> { VehicleType.Truck };

            var vehicles = new List<VehicleEntity>
            {
                new VehicleEntity { Id = Guid.NewGuid(), Brand = "Vehicle 1", Model = "Vehicle", Name = "Vehicle", RegistrationNumber = "ABC1111", Mileage = 1000, VehicleType = VehicleType.Car, DateOfProduction = new DateTime(2020, 1, 1), InsuranceOcValidUntil = new DateTime(2020, 1, 1),  InsuranceOcCost = 1000, TechnicalInspectionValidUntil = new DateTime(2020, 1, 1), IsAvailable = true, AppUserId = appUserId },
                new VehicleEntity { Id = Guid.NewGuid(), Brand = "Vehicle 2", Model = "Vehicle", Name = "Vehicle", RegistrationNumber = "ABC1111", Mileage = 1000, VehicleType = VehicleType.Car, DateOfProduction = new DateTime(2020, 1, 1), InsuranceOcValidUntil = new DateTime(2020, 1, 1),  InsuranceOcCost = 1000, TechnicalInspectionValidUntil = new DateTime(2020, 1, 1), IsAvailable = true, AppUserId = appUserId },
                new VehicleEntity { Id = Guid.NewGuid(), Brand = "Vehicle 3", Model = "Vehicle", Name = "Vehicle", RegistrationNumber = "ABC1111", Mileage = 1000, VehicleType = VehicleType.Truck, DateOfProduction = new DateTime(2020, 1, 1), InsuranceOcValidUntil = new DateTime(2020, 1, 1),  InsuranceOcCost = 1000, TechnicalInspectionValidUntil = new DateTime(2020, 1, 1), IsAvailable = true, AppUserId = appUserId },
            };

            _context.Vehicles.AddRange(vehicles);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetUserVehiclesAsync(appUserId, 1, 10, null, vehicleType, null, null, null);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Items.Count());
            Assert.AreEqual(vehicles[2].Brand, result.Items.First().Brand);
        }
    }
}