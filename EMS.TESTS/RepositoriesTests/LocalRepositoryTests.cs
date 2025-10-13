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
    }
}