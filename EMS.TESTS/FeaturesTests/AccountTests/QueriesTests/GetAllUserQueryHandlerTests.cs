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
        public async Task Handle_Returns_BySearchTerm_Users()
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

            _mockUserRepository.Setup(x => x.GetAllUsersAsync(pageNumber, pageSize, searchTerm))
                .ReturnsAsync(expectedResult);

            var query = new GetAllUserQuery(pageNumber, pageSize, searchTerm);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedUsers.Count(), result.Items.Count());
            Assert.AreEqual(expectedUsers[0].Email, result.Items[0].Email);
            _mockUserRepository.Verify(x => x.GetAllUsersAsync(pageNumber, pageSize, searchTerm), Times.Once);
        }

        [TestMethod]
        public async Task Handle_Returns_EmptyList_When_Users_NotFound()
        {
            // Arrange
            var pageNumber = 1;
            var pageSize = 10;

            var query = new GetAllUserQuery(pageNumber, pageSize, "nonexistent");
            var expectedResult = new PaginatedList<AppUserEntity>(new List<AppUserEntity>(), 0, pageNumber, pageSize);

            _mockUserRepository.Setup(x => x.GetAllUsersAsync(query.pageNumber, query.pageSize, query.searchTerm))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Items.Count());
            _mockUserRepository.Verify(x => x.GetAllUsersAsync(query.pageNumber, query.pageSize, query.searchTerm), Times.Once);
        }
    }
}