using EMS.APPLICATION.Features.Account.Queries;
using EMS.CORE.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EMS.TESTS.Features.AccountTests.QueriesTests
{
    [TestClass]
    public class GetNumberOfUsersQueryHandlerTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly GetNumberOfUsersQueryHandler _handler;

        public GetNumberOfUsersQueryHandlerTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _handler = new GetNumberOfUsersQueryHandler(_mockUserRepository.Object);
        }

        [TestMethod]
        public async Task Handle_Returns_NumberOfUsers()
        {
            // Arrange
            var expectedUserCount = 5;
            _mockUserRepository.Setup(x => x.GetNumberOfUsersAsync())
                .ReturnsAsync(expectedUserCount);

            var query = new GetNumberOfUsersQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.AreEqual(expectedUserCount, result);
            _mockUserRepository.Verify(x => x.GetNumberOfUsersAsync(), Times.Once);
        }

        [TestMethod]
        public async Task Handle_Returns_Zero_When_NoUsers()
        {
            // Arrange
            _mockUserRepository.Setup(x => x.GetNumberOfUsersAsync())
                .ReturnsAsync(0);

            var query = new GetNumberOfUsersQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.AreEqual(0, result);
            _mockUserRepository.Verify(x => x.GetNumberOfUsersAsync(), Times.Once);
        }
    }
}