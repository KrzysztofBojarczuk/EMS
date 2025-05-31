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
        private GetUserAddressForTaskQueryHandler _handler;

        [TestInitialize]
        public void Setup()
        {
            _mockAddressRepository = new Mock<IAddressRepository>();
            _handler = new GetUserAddressForTaskQueryHandler(_mockAddressRepository.Object);
        }

        [TestMethod]
        public async Task Handle_Returns_BySearchTerm_Addresses()
        {
            // Arrange
            var appUserId = "user123";
            var searchTerm = "Street";

            var addresses = new List<AddressEntity>
            {
                new AddressEntity { City = "City A", Street = "Test Street", Number = "1", ZipCode = "00-001", AppUserId = appUserId },
                new AddressEntity { City = "City B", Street = "Street Avenue", Number = "2", ZipCode = "00-002", AppUserId = appUserId }
            };

            _mockAddressRepository
                .Setup(repo => repo.GetUserAddressesForTaskAsync(appUserId, searchTerm))
                .ReturnsAsync(addresses);

            var query = new GetUserAddressForTaskQuery(appUserId, searchTerm);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            CollectionAssert.AreEqual(addresses, new List<AddressEntity>(result));
            _mockAddressRepository.Verify(repo => repo.GetUserAddressesForTaskAsync(appUserId, searchTerm), Times.Once);
        }


        [TestMethod]
        public async Task Handle_Returns_EmptyLists_When_Addresses_NotFound()
        {
            // Arrange
            var appUserId = "user123";
            var searchTerm = "NonExistentName";

            var addresses = new List<AddressEntity>
            {
                new AddressEntity { City = "City A", Street = "Test Street", Number = "1", ZipCode = "00-001", AppUserId = appUserId },
                new AddressEntity { City = "City B", Street = "Street Avenue", Number = "2", ZipCode = "00-002", AppUserId = appUserId }
            };

            _mockAddressRepository
                .Setup(repo => repo.GetUserAddressesForTaskAsync(appUserId, searchTerm))
                .ReturnsAsync(addresses.Where(x => x.Street.Contains(searchTerm)).ToList());

            var query = new GetUserAddressForTaskQuery(appUserId, searchTerm);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
            _mockAddressRepository.Verify(repo => repo.GetUserAddressesForTaskAsync(appUserId, searchTerm), Times.Once);
        }
    }
}