using EMS.CORE.Entities;
using EMS.INFRASTRUCTURE.Data;
using EMS.INFRASTRUCTURE.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EMS.TESTS.RepositoriesTests
{
    [TestClass]
    public class LocalRepositoryTests
    {
        private AppDbContext _context;
        private ILocalRepository _repository;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
            _repository = new LocalRepository(_context);
        }

        [TestMethod]
        public async Task GetUserLocalAsync_Returns_AllLocals()
        {
            // Arrange
            var appUserId = "user-id-123";

            var locals = new List<LocalEntity>
            {
                new LocalEntity { Description = "Local 1", LocalNumber = 1, Surface = 100.0, NeedsRepair = false, AppUserId = appUserId },
                new LocalEntity { Description = "Local 2", LocalNumber = 2, Surface = 150.0, NeedsRepair = false, AppUserId = appUserId },
                new LocalEntity { Description = "Local 3", LocalNumber = 3, Surface = 200.0, NeedsRepair = false, AppUserId = appUserId }
            };

            _context.Locals.AddRange(locals);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetUserLocalAsync(appUserId, 1, 10, null);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Items.Count());
        }

        [TestMethod]
        public async Task GetUserLocalAsync_BySearchTerm_Returns_AllLocals()
        {
            // Arrange
            var appUserId = "user-id-123";
            var searchTerm = "test";

            var locals = new List<LocalEntity>
            {
                new LocalEntity { Description = "Local 1 Test", LocalNumber = 4, Surface = 250.0, NeedsRepair = false, AppUserId = appUserId },
                new LocalEntity { Description = "Local 2 Test", LocalNumber = 4, Surface = 250.0, NeedsRepair = false, AppUserId = appUserId },
                new LocalEntity { Description = "Local 3", LocalNumber = 1, Surface = 100.0, NeedsRepair = false, AppUserId = appUserId },
                new LocalEntity { Description = "Local 4", LocalNumber = 2, Surface = 150.0, NeedsRepair = false, AppUserId = appUserId },
                new LocalEntity { Description = "Local 5", LocalNumber = 3, Surface = 200.0, NeedsRepair = false, AppUserId = appUserId },
            };

            _context.Locals.AddRange(locals);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetUserLocalAsync(appUserId, 1, 10, searchTerm);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Items.Count());
            Assert.AreEqual(locals[0].Description, result.Items.First().Description);
        }

        [TestMethod]
        public async Task GetUserLocalAsync_When_LocalDoesNotExist_Returns_EmptyList()
        {
            // Arrange
            var appUserId = "user-id-123";
            var searchTerm = "nonexistent";

            var locals = new List<LocalEntity>
            {
                new LocalEntity { Description = "Local 1", LocalNumber = 1, Surface = 100.0, NeedsRepair = false, AppUserId = appUserId },
                new LocalEntity { Description = "Local 2", LocalNumber = 2, Surface = 150.0, NeedsRepair = false, AppUserId = appUserId },
                new LocalEntity { Description = "Local 3", LocalNumber = 3, Surface = 200.0, NeedsRepair = false, AppUserId = appUserId }
            };

            _context.Locals.AddRange(locals);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetUserLocalAsync(appUserId, 1, 10, searchTerm);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Items.Count());
        }

        [TestMethod]
        public async Task GetLocalByIdAsync_When_LocalExists_Returns_Local()
        {
            // Arrange
            var local = new LocalEntity
            {
                Description = "Test",
                LocalNumber = 1,
                Surface = 100.0,
                NeedsRepair = false,
                AppUserId = "user-id-123"
            };

            _context.Locals.Add(local);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetLocalByIdAsync(local.Id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(local.Description, result.Description);
            Assert.AreEqual(local.LocalNumber, result.LocalNumber);
            Assert.AreEqual(local.Surface, result.Surface);
            Assert.AreEqual(local.NeedsRepair, result.NeedsRepair);
            Assert.AreEqual(local.AppUserId, result.AppUserId);
        }

        [TestMethod]
        public async Task GetLocalByIdAsync_When_LocalDoesNotExist_Returns_Null()
        {
            // Act
            var result = await _repository.GetLocalByIdAsync(Guid.NewGuid());

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task AddLocalAsync_Returns_Local()
        {
            // Arrange
            var local = new LocalEntity
            {
                Description = "Test",
                LocalNumber = 1,
                Surface = 100.0,
                NeedsRepair = false,
                AppUserId = "user-id-123"
            };

            // Act
            var result = await _repository.AddLocalAsync(local);

            // Assert 
            Assert.IsNotNull(result);
            Assert.AreEqual(local.Description, result.Description);
            Assert.AreEqual(local.LocalNumber, result.LocalNumber);
            Assert.AreEqual(local.Surface, result.Surface);
            Assert.AreEqual(local.NeedsRepair, result.NeedsRepair);
            Assert.AreEqual(local.AppUserId, result.AppUserId);
            Assert.AreNotEqual(Guid.Empty, result.Id);
            Assert.AreEqual(1, _context.Locals.Count());
        }

        [TestMethod]
        public async Task UpdateLocalAsync_When_EntityIsNotNullAndExists_UpdatesAnd_Returns_Local()
        {
            // Arrange
            var appUserId = "user-id-123";
            var local = new LocalEntity
            {
                Description = "Test",
                LocalNumber = 1,
                Surface = 100.0,
                NeedsRepair = true,
                AppUserId = appUserId
            };

            _context.Locals.Add(local);
            await _context.SaveChangesAsync();

            var updatedLocal = new LocalEntity
            {
                Description = "Test New",
                LocalNumber = 2,
                Surface = 200.0,
                NeedsRepair = false,
                AppUserId = appUserId
            };

            // Act
            var result = await _repository.UpdateLocalAsync(local.Id, appUserId, updatedLocal);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(local.Id, result.Id);
            Assert.AreEqual(updatedLocal.Description, result.Description);
            Assert.AreEqual(updatedLocal.LocalNumber, result.LocalNumber);
            Assert.AreEqual(updatedLocal.Surface, result.Surface);
            Assert.AreEqual(updatedLocal.NeedsRepair, result.NeedsRepair);
        }

        [TestMethod]
        public async Task UpdateLocalAsync_When_LocalDoesNotExist_Returns_Entity()
        {
            // Arrange
            var nonExistentId = Guid.NewGuid();
            var appUserId = "user-id-123";
            var updatedLocal = new LocalEntity
            {
                Description = "Test New",
                LocalNumber = 2,
                Surface = 200.0,
                NeedsRepair = false,
                AppUserId = appUserId
            };

            // Act
            var result = await _repository.UpdateLocalAsync(nonExistentId, appUserId, updatedLocal);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(updatedLocal.Description, result.Description);
            Assert.AreEqual(updatedLocal.LocalNumber, result.LocalNumber);
            Assert.AreEqual(updatedLocal.Surface, result.Surface);
            Assert.AreEqual(updatedLocal.NeedsRepair, result.NeedsRepair);
        }

        [TestMethod]
        public async Task DeleteLocalAsync_When_LocalExists_Returns_True()
        {
            // Arrange
            var appUserId = "user-id-123";
            var local = new LocalEntity
            {
                Description = "Test",
                LocalNumber = 1,
                Surface = 100.0,
                NeedsRepair = false,
                AppUserId = appUserId
            };

            _context.Locals.Add(local);
            await _context.SaveChangesAsync();

            var localCountBefore = _context.Locals.Count();

            // Act
            var result = await _repository.DeleteLocalAsync(local.Id, appUserId);

            var localCountAfter = _context.Locals.Count();

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(localCountBefore - 1, localCountAfter);
            Assert.AreEqual(0, _context.Locals.Count());
        }

        [TestMethod]
        public async Task DeleteLocalAsync_When_LocalDoesNotExist_Returns_False()
        {
            // Arrange
            var appUserId = "user-id-123";

            // Act
            var result = await _repository.DeleteLocalAsync(Guid.NewGuid(), appUserId);

            // Assert
            Assert.IsFalse(result);
        }
    }
}