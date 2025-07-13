using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using EMS.INFRASTRUCTURE.Data;
using EMS.INFRASTRUCTURE.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EMS.TESTS.RepositoriesTests
{
    [TestClass]
    public class AddressRepositoryTests
    {
        private AppDbContext _context;
        private IAddressRepository _repository;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
            _repository = new AddressRepository(_context);
        }

        [TestMethod]
        public async Task AddAdressAsync_Returns_Address()
        {
            // Arrange
            var address = new AddressEntity
            {
                City = "Test City",
                Street = "Test Street",
                Number = "123",
                ZipCode = "00-001",
                AppUserId = "user123"
            };

            // Act
            var result = await _repository.AddAddressAsync(address);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(address.City, result.City);
            Assert.AreNotEqual(Guid.Empty, result.Id);
            Assert.AreEqual(1, _context.Address.Count());
        }

        [TestMethod]
        public async Task DeleteAddressAsync_When_AddressExists_DeletesAddress_AndNullifies_TaskReferences()
        {
            // Arrange
            var addressId1 = Guid.NewGuid();
            var addressId2 = Guid.NewGuid();

            var userId = "user-id-123";

            var addresses = new List<AddressEntity>
            {
                new AddressEntity { Id = addressId1, AppUserId = userId, Street = "Main Street", City = "New York", Number = "10A", ZipCode = "10001" },
                new AddressEntity { Id = addressId2, AppUserId = userId, Street = "Second Avenue", City = "Chicago", Number = "22B", ZipCode = "60601" },
            };

            var tasks = new List<TaskEntity>
            {
                new TaskEntity { Id = Guid.NewGuid(), Name = "Test Task 1", Description = "Test Description", AppUserId = "user1", AddressId = addressId1 },
                new TaskEntity { Id = Guid.NewGuid(), Name = "Test Task 2", Description = "Test Description", AppUserId = "user1", AddressId = addressId2 },
            };

            _context.Tasks.AddRange(tasks);
            _context.Address.AddRange(addresses);
            await _context.SaveChangesAsync();

            var addressCountBefore = _context.Address.Count();

            // Act
            var result = await _repository.DeleteAddressAsync(addressId1);

            var addressCountAfter = _context.Address.Count();

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(addressCountBefore - 1, addressCountAfter);
            Assert.IsNull(_context.Address.FirstOrDefault(a => a.Id == addressId1));
            Assert.IsTrue(tasks.All(x => x.AddressId != addressId1));
        }

        [TestMethod]
        public async Task DeleteAddressAsync_When_AddressDoesNotExist_Returns_False()
        {
            // Act
            var result = await _repository.DeleteAddressAsync(Guid.NewGuid());

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task GetAddressByIdAsync_When_AddressExists_Returns_Address()
        {
            // Arrange
            var addressId = Guid.NewGuid();
            var address = new AddressEntity
            {
                Id = addressId,
                City = "Test City",
                Street = "Test Street",
                Number = "123",
                ZipCode = "00-001",
                AppUserId = "user-id-123"
            };

            _context.Address.Add(address);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetAddressByIdAsync(addressId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(addressId, result.Id);
            Assert.AreEqual(address.City, result.City);
            Assert.AreEqual(address.Street, result.Street);
        }

        [TestMethod]
        public async Task GetAddressByIdAsync_When_AddressDoesNotExist_Returns_Null()
        {
            // Act
            var result = await _repository.GetAddressByIdAsync(Guid.NewGuid());

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetUserAddressesAsync_BySearchTerm_Returns_Addresses()
        {
            // Arrange
            var userId = "user-id-123";
            var searchTerm = "main";

            var addresses = new List<AddressEntity>
            {
                new AddressEntity { Id = Guid.NewGuid(), AppUserId = userId, Street = "Main Street", City = "New York", Number = "10A", ZipCode = "10001" },
                new AddressEntity { Id = Guid.NewGuid(), AppUserId = userId, Street = "Second Avenue", City = "Chicago", Number = "22B", ZipCode = "60601" },
                new AddressEntity { Id = Guid.NewGuid(), AppUserId = userId, Street = "Avenu Street", City = "Los Angeles", Number = "99", ZipCode = "90001" }
            };

            _context.Address.AddRange(addresses);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetUserAddressesAsync(userId, 1, 10, searchTerm);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Items.Count);
            Assert.AreEqual(addresses[0].Street, result.Items.First().Street);
        }
    }
}