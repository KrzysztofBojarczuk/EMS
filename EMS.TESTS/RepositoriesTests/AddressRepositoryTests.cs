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
        public async Task AddEmployeeAsync_AddsEmployee_ReturnsEntity()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "AddEmployeeTestDb")
                .Options;

            var employee = new EmployeeEntity
            {
                Name = "Anna Nowak",
                Email = "anna@example.com",
                Phone = "123456789",
                AppUserId = "user123"
            };

            using (var context = new AppDbContext(options))
            {
                var repository = new EmployeeRepository(context);

                // Act
                var result = await repository.AddEmployeeAsync(employee);

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual("Anna Nowak", result.Name);
                Assert.AreNotEqual(Guid.Empty, result.Id); 
                Assert.AreEqual(1, context.Employees.Count());
            }
        }
    }
}