using EMS.APPLICATION.Features.Address.Queries;
using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using EMS.INFRASTRUCTURE.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EMS.TESTS.FeaturesTests.AddressTests.QueriesTests
{
    [TestClass]
    public class GetUserAddressQueryHandlerTests
    {
        private Mock<IAddressRepository> _mockAddressRepository;
        private GetUserAddressesQueryHandler _handler;

        [TestInitialize]
        public void Setup()
        {
            _mockAddressRepository = new Mock<IAddressRepository>();
            _handler = new GetUserAddressesQueryHandler(_mockAddressRepository.Object);
        }

        [TestMethod]
        public async Task Handle_Returns_AllAddresses()
        {
            // Arrange
            var appUserId = "user-id-123";
            var pageNumber = 1;
            var pageSize = 10;

            var expectedAddresses = new List<AddressEntity>
            {
                new AddressEntity { Id = Guid.NewGuid(), City = "City A", Street = "Main Street", Number = "1", ZipCode = "00-001", AppUserId = appUserId },
                new AddressEntity { Id = Guid.NewGuid(), City = "City B", Street = "Second Street", Number = "2", ZipCode = "00-002", AppUserId = appUserId },
                new AddressEntity { Id = Guid.NewGuid(), City = "City C", Street = "Street Koszalin", Number = "2", ZipCode = "00-002", AppUserId = appUserId }
            };

            var paginatedList = new PaginatedList<AddressEntity>(expectedAddresses, expectedAddresses.Count(), pageNumber, pageSize);

            _mockAddressRepository.Setup(x => x.GetUserAddressesAsync(appUserId, pageNumber, pageSize, null))
                .ReturnsAsync(paginatedList);

            var query = new GetUserAddressesQuery(appUserId, pageNumber, pageSize, null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedAddresses.Count(), result.Items.Count());
            CollectionAssert.AreEqual(expectedAddresses, result.Items.ToList());
            _mockAddressRepository.Verify(x => x.GetUserAddressesAsync(appUserId, pageNumber, pageSize, null), Times.Once);
        }

        [TestMethod]
        public async Task Handle_Returns_BySearchTerm_Addresses()
        {
            // Arrange
            var appUserId = "user-id-123";
            var pageNumber = 1;
            var pageSize = 10;
            var searchTerm = "Street";

            var expectedAddresses = new List<AddressEntity>
            {
                new AddressEntity { Id = Guid.NewGuid(), City = "City A", Street = "Main Street", Number = "1", ZipCode = "00-001", AppUserId = appUserId },
                new AddressEntity { Id = Guid.NewGuid(), City = "City B", Street = "Second Street", Number = "2", ZipCode = "00-002", AppUserId = appUserId }
            };

            var paginatedList = new PaginatedList<AddressEntity>(expectedAddresses, expectedAddresses.Count(), pageNumber, pageSize);

            _mockAddressRepository.Setup(x => x.GetUserAddressesAsync(appUserId, pageNumber, pageSize, searchTerm))
                .ReturnsAsync(paginatedList);

            var query = new GetUserAddressesQuery(appUserId, pageNumber, pageSize, searchTerm);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedAddresses.Count(), result.Items.Count());
            CollectionAssert.AreEqual(expectedAddresses, result.Items.ToList());
            _mockAddressRepository.Verify(x => x.GetUserAddressesAsync(appUserId, pageNumber, pageSize, searchTerm), Times.Once);
        }

        [TestMethod]
        public async Task Handle_Returns_EmptyList_When_Addresses_NotFound()
        {
            // Arrange
            var appUserId = "user-id-123";
            var pageNumber = 1;
            var pageSize = 10;
            var searchTerm = "nonexistent";

            var paginatedList = new PaginatedList<AddressEntity>(new List<AddressEntity>(), 0, pageNumber, pageSize);

            _mockAddressRepository.Setup(x => x.GetUserAddressesAsync(appUserId, pageNumber, pageSize, searchTerm))
                .ReturnsAsync(paginatedList);

            var query = new GetUserAddressesQuery(appUserId, pageNumber, pageSize, searchTerm);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Items.Count());
            _mockAddressRepository.Verify(x => x.GetUserAddressesAsync(appUserId, pageNumber, pageSize, searchTerm), Times.Once);
        }
    }
}