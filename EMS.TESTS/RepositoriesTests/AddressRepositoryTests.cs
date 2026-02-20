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
        public async Task AddAddressAsync_Returns_Address()
        {
            // Arrange
            var address = new AddressEntity
            {
                City = "Address",
                Street = "Address",
                Number = "1",
                ZipCode = "00-001",
                AppUserId = "user-id-123"
            };

            // Act
            var result = await _repository.AddAddressAsync(address);

            var addressCount = await _context.Address.CountAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(address.City, result.City);
            Assert.AreEqual(address.Street, result.Street);
            Assert.AreEqual(address.Number, result.Number);
            Assert.AreEqual(address.ZipCode, result.ZipCode);
            Assert.AreEqual(address.AppUserId, result.AppUserId);
            Assert.AreNotEqual(Guid.Empty, result.Id);
            Assert.AreEqual(1, addressCount);
        }

        [TestMethod]
        public async Task GetAddressByIdAsync_When_AddressExists_Returns_Address()
        {
            // Arrange
            var addressId = Guid.NewGuid();

            var address = new AddressEntity
            {
                Id = addressId,
                City = "Address",
                Street = "Address",
                Number = "1",
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
            Assert.AreEqual(address.Number, result.Number);
            Assert.AreEqual(address.ZipCode, result.ZipCode);
            Assert.AreEqual(address.AppUserId, result.AppUserId);
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
        public async Task GetUserAddressesAsync_Returns_AllUserAddresses()
        {
            // Arrange
            var appUserId1 = "user-id-123";
            var appUserId2 = "user-id-1234";

            var addresses = new List<AddressEntity>
            {
                new AddressEntity { Id = Guid.NewGuid(), City = "Address 1", Street = "Address", Number = "1", ZipCode = "00-001", AppUserId = appUserId1 },
                new AddressEntity { Id = Guid.NewGuid(), City = "Address 2", Street = "Address", Number = "1", ZipCode = "00-001", AppUserId = appUserId1 },
                new AddressEntity { Id = Guid.NewGuid(), City = "Address 3", Street = "Address", Number = "1", ZipCode = "00-001", AppUserId = appUserId1 },
                new AddressEntity { Id = Guid.NewGuid(), City = "Address 4", Street = "Address", Number = "1", ZipCode = "00-001", AppUserId = appUserId2 },
                new AddressEntity { Id = Guid.NewGuid(), City = "Address 5", Street = "Address", Number = "1", ZipCode = "00-001", AppUserId = appUserId2 },
            };

            _context.Address.AddRange(addresses);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetUserAddressesAsync(appUserId1, 1, 10, null);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Items.Count());
        }

        [TestMethod]
        public async Task GetUserAddressesAsync_BySearchTerm_Returns_Addresses()
        {
            // Arrange
            var appUserId = "user-id-123";
            var searchTerm = "test";

            var addresses = new List<AddressEntity>
            {
                new AddressEntity { Id = Guid.NewGuid(), City = "Address 1 Test", Street = "Address", Number = "1", ZipCode = "00-001", AppUserId = appUserId},
                new AddressEntity { Id = Guid.NewGuid(), City = "Address 2", Street = "Address", Number = "1", ZipCode = "00-001", AppUserId = appUserId},
                new AddressEntity { Id = Guid.NewGuid(), City = "Address 3", Street = "Address", Number = "1", ZipCode = "00-001", AppUserId = appUserId},
            };

            _context.Address.AddRange(addresses);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetUserAddressesAsync(appUserId, 1, 10, searchTerm);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Items.Count());
            Assert.AreEqual(addresses[0].Street, result.Items.First().Street);
        }

        [TestMethod]
        public async Task GetUserAddressesAsync_When_AddressesDoesNotExist_Returns_EmptyList()
        {
            // Arrange
            var appUserId = "user-id-123";
            var searchTerm = "nonexistent";

            var addresses = new List<AddressEntity>
            {
                new AddressEntity { Id = Guid.NewGuid(), City = "Address 1", Street = "Address", Number = "1", ZipCode = "00-001", AppUserId = appUserId},
                new AddressEntity { Id = Guid.NewGuid(), City = "Address 2", Street = "Address", Number = "1", ZipCode = "00-001", AppUserId = appUserId},
                new AddressEntity { Id = Guid.NewGuid(), City = "Address 3", Street = "Address", Number = "1", ZipCode = "00-001", AppUserId = appUserId},
            };

            _context.Address.AddRange(addresses);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetUserAddressesAsync(appUserId, 1, 10, searchTerm);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Items.Count());
        }

        [TestMethod]
        public async Task GetUserAddressesAsync_When_UserHasNoAddresses_Returns_EmptyList()
        {
            // Arrange
            var appUserId = "user-id-123";

            // Act
            var result = await _repository.GetUserAddressesAsync(appUserId, 1, 10, null);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Items.Count());
        }

        [TestMethod]
        public async Task GetUserAddressesForTaskAsync_Returns_AllAddresses()
        {
            // Arrange
            var appUserId = "user-id-123";

            var addresses = new List<AddressEntity>
            {
                new AddressEntity { Id = Guid.NewGuid(), City = "Address 1", Street = "Address", Number = "1", ZipCode = "00-001", AppUserId = appUserId},
                new AddressEntity { Id = Guid.NewGuid(), City = "Address 2", Street = "Address", Number = "1", ZipCode = "00-001", AppUserId = appUserId},
                new AddressEntity { Id = Guid.NewGuid(), City = "Address 3", Street = "Address", Number = "1", ZipCode = "00-001", AppUserId = appUserId},
                new AddressEntity { Id = Guid.NewGuid(), City = "Address 4", Street = "Address", Number = "1", ZipCode = "00-001", AppUserId = appUserId}
            };

            _context.Address.AddRange(addresses);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetUserAddressesForTaskAsync(appUserId, null);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.Count());
        }

        [TestMethod]
        public async Task GetUserAddressesForTaskAsync_BySearchTerm_Returns_Addresses()
        {
            // Arrange
            var appUserId = "user-id-123";
            var searchTerm = "test";

            var addresses = new List<AddressEntity>
            {
                new AddressEntity { Id = Guid.NewGuid(), City = "Address 1 Test", Street = "Address", Number = "1", ZipCode = "00-001", AppUserId = appUserId },
                new AddressEntity { Id = Guid.NewGuid(), City = "Address 2 Test", Street = "Address", Number = "1", ZipCode = "00-001", AppUserId = appUserId},
                new AddressEntity { Id = Guid.NewGuid(), City = "Address 3", Street = "Address", Number = "1", ZipCode = "00-001", AppUserId = appUserId },
                new AddressEntity { Id = Guid.NewGuid(), City = "Address 4", Street = "Address", Number = "1", ZipCode = "00-001", AppUserId = appUserId }
            };

            _context.Address.AddRange(addresses);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetUserAddressesForTaskAsync(appUserId, searchTerm);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual(addresses[0].Street, result.First().Street);
        }

        [TestMethod]
        public async Task GetUserAddressesForTaskAsync_When_AddressesDoesNotExist_Returns_EmptyList()
        {
            // Arrange
            var appUserId = "user-id-123";
            var searchTerm = "nonexistent";

            var addresses = new List<AddressEntity>
            {
                new AddressEntity { Id = Guid.NewGuid(), City = "Address 1", Street = "Address", Number = "1", ZipCode = "00-001", AppUserId = appUserId },
                new AddressEntity { Id = Guid.NewGuid(), City = "Address 2", Street = "Address", Number = "1", ZipCode = "00-001", AppUserId = appUserId },
                new AddressEntity { Id = Guid.NewGuid(), City = "Address 3", Street = "Address", Number = "1", ZipCode = "00-001", AppUserId = appUserId },
            };

            _context.Address.AddRange(addresses);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetUserAddressesForTaskAsync(appUserId, searchTerm);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public async Task UpdateAddressAsync_When_AddressExists_Returns_Address()
        {
            // Arrange
            var appUserId = "user-id-123";

            var address = new AddressEntity
            {
                Id = Guid.NewGuid(),
                City = "Address",
                Street = "Address",
                Number = "1",
                ZipCode = "00-001",
                AppUserId = appUserId
            };

            _context.Address.Add(address);
            await _context.SaveChangesAsync();

            var updatedAddress = new AddressEntity
            {
                City = "Address Test",
                Street = "Address",
                Number = "1",
                ZipCode = "00-001",
                AppUserId = appUserId
            };

            // Act
            var result = await _repository.UpdateAddressAsync(address.Id, appUserId, updatedAddress);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(address.Id, result.Id);
            Assert.AreEqual(updatedAddress.City, result.City);
            Assert.AreEqual(updatedAddress.Street, result.Street);
            Assert.AreEqual(updatedAddress.Number, result.Number);
            Assert.AreEqual(updatedAddress.ZipCode, result.ZipCode);
        }

        [TestMethod]
        public async Task UpdateAddressAsync_When_AddressDoesNotExist_Returns_Entity()
        {
            // Arrange
            var nonExistentId = Guid.NewGuid();
            var appUserId = "user-id-123";

            var updatedAddress = new AddressEntity
            {
                Id = Guid.NewGuid(),
                City = "Addresss",
                Street = "Address",
                Number = "1",
                ZipCode = "00-001",
                AppUserId = appUserId
            };

            // Act
            var result = await _repository.UpdateAddressAsync(nonExistentId, appUserId, updatedAddress);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(updatedAddress.City, result.City);
            Assert.AreEqual(updatedAddress.Street, result.Street);
            Assert.AreEqual(updatedAddress.Number, result.Number);
            Assert.AreEqual(updatedAddress.ZipCode, result.ZipCode);
        }

        [TestMethod]
        public async Task DeleteAddressAsync_When_AddressExists_Returns_True()
        {
            // Arrange
            var appUserId = "user-id-123";

            var addresses = new List<AddressEntity>
            {
                new AddressEntity { Id = Guid.NewGuid(), City = "Address", Street = "Address", Number = "1", ZipCode = "00-001", AppUserId = appUserId },
                new AddressEntity { Id = Guid.NewGuid(), City = "Address", Street = "Address", Number = "1", ZipCode = "00-001", AppUserId = appUserId }
            };

            var tasks = new List<TaskEntity>
            {
                new TaskEntity { Id = Guid.NewGuid(), Name = "Task", Description = "Task", AppUserId = "user1", AddressId = addresses[0].Id },
                new TaskEntity { Id = Guid.NewGuid(), Name = "Task", Description = "Task", AppUserId = "user1", AddressId = addresses[1].Id }
            };

            _context.Tasks.AddRange(tasks);
            _context.Address.AddRange(addresses);
            await _context.SaveChangesAsync();

            var addressCountBefore = await _context.Address.CountAsync();

            // Act
            var result = await _repository.DeleteAddressAsync(addresses[0].Id, appUserId);

            var deletedAddress = await _context.Address.FirstOrDefaultAsync(x => x.Id == addresses[0].Id);

            var noTaskReferencesDeletedAddress = tasks.All(x => x.AddressId != addresses[0].Id);

            var addressCountAfter = await _context.Address.CountAsync();

            // Assert
            Assert.IsTrue(result);
            Assert.IsNull(deletedAddress);
            Assert.IsTrue(noTaskReferencesDeletedAddress);
            Assert.AreEqual(addressCountBefore - 1, addressCountAfter);
        }

        [TestMethod]
        public async Task DeleteAddressAsync_When_AddressDoesNotExist_Returns_False()
        {
            // Arrange
            var appUserId = "user-id-123";

            // Act
            var result = await _repository.DeleteAddressAsync(Guid.NewGuid(), appUserId);

            // Assert
            Assert.IsFalse(result);
        }
    }
}