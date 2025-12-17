using EMS.APPLICATION.Features.Reservation.Queries;
using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using EMS.INFRASTRUCTURE.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EMS.TESTS.FeaturesTests.ReservationTests.QueriesTests
{
    [TestClass]
    public class GetUserResevationsQueryHandlerTests
    {
        private Mock<IReservationRepository> _mockReservationRepository;
        private GetUserReservationsQueryHandler _handler;

        public GetUserResevationsQueryHandlerTests()
        {
            _mockReservationRepository = new Mock<IReservationRepository>();
            _handler = new GetUserReservationsQueryHandler(_mockReservationRepository.Object);
        }

        [TestMethod]
        public async Task Handle_Returns_AllReservations()
        {
            // Arrange
            var appUserId = "user-id-123";
            var pageNumber = 1;
            var pageSize = 10;

            var expectedReservations = new List<ReservationEntity>
            {
                new ReservationEntity { Id = Guid.NewGuid(), Description = "Reservation 1", AppUserId = appUserId, CheckInDate = DateTime.UtcNow.AddDays(1), CheckOutDate = DateTime.UtcNow.AddDays(2) },
                new ReservationEntity { Id = Guid.NewGuid(), Description = "Reservation 2", AppUserId = appUserId, CheckInDate = DateTime.UtcNow.AddDays(3), CheckOutDate = DateTime.UtcNow.AddDays(4) },
                new ReservationEntity { Id = Guid.NewGuid(), Description = "Reservation 3", AppUserId = appUserId, CheckInDate = DateTime.UtcNow.AddDays(5), CheckOutDate = DateTime.UtcNow.AddDays(6) }
            };

            var paginatedList = new PaginatedList<ReservationEntity>(expectedReservations, expectedReservations.Count(), pageNumber, pageSize);

            _mockReservationRepository.Setup(x => x.GetUserReservationsAsync(appUserId, pageNumber, pageSize, null, null))
                .ReturnsAsync(paginatedList);

            var query = new GetUserReservationsQuery(appUserId, pageNumber, pageSize, null, null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedReservations.Count(), result.Items.Count());
            CollectionAssert.AreEqual(expectedReservations, result.Items.ToList());
            _mockReservationRepository.Verify(x => x.GetUserReservationsAsync(appUserId, pageNumber, pageSize, null, null), Times.Once);
        }

        [TestMethod]
        public async Task Handle_Returns_BySearchTerm_Reservations()
        {
            // Arrange
            var appUserId = "user-id-123";
            var pageNumber = 1;
            var pageSize = 10;
            var searchTerm = "test";

            var expectedReservations = new List<ReservationEntity>
            {
                new ReservationEntity { Id = Guid.NewGuid(), Description = "Reservation 1 Test", AppUserId = appUserId, CheckInDate = DateTime.UtcNow.AddDays(1), CheckOutDate = DateTime.UtcNow.AddDays(2) },
                new ReservationEntity { Id = Guid.NewGuid(), Description = "Reservation 2 Test", AppUserId = appUserId, CheckInDate = DateTime.UtcNow.AddDays(3), CheckOutDate = DateTime.UtcNow.AddDays(4) },
                new ReservationEntity { Id = Guid.NewGuid(), Description = "Reservation 3 Test", AppUserId = appUserId, CheckInDate = DateTime.UtcNow.AddDays(5), CheckOutDate = DateTime.UtcNow.AddDays(6) }
            };

            var paginatedList = new PaginatedList<ReservationEntity>(expectedReservations, expectedReservations.Count(), pageNumber, pageSize);

            _mockReservationRepository.Setup(x => x.GetUserReservationsAsync(appUserId, pageNumber, pageSize, searchTerm, null))
                .ReturnsAsync(paginatedList);

            var query = new GetUserReservationsQuery(appUserId, pageNumber, pageSize, searchTerm, null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedReservations.Count(), result.Items.Count());
            CollectionAssert.AreEqual(expectedReservations, result.Items.ToList());
            _mockReservationRepository.Verify(x => x.GetUserReservationsAsync(appUserId, pageNumber, pageSize, searchTerm, null), Times.Once);
        }

        [TestMethod]
        public async Task Handle_Returns_EmptyList_When_Resevations_NotFound()
        {
            // Arrange
            var appUserId = "user-id-123";
            var pageNumber = 1;
            var pageSize = 10;
            var searchTerm = "nonexistent";

            var paginatedList = new PaginatedList<ReservationEntity>(new List<ReservationEntity>(), 0, pageNumber, pageSize);

            _mockReservationRepository.Setup(x => x.GetUserReservationsAsync(appUserId, pageNumber, pageSize, searchTerm, null))
                .ReturnsAsync(paginatedList);

            var query = new GetUserReservationsQuery(appUserId, pageNumber, pageSize, searchTerm, null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Items.Count());
            _mockReservationRepository.Verify(x => x.GetUserReservationsAsync(appUserId, pageNumber, pageSize, searchTerm, null), Times.Once);
        }

        [TestMethod]
        public async Task Handle_Returns_SortedByCheckInDateAscending_Reservations()
        {
            // Arrange
            var appUserId = "user-id-123";
            var pageNumber = 1;
            var pageSize = 10;
            var sortOrder = "start_asc";

            var expectedReservations = new List<ReservationEntity>
            {
                new ReservationEntity { Id = Guid.NewGuid(), Description = "Reservation 1", AppUserId = appUserId, CheckInDate = DateTime.UtcNow.AddDays(1), CheckOutDate = DateTime.UtcNow.AddDays(2) },
                new ReservationEntity { Id = Guid.NewGuid(), Description = "Reservation 2", AppUserId = appUserId, CheckInDate = DateTime.UtcNow.AddDays(3), CheckOutDate = DateTime.UtcNow.AddDays(4) },
                new ReservationEntity { Id = Guid.NewGuid(), Description = "Reservation 3", AppUserId = appUserId, CheckInDate = DateTime.UtcNow.AddDays(5), CheckOutDate = DateTime.UtcNow.AddDays(6) }
            };

            var paginatedList = new PaginatedList<ReservationEntity>(expectedReservations, expectedReservations.Count(), pageNumber, pageSize);

            _mockReservationRepository.Setup(x => x.GetUserReservationsAsync(appUserId, pageNumber, pageSize, null, sortOrder))
                .ReturnsAsync(paginatedList);

            var query = new GetUserReservationsQuery(appUserId, pageNumber, pageSize, null, sortOrder);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedReservations.Count(), result.Items.Count());
            CollectionAssert.AreEqual(expectedReservations, result.Items.ToList());
            _mockReservationRepository.Verify(x => x.GetUserReservationsAsync(appUserId, pageNumber, pageSize, null, sortOrder), Times.Once);
        }

        [TestMethod]
        public async Task Handle_Returns_SortedByCheckInDateDescending_Reservations()
        {
            // Arrange
            var appUserId = "user-id-123";
            var pageNumber = 1;
            var pageSize = 10;
            var sortOrder = "start_desc";

            var expectedReservations = new List<ReservationEntity>
            {
                new ReservationEntity { Id = Guid.NewGuid(), Description = "Reservation 1", AppUserId = appUserId, CheckInDate = DateTime.UtcNow.AddDays(5), CheckOutDate = DateTime.UtcNow.AddDays(6) },
                new ReservationEntity { Id = Guid.NewGuid(), Description = "Reservation 2", AppUserId = appUserId, CheckInDate = DateTime.UtcNow.AddDays(3), CheckOutDate = DateTime.UtcNow.AddDays(4) },
                new ReservationEntity { Id = Guid.NewGuid(), Description = "Reservation 3", AppUserId = appUserId, CheckInDate = DateTime.UtcNow.AddDays(1), CheckOutDate = DateTime.UtcNow.AddDays(2) }
            };

            var paginatedList = new PaginatedList<ReservationEntity>(expectedReservations, expectedReservations.Count(), pageNumber, pageSize);

            _mockReservationRepository.Setup(x => x.GetUserReservationsAsync(appUserId, pageNumber, pageSize, null, sortOrder))
                .ReturnsAsync(paginatedList);

            var query = new GetUserReservationsQuery(appUserId, pageNumber, pageSize, null, sortOrder);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedReservations.Count(), result.Items.Count());
            CollectionAssert.AreEqual(expectedReservations, result.Items.ToList());
            _mockReservationRepository.Verify(x => x.GetUserReservationsAsync(appUserId, pageNumber, pageSize, null, sortOrder), Times.Once);
        }

        [TestMethod]
        public async Task Handle_Returns_SortedByCheckOutDateAscending_Reservations()
        {
            // Arrange
            var appUserId = "user-id-123";
            var pageNumber = 1;
            var pageSize = 10;
            var sortOrder = "end_asc";

            var expectedReservations = new List<ReservationEntity>
            {
                new ReservationEntity { Id = Guid.NewGuid(), Description = "R3", AppUserId = appUserId, CheckOutDate = DateTime.UtcNow.AddDays(1) },
                new ReservationEntity { Id = Guid.NewGuid(), Description = "R2", AppUserId = appUserId, CheckOutDate = DateTime.UtcNow.AddDays(3) },
                new ReservationEntity { Id = Guid.NewGuid(), Description = "R1", AppUserId = appUserId, CheckOutDate = DateTime.UtcNow.AddDays(5) }
            };

            var paginatedList = new PaginatedList<ReservationEntity>(expectedReservations, expectedReservations.Count(), pageNumber, pageSize);

            _mockReservationRepository.Setup(x => x.GetUserReservationsAsync(appUserId, pageNumber, pageSize, null, sortOrder))
                .ReturnsAsync(paginatedList);

            var query = new GetUserReservationsQuery(appUserId, pageNumber, pageSize, null, sortOrder);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedReservations.Count(), result.Items.Count());
            CollectionAssert.AreEqual(expectedReservations, result.Items.ToList());
            _mockReservationRepository.Verify(x => x.GetUserReservationsAsync(appUserId, pageNumber, pageSize, null, sortOrder), Times.Once);
        }
    }
}