﻿using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using EMS.INFRASTRUCTURE.Data;
using EMS.INFRASTRUCTURE.Extensions;
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
        private AppDbContext _context;
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
              .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
              .Options;


            _context = new AppDbContext(options);
            _repository = new UsersRepository(_context, _userManagerMock.Object);

            _mockUserRepository = new Mock<IUserRepository>();
        }

        [TestMethod]
        public async Task DeleteUserAsync_When_UserExists_Returns_True()
        {
            // Arrange
            var appUserId = "user-id-123";
            var user = new AppUserEntity { Id = appUserId, UserName = "testuser" };

            _userManagerMock.Setup(x => x.FindByIdAsync(appUserId))
                .ReturnsAsync(user);

            _userManagerMock.Setup(x => x.DeleteAsync(user))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _repository.DeleteUserAsync(appUserId);

            // Assert
            Assert.IsTrue(result);
            _userManagerMock.Verify(x => x.DeleteAsync(user), Times.Once);
            var remainingUsers = _userManagerMock.Object.Users.ToList();
            Assert.IsFalse(remainingUsers.Any(u => u.Id == appUserId));
            Assert.AreEqual(0, remainingUsers.Count);
        }

        [TestMethod]
        public async Task DeleteUserAsync_When_UserDoesNotExist_Returns_False()
        {
            // Arrange
            var appUserId = "nonexistent";
            _userManagerMock.Setup(x => x.FindByIdAsync(appUserId))
                .ReturnsAsync((AppUserEntity)null);

            // Act
            var result = await _repository.DeleteUserAsync(appUserId);

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
            var users = new List<AppUserEntity>
            {
               new AppUserEntity { UserName = "john", Email = "john@example.com" },
               new AppUserEntity { UserName = "Alice", Email = "alice@example.com" },
               new AppUserEntity { UserName = "Chris", Email = "chris@example.com" }
            };

            _mockUserRepository.Setup(x => x.GetNumberOfUsersAsync()).ReturnsAsync(users.Count);

            // Act
            var count = await _mockUserRepository.Object.GetNumberOfUsersAsync();

            // Assert
            Assert.IsNotNull(count);
            Assert.AreEqual(3, count);
        }
    }
}