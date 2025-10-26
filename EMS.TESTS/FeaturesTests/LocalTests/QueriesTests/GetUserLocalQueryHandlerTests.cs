using EMS.APPLICATION.Features.Local.Queries;
using EMS.CORE.Entities;
using EMS.INFRASTRUCTURE.Extensions;
using EMS.INFRASTRUCTURE.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EMS.TESTS.FeaturesTests.LocalTests.QueriesTests
{
    [TestClass]
    public class GetUserLocalQueryHandlerTests
    {
        private Mock<ILocalRepository> _mockLocalRepository;
        private GetUserLocalQueryHandler _handler;

        public GetUserLocalQueryHandlerTests()
        {
            _mockLocalRepository = new Mock<ILocalRepository>();
            _handler = new GetUserLocalQueryHandler(_mockLocalRepository.Object);
        }

        [TestMethod]
        public async Task Handle_Returns_AllLocals()
        {
            // Arrange
            var appUserId = "user-id-123";
            var pageNumber = 1;
            var pageSize = 10;

            var expectedLocals = new List<LocalEntity>
            {
                new LocalEntity { Description = "Local 1", LocalNumber = 1, Surface = 100.0, NeedsRepair = false, AppUserId = appUserId },
                new LocalEntity { Description = "Local 2", LocalNumber = 2, Surface = 150.0, NeedsRepair = false, AppUserId = appUserId },
                new LocalEntity { Description = "Local 3", LocalNumber = 3, Surface = 200.0, NeedsRepair = false, AppUserId = appUserId }
            };

            var paginatedList = new PaginatedList<LocalEntity>(expectedLocals, expectedLocals.Count(), pageNumber, pageSize);

            _mockLocalRepository.Setup(x => x.GetUserLocalAsync(appUserId, pageNumber, pageSize, null))
                .ReturnsAsync(paginatedList);

            var query = new GetUserLocalQuery(appUserId, pageNumber, pageSize, null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedLocals.Count(), result.Items.Count());
            CollectionAssert.AreEqual(expectedLocals, result.Items.ToList());
            _mockLocalRepository.Verify(x => x.GetUserLocalAsync(appUserId, pageNumber, pageSize, null), Times.Once);
        }

        [TestMethod]
        public async Task Handle_Returns_BySearchTerm_Locals()
        {
            // Arrange
            var appUserId = "user-id-123";
            var pageNumber = 1;
            var pageSize = 10;
            var searchTerm = "Local";

            var expectedLocals = new List<LocalEntity>
            {
                new LocalEntity { Description = "Local 1", LocalNumber = 1, Surface = 100.0, NeedsRepair = false, AppUserId = appUserId },
                new LocalEntity { Description = "Local 2", LocalNumber = 2, Surface = 150.0, NeedsRepair = false, AppUserId = appUserId },
                new LocalEntity { Description = "Local 3", LocalNumber = 3, Surface = 200.0, NeedsRepair = false, AppUserId = appUserId }
            };

            var paginatedList = new PaginatedList<LocalEntity>(expectedLocals, expectedLocals.Count(), pageNumber, pageSize);

            _mockLocalRepository.Setup(x => x.GetUserLocalAsync(appUserId, pageNumber, pageSize, searchTerm))
                .ReturnsAsync(paginatedList);

            var query = new GetUserLocalQuery(appUserId, pageNumber, pageSize, searchTerm);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedLocals.Count(), result.Items.Count());
            CollectionAssert.AreEqual(expectedLocals, result.Items.ToList());
            _mockLocalRepository.Verify(x => x.GetUserLocalAsync(appUserId, pageNumber, pageSize, searchTerm), Times.Once);
        }

        [TestMethod]
        public async Task Handle_Returns_EmptyList_When_Locals_NotFound()
        {
            // Arrange
            var appUserId = "user-id-123";
            var pageNumber = 1;
            var pageSize = 10;
            var searchTerm = "nonexistent";

            var paginatedList = new PaginatedList<LocalEntity>(new List<LocalEntity>(), 0, pageNumber, pageSize);

            _mockLocalRepository.Setup(x => x.GetUserLocalAsync(appUserId, pageNumber, pageSize, searchTerm))
                .ReturnsAsync(paginatedList);

            var query = new GetUserLocalQuery(appUserId, pageNumber, pageSize, searchTerm);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Items.Count());
            _mockLocalRepository.Verify(x => x.GetUserLocalAsync(appUserId, pageNumber, pageSize, searchTerm), Times.Once);
        }
    }
}