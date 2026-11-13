using EMS.APPLICATION.Features.Address.Queries;
using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using EMS.INFRASTRUCTURE.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EMS.TESTS.FeaturesTests.AddressTests.QueriesTests
{
    [TestClass]
    public class GetUserAddressQueryHandlerTests
    {
        private Mock<IAddressRepository> _mockAddressRepository;
        private Mock<UserManager<AppUserEntity>> _mockUserManager;
        private GetUserAddressesQueryHandler _handler;

        [TestInitialize]
        public void Setup()
        {
            _mockAddressRepository = new Mock<IAddressRepository>();
            var store = new Mock<IUserStore<AppUserEntity>>();
            _mockUserManager = new Mock<UserManager<AppUserEntity>>(store.Object, null, null, null, null, null, null, null, null);
            _handler = new GetUserAddressesQueryHandler(_mockAddressRepository.Object, _mockUserManager.Object);
        }

        [TestMethod]
        public async Task Handle_Returns_AllAddresses()
        {
            // Arrange
            var userName = "test-user";
            var appUserId = "user-id-123";
            var pageNumber = 1;
            var pageSize = 10;
            var searchTerm = "test";

            var appUser = new AppUserEntity
            {
                Id = appUserId,
                UserName = userName
            };

            var expectedAddresses = new List<AddressEntity>
            {
                new AddressEntity { Id = Guid.NewGuid(), City = "Address 1", Street = "Address", Number = "1", ZipCode = "00-001", AppUserId = appUserId },
                new AddressEntity { Id = Guid.NewGuid(), City = "Address 2", Street = "Address", Number = "1", ZipCode = "00-001", AppUserId = appUserId },
                new AddressEntity { Id = Guid.NewGuid(), City = "Address 3", Street = "Address", Number = "1", ZipCode = "00-001", AppUserId = appUserId }
            };

            var paginatedList = new PaginatedList<AddressEntity>(expectedAddresses, expectedAddresses.Count(), pageNumber, pageSize);

            _mockUserManager.Setup(x => x.FindByNameAsync(appUser.UserName)).ReturnsAsync(appUser);
            _mockAddressRepository.Setup(x => x.GetUserAddressesAsync(appUser.Id, pageNumber, pageSize, null))
                .ReturnsAsync(paginatedList);

            var query = new GetUserAddressesQuery(appUser.UserName, pageNumber, pageSize, null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(expectedAddresses.Count(), result.Value.Items.Count);
            Assert.AreEqual(expectedAddresses[0].City, result.Value.Items[0].City);
            _mockAddressRepository.Verify(x => x.GetUserAddressesAsync(appUserId, pageNumber, pageSize, null), Times.Once);
        }

        [TestMethod]
        public async Task Handle_Returns_Addresses_BySearchTerm()
        {
            // Arrange
            var userName = "test-user";
            var appUserId = "user-id-123";
            var pageNumber = 1;
            var pageSize = 10;
            var searchTerm = "test";

            var appUser = new AppUserEntity
            {
                Id = appUserId,
                UserName = userName
            };

            var expectedAddresses = new List<AddressEntity>
            {
                new AddressEntity { Id = Guid.NewGuid(), City = "Address 1 Test", Street = "Address", Number = "1", ZipCode = "00-001", AppUserId = appUserId },
                new AddressEntity { Id = Guid.NewGuid(), City = "Address 2 Test", Street = "Address", Number = "1", ZipCode = "00-001", AppUserId = appUserId }
            };

            var paginatedList = new PaginatedList<AddressEntity>(expectedAddresses, expectedAddresses.Count(), pageNumber, pageSize);

            _mockUserManager.Setup(x => x.FindByNameAsync(appUser.UserName)).ReturnsAsync(appUser);
            _mockAddressRepository.Setup(x => x.GetUserAddressesAsync(appUser.Id, pageNumber, pageSize, searchTerm))
                .ReturnsAsync(paginatedList);

            var query = new GetUserAddressesQuery(appUser.UserName, pageNumber, pageSize, searchTerm);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(expectedAddresses.Count(), result.Value.Items.Count);
            Assert.AreEqual(expectedAddresses[0].City, result.Value.Items[0].City);
            _mockAddressRepository.Verify(x => x.GetUserAddressesAsync(appUserId, pageNumber, pageSize, searchTerm), Times.Once);
        }
    }
}