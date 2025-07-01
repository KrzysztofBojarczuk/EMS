using EMS.CORE.Entities;
using EMS.INFRASTRUCTURE.Data;
using EMS.INFRASTRUCTURE.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EMS.TESTS.RepositoriesTests
{
    [TestClass]
    public class UsersRepositoryTests
    {
        private UsersRepository _repository;
        private Mock<UserManager<AppUserEntity>> _userManagerMock;

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
        }

        [TestMethod]
        public async Task DeleteUserAsync_When_UserExists_ReturnsTrue()
        {
            // Arrange
            var userId = "user123";
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
    }
}