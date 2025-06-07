using EMS.APPLICATION.Features.Account.Queries;
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
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly GetAllUserQueryHandler _handler;

        public GetAllUserQueryHandlerTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _handler = new GetAllUserQueryHandler(_mockUserRepository.Object);
        }

        [TestMethod]
        public async Task Handle_ByNull_Returns_Users()
        {
            // Arrange
            int pageNumber = 1;
            int pageSize = 10;

            var expectedUsers = new List<AppUserEntity>
            {
               new AppUserEntity { UserName = "John", Email = "john@example.com" },
               new AppUserEntity { UserName = "Johnny", Email = "johnny@example.com" }
            };

            var expectedResult = new PaginatedList<AppUserEntity>(expectedUsers, expectedUsers.Count(), pageNumber, pageSize);

            _mockUserRepository.Setup(repo => repo.GettAllUsersAsync(pageNumber, pageSize, null))
                .ReturnsAsync(expectedResult);

            var query = new GetAllUserQuery(pageNumber, pageSize, null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedUsers.Count(), result.Items.Count);
            Assert.AreEqual(expectedUsers[0].Email, result.Items[0].Email);
        }

        [TestMethod]
        public async Task Handle_BySearchTerm_Returns_Users()
        {
            // Arrange
            int pageNumber = 1;
            int pageSize = 10;
            var searchTerm = "John";

            var expectedUsers = new List<AppUserEntity>
            {
               new AppUserEntity { UserName = "John", Email = "john@example.com" },
               new AppUserEntity { UserName = "Johnny", Email = "johnny@example.com" }
            };

            var expectedResult = new PaginatedList<AppUserEntity>(expectedUsers, expectedUsers.Count(), pageNumber, pageSize);

            _mockUserRepository.Setup(repo => repo.GettAllUsersAsync(pageNumber, pageSize, searchTerm))
                .ReturnsAsync(expectedResult);

            var query = new GetAllUserQuery(pageNumber, pageSize, searchTerm);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedUsers.Count(), result.Items.Count());
            Assert.AreEqual(expectedUsers[0].Email, result.Items[0].Email);
        }

        [TestMethod]
        public async Task Handle_Returns_EmptyList_When_Users_NotFound()
        {
            // Arrange
            var query = new GetAllUserQuery(1, 10, "nonexistent");
            var expectedResult = new PaginatedList<AppUserEntity>(new List<AppUserEntity>(), 0, 1, 10);

            _mockUserRepository.Setup(repo =>
                    repo.GettAllUsersAsync(query.pageNumber, query.pageSize, query.searchTerm))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Items.Count);
        }
    }
}