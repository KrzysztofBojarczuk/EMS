using EMS.CORE.Entities;
using EMS.INFRASTRUCTURE.Data;
using EMS.INFRASTRUCTURE.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EMS.TESTS.RepositoriesTests
{
    [TestClass]
    public class AddressRepositoryTests
    {

        [TestMethod]
        public async Task AddEmployeeAsync_Returns_Employee()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "AddAddressTestDb")
                .Options;

            var address = new AddressEntity
            {
                City = "Test City",
                Street = "Test Street",
                Number = "123",
                ZipCode = "00-001",
                AppUserId = "user123"
            };

            using (var context = new AppDbContext(options))
            {
                var repository = new AddressRepository(context);

                // Act
                var result = await repository.AddAddressAsync(address);

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual("Test City", result.City);
                Assert.AreNotEqual(Guid.Empty, result.Id); 
                Assert.AreEqual(1, context.Address.Count());
            }
        }
    }
}