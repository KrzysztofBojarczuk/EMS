using EMS.APPLICATION.Features.Address.Queries;
using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EMS.TESTS.FeaturesTests.AddressTests.QueriesTests
{
    [TestClass]
    public class GetUserAddressForTaskQueryHandlerTests
    {
        private Mock<IAddressRepository> _mockAddressRepository;
        private GetUserAddressesForTaskQueryHandler _handler;

        [TestInitialize]
        public void Setup()
        {
            _mockAddressRepository = new Mock<IAddressRepository>();
            _handler = new GetUserAddressesForTaskQueryHandler(_mockAddressRepository.Object);
        }

        [TestMethod]
        public async Task Handle_Returns_AllAddresses()
        {
            // Arrange
            var appUserId = "user-id-123";

            var expectedAddresses = new List<AddressEntity>
            {
                new AddressEntity { Id = Guid.NewGuid(), City = "Address 1", Street = "Address", Number = "1", ZipCode = "00-001", AppUserId = appUserId },
                new AddressEntity { Id = Guid.NewGuid(), City = "Address 2", Street = "Address", Number = "1", ZipCode = "00-001", AppUserId = appUserId },
                new AddressEntity { Id = Guid.NewGuid(), City = "Address 3", Street = "Address", Number = "1", ZipCode = "00-001", AppUserId = appUserId }
            };

            _mockAddressRepository.Setup(x => x.GetUserAddressesForTaskAsync(appUserId, null))
                .ReturnsAsync(expectedAddresses);

            var query = new GetUserAddressesForTaskQuery(appUserId, null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedAddresses.Count(), result.Count());
            CollectionAssert.AreEqual(expectedAddresses, result.ToList());
            _mockAddressRepository.Verify(x => x.GetUserAddressesForTaskAsync(appUserId, null), Times.Once);
        }

        [TestMethod]
        public async Task Handle_Returns_BySearchTerm_Addresses()
        {
            // Arrange
            var appUserId = "user-id-123";
            var searchTerm = "test";

            var expectedAddresses = new List<AddressEntity>
            {
                new AddressEntity { Id = Guid.NewGuid(), City = "Address 1 Test", Street = "Address", Number = "1", ZipCode = "00-001", AppUserId = appUserId },
                new AddressEntity { Id = Guid.NewGuid(), City = "Address 2 Test", Street = "Address", Number = "1", ZipCode = "00-001", AppUserId = appUserId }
            };

            _mockAddressRepository.Setup(x => x.GetUserAddressesForTaskAsync(appUserId, searchTerm))
                .ReturnsAsync(expectedAddresses);

            var query = new GetUserAddressesForTaskQuery(appUserId, searchTerm);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedAddresses.Count(), result.Count());
            CollectionAssert.AreEqual(expectedAddresses, result.ToList());
            _mockAddressRepository.Verify(x => x.GetUserAddressesForTaskAsync(appUserId, searchTerm), Times.Once);
        }

        [TestMethod]
        public async Task Handle_Returns_EmptyList_When_Addresses_NotFound()
        {
            // Arrange
            var appUserId = "user-id-123";
            var searchTerm = "nonexistent";

            _mockAddressRepository.Setup(x => x.GetUserAddressesForTaskAsync(appUserId, searchTerm))
               .ReturnsAsync(new List<AddressEntity>());

            var query = new GetUserAddressesForTaskQuery(appUserId, searchTerm);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
            _mockAddressRepository.Verify(x => x.GetUserAddressesForTaskAsync(appUserId, searchTerm), Times.Once);
        }
    }
}