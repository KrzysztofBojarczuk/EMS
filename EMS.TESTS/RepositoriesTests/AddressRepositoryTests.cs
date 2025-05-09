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

        [TestMethod]
        public async Task DeleteAddressAsync_WhenAddressExists_DeletesAddress_AndNullifies_TaskReferences()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var addressId1 = Guid.NewGuid();
            var addressId2 = Guid.NewGuid();

            using (var context = new AppDbContext(options))
            {
                context.Address.Add(new AddressEntity
                {
                    Id = addressId1,
                    City = "Sample City 1",
                    Street = "Sample Street 1",
                    Number = "10",
                    ZipCode = "00-001",
                    AppUserId = "user1"
                });

                context.Address.Add(new AddressEntity
                {
                    Id = addressId2,
                    City = "Sample City 2",
                    Street = "Sample Street 2",
                    Number = "10",
                    ZipCode = "00-001",
                    AppUserId = "user1"
                });

                context.Tasks.Add(new TaskEntity
                {
                    Id = Guid.NewGuid(),
                    Name = "Test Task 2",
                    Description = "Test Description 2", 
                    AppUserId = "user1",              
                    AddressId = addressId1
                });

                context.Tasks.Add(new TaskEntity
                {
                    Id = Guid.NewGuid(),
                    Name = "Test Task 2",
                    Description = "Test Description 2",
                    AppUserId = "user1",
                    AddressId = addressId2
                });

                await context.SaveChangesAsync();
            }

            using (var context = new AppDbContext(options))
            {
                var repository = new AddressRepository(context);

                var addressCountBefore = context.Address.Count();

                // Act
                var result = await repository.DeleteAddressAsync(addressId1);

                var addressCountAfter = context.Address.Count() +1 ;

                // Assert
                Assert.IsTrue(result);
                Assert.AreEqual(addressCountBefore, addressCountAfter);
                Assert.IsNull(context.Address.FirstOrDefault(a => a.Id == addressId1));

                var tasks = context.Tasks.ToList();
                Assert.IsNull(tasks.First().AddressId);
            }
        }
    }
}