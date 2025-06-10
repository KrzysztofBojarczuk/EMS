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
        private GetUserAddressQueryHandler _handler;

        [TestInitialize]
        public void Setup()
        {
            _mockAddressRepository = new Mock<IAddressRepository>();
            _handler = new GetUserAddressQueryHandler(_mockAddressRepository.Object);
        }

        [TestMethod]
        public async Task Handle_Returns_BySearchTerm_Addresses()
        {
            // Arrange
            var appUserId = "user123";
            var pageNumber = 1;
            var pageSize = 10;
            var searchTerm = "Street";

            var expectedAddreses = new List<AddressEntity>
            {
                new AddressEntity { City = "City A", Street = "Main Street", Number = "1", ZipCode = "00-001", AppUserId = appUserId },
                new AddressEntity { City = "City B", Street = "Second Street", Number = "2", ZipCode = "00-002", AppUserId = appUserId }
            };

            var paginatedList = new PaginatedList<AddressEntity>(expectedAddreses , expectedAddreses.Count(), pageNumber, pageSize);

            _mockAddressRepository
                .Setup(repo => repo.GetUserAddressesAsync(appUserId, pageNumber, pageSize, searchTerm))
                .ReturnsAsync(paginatedList);

            var query = new GetUserAddressQuery(appUserId, pageNumber, pageSize, searchTerm);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedAddreses.Count(), result.Items.Count);
            CollectionAssert.AreEqual(expectedAddreses, result.Items.ToList());
            _mockAddressRepository.Verify(repo => repo.GetUserAddressesAsync(appUserId, pageNumber, pageSize, searchTerm), Times.Once);
        }
    }
}