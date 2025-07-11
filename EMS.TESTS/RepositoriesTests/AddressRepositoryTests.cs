using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using EMS.INFRASTRUCTURE.Data;
using EMS.INFRASTRUCTURE.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EMS.TESTS.RepositoriesTests
{
    [TestClass]
    public class AddressRepositoryTests
    {
        private AppDbContext _context;
        private IAddressRepository _repository;
        private Mock<IAddressRepository> _mockAdressRepository;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
            _repository = new AddressRepository(_context);

            _mockAdressRepository = new Mock<IAddressRepository>();
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

            _context.Address.AddRange(
                new AddressEntity
                {
                    Id = addressId1,
                    City = "Sample City 1",
                    Street = "Sample Street 1",
                    Number = "10",
                    ZipCode = "00-001",
                    AppUserId = "user1"
                },
                new AddressEntity
                {
                    Id = addressId2,
                    City = "Sample City 2",
                    Street = "Sample Street 2",
                    Number = "10",
                    ZipCode = "00-001",
                    AppUserId = "user1"
                }
            );

            _context.Tasks.AddRange(
                new TaskEntity
                {
                    Id = Guid.NewGuid(),
                    Name = "Test Task 1",
                    Description = "Test Description",
                    AppUserId = "user1",
                    AddressId = addressId1
                },
                new TaskEntity
                {
                    Id = Guid.NewGuid(),
                    Name = "Test Task 2",
                    Description = "Test Description",
                    AppUserId = "user1",
                    AddressId = addressId2
                }
            );

            await _context.SaveChangesAsync();

            var addressCountBefore = _context.Address.Count();

            // Act
            var result = await _repository.DeleteAddressAsync(addressId1);

            var addressCountAfter = _context.Address.Count();

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(addressCountBefore - 1, addressCountAfter);
            Assert.IsNull(_context.Address.FirstOrDefault(a => a.Id == addressId1));

            var tasks = _context.Tasks.ToList();
            Assert.IsTrue(tasks.All(x => x.AddressId != addressId1));
        }

        [TestMethod]
        public async Task DeleteAddressAsync_When_AddressDoesNotExist_ReturnsFalse()
        {
            // Act
            var result = await _repository.DeleteAddressAsync(Guid.NewGuid());

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task GetAddressByIdAsync_When_AddressExists_ReturnsAddress()
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
                AppUserId = "user123"
            };

            _context.Address.Add(address);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetAddressByIdAsync(addressId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(addressId, result.Id);
            Assert.AreEqual(address.City, result.City);
        }
    }
}