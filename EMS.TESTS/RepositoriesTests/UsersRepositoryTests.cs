using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using EMS.INFRASTRUCTURE.Data;
using EMS.INFRASTRUCTURE.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EMS.TESTS.RepositoriesTests
{
    [TestClass]
    public class UsersRepositoryTests
    {
        private AppDbContext _context;
        private UserManager<AppUserEntity> _userManager;
        private IUserRepository _repository;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
            _userManager = CreateUserManager(_context);
            _repository = new UsersRepository(_context, _userManager);
        }

        [TestMethod]
        public async Task DeleteUserAsync_When_UserExists_Returns_True()
        {
            // Arrange
            var user = new AppUserEntity { UserName = "john", Email = "john@example.com" };
 
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var userCountBefore = await _context.Users.CountAsync();

            // Act
            var result = await _repository.DeleteUserAsync(user.Id);

            var deletedUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == user.Id);

            var userCountAfter = await _context.Users.CountAsync();

            // Assert
            Assert.IsTrue(result);
            Assert.IsNull(deletedUser);
            Assert.AreEqual(userCountBefore -1, userCountAfter);
        }

        [TestMethod]
        public async Task DeleteUserAsync_When_UserDoesNotExist_Returns_False()
        {
            // Arrange
            var appUserId = "nonexistent";

            // Act
            var result = await _repository.DeleteUserAsync(appUserId);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task GetNumberOfUsersAsync_Returns_NumberOfUsers()
        {
            // Arrange
            var users = new List<AppUserEntity>
            {
               new AppUserEntity { UserName = "john", Email = "john@example.com" },
               new AppUserEntity { UserName = "Alice", Email = "alice@example.com" },
               new AppUserEntity { UserName = "Chris", Email = "chris@example.com" }
            };

            _context.Users.AddRange(users);
            await _context.SaveChangesAsync();

            // Act
            var count = await _repository.GetNumberOfUsersAsync();

            // Assert
            Assert.IsNotNull(count);
            Assert.AreEqual(3, count);
        }

        [TestMethod]
        public async Task GetAllUsersAsync_Returns_AllUsers()
        {
            // Arrange
            var users = new List<AppUserEntity>
            {
               new AppUserEntity { UserName = "john", Email = "john@example.com" },
               new AppUserEntity { UserName = "Alice", Email = "alice@example.com" },
               new AppUserEntity { UserName = "Chris", Email = "chris@example.com" },
               new AppUserEntity { UserName = "Tom", Email = "tom@example.com" }
            };

            _context.Users.AddRange(users);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetAllUsersAsync(1, 10, null);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.Items.Count());
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

            _context.Users.AddRange(users);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetAllUsersAsync(1, 10, searchTerm);

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

            var users = new List<AppUserEntity>
            {
               new AppUserEntity { UserName = "john", Email = "john@example.com" },
               new AppUserEntity { UserName = "Alice", Email = "alice@example.com" },
               new AppUserEntity { UserName = "Chris", Email = "chris@example.com" }
            };

            _context.Users.AddRange(users);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetAllUsersAsync(1, 10, searchTerm);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Items.Count);
        }

        private static UserManager<AppUserEntity> CreateUserManager(AppDbContext context)
        {
            var store = new UserStore<AppUserEntity>(context);
            return new UserManager<AppUserEntity>(
                store,
                null,
                new PasswordHasher<AppUserEntity>(),
                Array.Empty<IUserValidator<AppUserEntity>>(),
                Array.Empty<IPasswordValidator<AppUserEntity>>(),
                new UpperInvariantLookupNormalizer(),
                new IdentityErrorDescriber(),
                null,
                new Logger<UserManager<AppUserEntity>>(new LoggerFactory())
            );
        }
    }
}