using EMS.APPLICATION.Features.Userss.Queries;
using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using EMS.INFRASTRUCTURE.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EMS.TESTS.Features.AccountTests.QueriesTests
{
    [TestClass]
    public class GetAllUserQueryHandlerTests
    {
        private Mock<IUserRepository> _mockUserRepository;
        private GetAllUserQueryHandler _handler;

        public GetAllUserQueryHandlerTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _handler = new GetAllUserQueryHandler(_mockUserRepository.Object);
        }

        [TestMethod]
        public async Task Handle_Returns_AllUsers()
        {
            // Arrange
            var pageNumber = 1;
            var pageSize = 10;

            var expectedUsers = new List<AppUserEntity>
            {
                new AppUserEntity { UserName = "User 1", Email = "user1@example.com" },
                new AppUserEntity { UserName = "User 2", Email = "user2@example.com" },
                new AppUserEntity { UserName = "User 3", Email = "user3@example.com" }
            };

            var paginatedList = new PaginatedList<AppUserEntity>(expectedUsers, expectedUsers.Count(), pageNumber, pageSize);

            _mockUserRepository.Setup(x => x.GetAllUsersAsync(pageNumber, pageSize, null))
                .ReturnsAsync(paginatedList);

            var query = new GetAllUserQuery(pageNumber, pageSize, null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedUsers.Count(), result.Items.Count());
            CollectionAssert.AreEqual(expectedUsers, result.Items.ToList());
            _mockUserRepository.Verify(x => x.GetAllUsersAsync(pageNumber, pageSize, null), Times.Once);
        }

        [TestMethod]
        public async Task Handle_Returns_BySearchTerm_Users()
        {
            // Arrange
            var pageNumber = 1;
            var pageSize = 10;
            var searchTerm = "test";

            var expectedUsers = new List<AppUserEntity>
            {
                new AppUserEntity { UserName = "User 1 Test", Email = "user1@example.com" },
                new AppUserEntity { UserName = "User 2 Test", Email = "user2@example.com" }
            };

            var paginatedList = new PaginatedList<AppUserEntity>(expectedUsers, expectedUsers.Count(), pageNumber, pageSize);

            _mockUserRepository.Setup(x => x.GetAllUsersAsync(pageNumber, pageSize, searchTerm))
                .ReturnsAsync(paginatedList);

            var query = new GetAllUserQuery(pageNumber, pageSize, searchTerm);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedUsers.Count(), result.Items.Count());
            CollectionAssert.AreEqual(expectedUsers, result.Items.ToList());
            _mockUserRepository.Verify(x => x.GetAllUsersAsync(pageNumber, pageSize, searchTerm), Times.Once);
        }

        [TestMethod]
        public async Task Handle_Returns_EmptyList_When_Users_NotFound()
        {
            // Arrange
            var pageNumber = 1;
            var pageSize = 10;
            var searchTerm = "nonexistent";

            var paginatedList = new PaginatedList<AppUserEntity>(new List<AppUserEntity>(), 0, pageNumber, pageSize);

            _mockUserRepository.Setup(x => x.GetAllUsersAsync(pageNumber, pageSize, searchTerm))
                .ReturnsAsync(paginatedList);

            var query = new GetAllUserQuery(pageNumber, pageSize, searchTerm);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Items.Count());
            _mockUserRepository.Verify(x => x.GetAllUsersAsync(query.pageNumber, query.pageSize, query.searchTerm), Times.Once);
        }
    }
}