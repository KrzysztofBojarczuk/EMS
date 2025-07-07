using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using EMS.INFRASTRUCTURE.Data;
using EMS.INFRASTRUCTURE.Extensions;
using EMS.INFRASTRUCTURE.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EMS.TESTS.RepositoriesTests
{
    [TestClass]
    public class UsersRepositoryTests
    {
        private Mock<UserManager<AppUserEntity>> _userManagerMock;
        private IUserRepository _repository;
        private Mock<IUserRepository> _mockUserRepository;

        [TestInitialize]
        public void Setup()
        {
            var store = new Mock<IUserStore<AppUserEntity>>();
            _userManagerMock = new Mock<UserManager<AppUserEntity>>(
                store.Object, null, null, null, null, null, null, null, null);

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            var context = new AppDbContext(options);
            _repository = new UsersRepository(context, _userManagerMock.Object);

            _mockUserRepository = new Mock<IUserRepository>();
        }

        [TestMethod]
        public async Task DeleteUserAsync_When_UserExists_ReturnsTrue()
        {
            // Arrange
            var userId = "user-id-123";
            var user = new AppUserEntity { Id = userId, UserName = "testuser" };

            _userManagerMock.Setup(x => x.FindByIdAsync(userId))
                .ReturnsAsync(user);

            _userManagerMock.Setup(x => x.DeleteAsync(user))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _repository.DeleteUserAsync(userId);

            // Assert
            Assert.IsTrue(result);
            _userManagerMock.Verify(x => x.DeleteAsync(user), Times.Once);
            var remainingUsers = _userManagerMock.Object.Users.ToList();
            Assert.IsFalse(remainingUsers.Any(u => u.Id == userId));
            Assert.AreEqual(0, remainingUsers.Count);
        }

        [TestMethod]
        public async Task DeleteUserAsync_When_UserDoesNotExist_ReturnsFalse()
        {
            // Arrange
            var userId = "nonexistent";
            _userManagerMock.Setup(x => x.FindByIdAsync(userId))
                .ReturnsAsync((AppUserEntity)null);

            // Act
            var result = await _repository.DeleteUserAsync(userId);

            // Assert
            Assert.IsFalse(result);
            _userManagerMock.Verify(x => x.DeleteAsync(It.IsAny<AppUserEntity>()), Times.Never);
        }

        [TestMethod]
        public async Task GetAllUsersAsync_BySearchTerm_Returns_Users()
        {
            // Arrange
            var searchTerm = "john";

            var users = new List<AppUserEntity>
            {
               new AppUserEntity { UserName = "john", Email = "john@example.com" },
               new AppUserEntity { UserName = "Alice", Email = "alice@example.com" },
               new AppUserEntity { UserName = "Chris", Email = "chris@example.com" }
            };

            var paginatedList = new PaginatedList<AppUserEntity>(
                users.Where(x => x.UserName.ToLower().Contains(searchTerm.ToLower())).ToList(),
                1, 1, 10);

            _mockUserRepository.Setup(x => x.GetAllUsersAsync(1, 10, searchTerm))
                .ReturnsAsync(paginatedList);

            // Act
            var result = await _mockUserRepository.Object.GetAllUsersAsync(1, 10, searchTerm);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Items.Count());
            Assert.AreEqual(users[0].UserName, result.Items.First().UserName);
        }

        [TestMethod]
        public async Task GetAllUsersAsync_NotFound_Returns_EmptyList()
        {
            // Arrange
            var searchTerm = "nonexistent";
            var emptyList = new List<AppUserEntity>();
            var paginatedList = new PaginatedList<AppUserEntity>(emptyList, 0, 1, 10);

            _mockUserRepository.Setup(x => x.GetAllUsersAsync(1, 10, searchTerm))
                .ReturnsAsync(paginatedList);

            // Act
            var result = await _mockUserRepository.Object.GetAllUsersAsync(1, 10, searchTerm);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Items.Count);
        }

        [TestMethod]
        public async Task GetNumberOfUsersAsync_Returns_NumberOfUsers()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new AppDbContext(options);

            await context.Users.AddRangeAsync(new List<AppUserEntity>
            {
               new AppUserEntity { Id = "1", UserName = "Alice" },
               new AppUserEntity { Id = "2", UserName = "Bob" }
            });

            await context.SaveChangesAsync();

            var store = new UserStore<AppUserEntity>(context);
            var userManager = new UserManager<AppUserEntity>(
                store, null, null, null, null, null, null, null, null);

            var repository = new UsersRepository(context, userManager);

            // Act
            var count = await repository.GetNumberOfUsersAsync();

            // Assert
            Assert.IsNotNull(count);
            Assert.AreEqual(2, count);
        }
    }
}