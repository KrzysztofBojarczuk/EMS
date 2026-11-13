using EMS.APPLICATION.Features.Address.Queries;
using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EMS.TESTS.FeaturesTests.AddressTests.QueriesTests
{
    [TestClass]
    public class GetUserAddressForTaskQueryHandlerTests
    {
        private Mock<IAddressRepository> _mockAddressRepository;
        private Mock<UserManager<AppUserEntity>> _mockUserManager;
        private GetUserAddressesForTaskQueryHandler _handler;

        [TestInitialize]
        public void Setup()
        {
            _mockAddressRepository = new Mock<IAddressRepository>();
            var store = new Mock<IUserStore<AppUserEntity>>();
            _mockUserManager = new Mock<UserManager<AppUserEntity>>(store.Object, null, null, null, null, null, null, null, null);
            _handler = new GetUserAddressesForTaskQueryHandler(_mockAddressRepository.Object, _mockUserManager.Object);
        }

        [TestMethod]
        public async Task Handle_Returns_AllAddresses()
        {
            // Arrange
            var userName = "test-user";
            var appUserId = "user-id-123";

            var user = new AppUserEntity { Id = appUserId, UserName = userName };

            var expectedAddresses = new List<AddressEntity>
            {
                new AddressEntity { Id = Guid.NewGuid(), City = "Address 1", Street = "Address", Number = "1", ZipCode = "00-001", AppUserId = appUserId },
                new AddressEntity { Id = Guid.NewGuid(), City = "Address 2", Street = "Address", Number = "1", ZipCode = "00-001", AppUserId = appUserId },
                new AddressEntity { Id = Guid.NewGuid(), City = "Address 3", Street = "Address", Number = "1", ZipCode = "00-001", AppUserId = appUserId }
            };

            _mockUserManager.Setup(x => x.FindByNameAsync(user.UserName)).ReturnsAsync(user);
            _mockAddressRepository.Setup(x => x.GetUserAddressesForTaskAsync(user.Id, null))
                .ReturnsAsync(expectedAddresses);

            var query = new GetUserAddressesForTaskQuery(user.UserName, null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(expectedAddresses.Count, result.Value.Count());
            Assert.AreEqual(expectedAddresses[0].City, result.Value.First().City);
            _mockAddressRepository.Verify(x => x.GetUserAddressesForTaskAsync(user.Id, null), Times.Once);
        }

        [TestMethod]
        public async Task Handle_Returns_Addresses_BySearchTerm()
        {
            // Arrange
            var userName = "test-user";
            var appUserId = "user-id-123";
            var searchTerm = "test";

            var user = new AppUserEntity { Id = appUserId, UserName = userName };

            var expectedAddresses = new List<AddressEntity>
            {
                new AddressEntity { Id = Guid.NewGuid(), City = "Address 1 Test", Street = "Address", Number = "1", ZipCode = "00-001", AppUserId = appUserId },
                new AddressEntity { Id = Guid.NewGuid(), City = "Address 2 Test", Street = "Address", Number = "1", ZipCode = "00-001", AppUserId = appUserId }
            };

            _mockUserManager.Setup(x => x.FindByNameAsync(user.UserName)).ReturnsAsync(user);
            _mockAddressRepository.Setup(x => x.GetUserAddressesForTaskAsync(user.Id, searchTerm))
                .ReturnsAsync(expectedAddresses);

            var query = new GetUserAddressesForTaskQuery(user.UserName, searchTerm);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(expectedAddresses.Count, result.Value.Count());
            Assert.AreEqual(expectedAddresses[0].City, result.Value.First().City);
            _mockAddressRepository.Verify(x => x.GetUserAddressesForTaskAsync(user.Id, searchTerm), Times.Once);
        }

    }
}