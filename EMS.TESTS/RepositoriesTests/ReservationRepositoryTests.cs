using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using EMS.INFRASTRUCTURE.Data;
using EMS.INFRASTRUCTURE.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EMS.TESTS.RepositoriesTests
{
    [TestClass]
    public class ReservationRepositoryTests
    {
        private AppDbContext _context;
        private IReservationRepository _repository;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
            _repository = new ReservationRepository(_context);
        }

        [TestMethod]
        public async Task AddReservationAsync_Returns_Reservation()
        {
            // Arrange
            var reservation = new ReservationEntity
            {
                Description = "Reservation",
                CheckInDate = new DateTime(2026, 1, 15, 10, 0, 0),
                CheckOutDate = new DateTime(2026, 1, 17, 10, 0, 0),
                LocalId = Guid.NewGuid(),
                AppUserId = "user-id-123"
            };

            // Act
            var result = await _repository.AddReservationAsync(reservation);

            var reservationCount = await _context.Reservations.CountAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(reservation.LocalId, result.LocalId);
            Assert.AreEqual(reservation.CheckInDate, result.CheckInDate);
            Assert.AreEqual(reservation.CheckOutDate, result.CheckOutDate);
            Assert.AreEqual(reservation.AppUserId, result.AppUserId);
            Assert.AreNotEqual(Guid.Empty, result.Id);
            Assert.AreEqual(1, reservationCount);
        }

        [TestMethod]
        public async Task GetReservationByIdAsync_When_ReservationExists_Returns_Reservation()
        {
            // Arrange
            var reservationId = Guid.NewGuid();

            var reservation = new ReservationEntity
            {
                Id = reservationId,
                Description = "Reservation",
                CheckInDate = new DateTime(2026, 1, 15, 10, 0, 0),
                CheckOutDate = new DateTime(2026, 1, 18, 10, 0, 0),
                LocalId = Guid.NewGuid(),
                AppUserId = "user-id-123"
            };

            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetReservationByIdAsync(reservationId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(reservationId, result.Id);
            Assert.AreEqual(reservation.LocalId, result.LocalId);
            Assert.AreEqual(reservation.CheckInDate, result.CheckInDate);
            Assert.AreEqual(reservation.CheckOutDate, result.CheckOutDate);
            Assert.AreEqual(reservation.AppUserId, result.AppUserId);
        }

        [TestMethod]
        public async Task GetReservationByIdAsync_When_ReservationDoesNotExist_Returns_Null()
        {
            // Act
            var result = await _repository.GetReservationByIdAsync(Guid.NewGuid());

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetUserReservationsAsync_Returns_AllReservations()
        {
            // Arrange
            var appUserId1 = "user-id-123";
            var appUserId2 = "user-id-1234";

            var reservations = new List<ReservationEntity>
            {
                new ReservationEntity { Id = Guid.NewGuid(), Description = "Reservation 1", CheckInDate = new DateTime(2026, 1, 16, 10, 0, 0), CheckOutDate = new DateTime(2026, 1, 17, 10, 0, 0), AppUserId = appUserId1 },
                new ReservationEntity { Id = Guid.NewGuid(), Description = "Reservation 2", CheckInDate = new DateTime(2026, 1, 18, 10, 0, 0), CheckOutDate = new DateTime(2026, 1, 19, 10, 0, 0), AppUserId = appUserId1 },
                new ReservationEntity { Id = Guid.NewGuid(), Description = "Reservation 3", CheckInDate = new DateTime(2026, 1, 20, 10, 0, 0), CheckOutDate = new DateTime(2026, 1, 21, 10, 0, 0), AppUserId = appUserId1 },
                new ReservationEntity { Id = Guid.NewGuid(), Description = "Reservation 4", CheckInDate = new DateTime(2026, 1, 20, 10, 0, 0), CheckOutDate = new DateTime(2026, 1, 21, 10, 0, 0), AppUserId = appUserId2 },
                new ReservationEntity { Id = Guid.NewGuid(), Description = "Reservation 5", CheckInDate = new DateTime(2026, 1, 20, 10, 0, 0), CheckOutDate = new DateTime(2026, 1, 21, 10, 0, 0), AppUserId = appUserId2 },
            };

            _context.Reservations.AddRange(reservations);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetUserReservationsAsync(appUserId1, 1, 10, null, null);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Items.Count());
        }

        [TestMethod]
        public async Task GetUserReservationsAsync_BySearchTerm_Returns_Reservations()
        {
            // Arrange
            var appUserId = "user-id-123";
            var searchTerm = "test";

            var reservations = new List<ReservationEntity>
            {
                new ReservationEntity { Id = Guid.NewGuid(), Description = "Reservation 1 Test", CheckInDate = new DateTime(2026, 1, 16, 10, 0, 0), CheckOutDate = new DateTime(2026, 1, 17, 10, 0, 0), AppUserId = appUserId },
                new ReservationEntity { Id = Guid.NewGuid(), Description = "Reservation 2", CheckInDate = new DateTime(2026, 1, 18, 10, 0, 0), CheckOutDate = new DateTime(2026, 1, 19, 10, 0, 0), AppUserId = appUserId },
                new ReservationEntity { Id = Guid.NewGuid(), Description = "Reservation 3", CheckInDate = new DateTime(2026, 1, 20, 10, 0, 0), CheckOutDate = new DateTime(2026, 1, 21, 10, 0, 0), AppUserId = appUserId }
            };

            _context.Reservations.AddRange(reservations);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetUserReservationsAsync(appUserId, 1, 10, searchTerm, null);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Items.Count());
            Assert.AreEqual(reservations[0].Description, result.Items.First().Description);
        }

        [TestMethod]
        public async Task GetUserReservationsAsync_When_ReservationDoesNotExist_Returns_EmptyList()
        {
            // Arrange
            var appUserId = "user-id-123";
            var searchTerm = "nonexistent";

            var reservations = new List<ReservationEntity>
            {
                new ReservationEntity { Id = Guid.NewGuid(), Description = "Reservation 1", CheckInDate = new DateTime(2026, 1, 16, 10, 0, 0), CheckOutDate = new DateTime(2026, 1, 17, 10, 0, 0), AppUserId = appUserId },
                new ReservationEntity { Id = Guid.NewGuid(), Description = "Reservation 2", CheckInDate = new DateTime(2026, 1, 18, 10, 0, 0), CheckOutDate = new DateTime(2026, 1, 19, 10, 0, 0), AppUserId = appUserId },
                new ReservationEntity { Id = Guid.NewGuid(), Description = "Reservation 3", CheckInDate = new DateTime(2026, 1, 20, 10, 0, 0), CheckOutDate = new DateTime(2026, 1, 21, 10, 0, 0), AppUserId = appUserId }
            };

            _context.Reservations.AddRange(reservations);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetUserReservationsAsync(appUserId, 1, 10, searchTerm, null);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Items.Count());
        }

        [TestMethod]
        public async Task GetUserReservationsAsync_SortedByCheckInDateAscending_Returns_SortedReservations()
        {
            // Arrange
            var appUserId = "user-id-123";
            var sortOrder = "start_asc";

            var reservations = new List<ReservationEntity>
            {
                new ReservationEntity { Id = Guid.NewGuid(), Description = "Reservation 1", CheckInDate = new DateTime(2026, 1, 14, 10, 0, 0), CheckOutDate = new DateTime(2026, 1, 15, 10, 0, 0), AppUserId = appUserId },
                new ReservationEntity { Id = Guid.NewGuid(), Description = "Reservation 2", CheckInDate = new DateTime(2026, 1, 12, 10, 0, 0), CheckOutDate = new DateTime(2026, 1, 13, 10, 0, 0), AppUserId = appUserId },
                new ReservationEntity { Id = Guid.NewGuid(), Description = "Reservation 3", CheckInDate = new DateTime(2026, 1, 10, 10, 0, 0), CheckOutDate = new DateTime(2026, 1, 11, 10, 0, 0), AppUserId = appUserId }
            };

            _context.Reservations.AddRange(reservations);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetUserReservationsAsync(appUserId, 1, 10, null, sortOrder);
            var sorted = result.Items.ToList();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, sorted.Count());
            Assert.AreEqual(reservations[0].Id, sorted[2].Id);
            Assert.AreEqual(reservations[1].Id, sorted[1].Id);
            Assert.AreEqual(reservations[2].Id, sorted[0].Id);
        }

        [TestMethod]
        public async Task GetUserReservationsAsync_SortedByCheckInDateDescending_Returns_SortedReservations()
        {
            // Arrange
            var appUserId = "user-id-123";
            var sortOrder = "start_desc";

            var reservations = new List<ReservationEntity>
            {
                new ReservationEntity { Id = Guid.NewGuid(), Description = "Reservation 1", CheckInDate = new DateTime(2026, 1, 10, 10, 0, 0), CheckOutDate = new DateTime(2026, 1, 11, 10, 0, 0), AppUserId = appUserId },
                new ReservationEntity { Id = Guid.NewGuid(), Description = "Reservation 2", CheckInDate = new DateTime(2026, 1, 12, 10, 0, 0), CheckOutDate = new DateTime(2026, 1, 13, 10, 0, 0), AppUserId = appUserId },
                new ReservationEntity { Id = Guid.NewGuid(), Description = "Reservation 3", CheckInDate = new DateTime(2026, 1, 14, 10, 0, 0), CheckOutDate = new DateTime(2026, 1, 15, 10, 0, 0), AppUserId = appUserId }
            };

            _context.Reservations.AddRange(reservations);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetUserReservationsAsync(appUserId, 1, 10, null, sortOrder);
            var sorted = result.Items.ToList();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, sorted.Count());
            Assert.AreEqual(reservations[0].Id, sorted[2].Id);
            Assert.AreEqual(reservations[1].Id, sorted[1].Id);
            Assert.AreEqual(reservations[2].Id, sorted[0].Id);
        }

        [TestMethod]
        public async Task GetUserReservationsAsync_SortedByCheckOutDateAscending_Returns_SortedReservations()
        {
            // Arrange
            var appUserId = "user-id-123";
            var sortOrder = "end_asc";

            var reservations = new List<ReservationEntity>
            {
                new ReservationEntity { Id = Guid.NewGuid(), Description = "Reservation 1", CheckInDate = new DateTime(2026, 1, 14, 10, 0, 0), CheckOutDate = new DateTime(2026, 1, 15, 10, 0, 0), AppUserId = appUserId },
                new ReservationEntity { Id = Guid.NewGuid(), Description = "Reservation 2", CheckInDate = new DateTime(2026, 1, 12, 10, 0, 0), CheckOutDate = new DateTime(2026, 1, 13, 10, 0, 0), AppUserId = appUserId },
                new ReservationEntity { Id = Guid.NewGuid(), Description = "Reservation 3", CheckInDate = new DateTime(2026, 1, 10, 10, 0, 0), CheckOutDate = new DateTime(2026, 1, 11, 10, 0, 0), AppUserId = appUserId }
            };

            _context.Reservations.AddRange(reservations);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetUserReservationsAsync(appUserId, 1, 10, null, sortOrder);
            var sorted = result.Items.ToList();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, sorted.Count());
            Assert.AreEqual(reservations[0].Id, sorted[2].Id);
            Assert.AreEqual(reservations[1].Id, sorted[1].Id);
            Assert.AreEqual(reservations[2].Id, sorted[0].Id);
        }

        [TestMethod]
        public async Task GetUserReservationsAsync_SortedByCheckOutDateDescending_Returns_SortedReservations()
        {
            // Arrange
            var appUserId = "user-id-123";
            var sortOrder = "end_desc";

            var reservations = new List<ReservationEntity>
            {
                new ReservationEntity { Id = Guid.NewGuid(), Description = "Reservation 1", CheckInDate = new DateTime(2026, 1, 10, 10, 0, 0), CheckOutDate = new DateTime(2026, 1, 11, 10, 0, 0), AppUserId = appUserId },
                new ReservationEntity { Id = Guid.NewGuid(), Description = "Reservation 2", CheckInDate = new DateTime(2026, 1, 12, 10, 0, 0), CheckOutDate = new DateTime(2026, 1, 13, 10, 0, 0), AppUserId = appUserId },
                new ReservationEntity { Id = Guid.NewGuid(), Description = "Reservation 3", CheckInDate = new DateTime(2026, 1, 14, 10, 0, 0), CheckOutDate = new DateTime(2026, 1, 15, 10, 0, 0), AppUserId = appUserId }
            };

            _context.Reservations.AddRange(reservations);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetUserReservationsAsync(appUserId, 1, 10, null, sortOrder);
            var sorted = result.Items.ToList();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, sorted.Count());
            Assert.AreEqual(reservations[0].Id, sorted[2].Id);
            Assert.AreEqual(reservations[1].Id, sorted[1].Id);
            Assert.AreEqual(reservations[2].Id, sorted[0].Id);
        }

        [TestMethod]
        public async Task GetUserReservationsAsync_When_SortedDoesNotExist_Returns_SortedReservations()
        {
            // Arrange
            var appUserId = "user-id-123";
            var sortOrder = "nonexistent";

            var reservations = new List<ReservationEntity>
            {
                new ReservationEntity { Id = Guid.NewGuid(), Description = "Reservation 1", CheckInDate = new DateTime(2026, 1, 10, 10, 0, 0), CheckOutDate = new DateTime(2026, 1, 11, 10, 0, 0), AppUserId = appUserId },
                new ReservationEntity { Id = Guid.NewGuid(), Description = "Reservation 2", CheckInDate = new DateTime(2026, 1, 12, 10, 0, 0), CheckOutDate = new DateTime(2026, 1, 13, 10, 0, 0), AppUserId = appUserId },
                new ReservationEntity { Id = Guid.NewGuid(), Description = "Reservation 3", CheckInDate = new DateTime(2026, 1, 14, 10, 0, 0), CheckOutDate = new DateTime(2026, 1, 15, 10, 0, 0), AppUserId = appUserId }
            };

            _context.Reservations.AddRange(reservations);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetUserReservationsAsync(appUserId, 1, 10, null, sortOrder);
            var sorted = result.Items.ToList();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, sorted.Count());
            Assert.AreEqual(reservations[0].Id, sorted[2].Id);
            Assert.AreEqual(reservations[1].Id, sorted[1].Id);
            Assert.AreEqual(reservations[2].Id, sorted[0].Id);
        }

        [TestMethod]
        public async Task IsLocalBusyAsync_When_NoReservations_Returns_False()
        {
            // Arrange
            var localId = Guid.NewGuid();

            // Act
            var result = await _repository.IsLocalBusyAsync(localId, DateTime.UtcNow, DateTime.UtcNow.AddDays(1));

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task IsLocalBusyAsync_When_CheckIn_Inside_ExistingReservation_Returns_True()
        {
            // Arrange
            var localId = Guid.NewGuid();

            var reservation = new ReservationEntity
            {
                Id = Guid.NewGuid(),
                Description = "Reservation",
                CheckInDate = new DateTime(2026, 1, 15, 10, 0, 0),
                CheckOutDate = new DateTime(2026, 1, 18, 10, 0, 0),
                LocalId = localId,
                AppUserId = "user-id-123"
            };

            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.IsLocalBusyAsync(localId, reservation.CheckInDate.AddHours(12), reservation.CheckOutDate.AddDays(1));

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task IsLocalBusyAsync_When_CheckOut_Inside_ExistingReservation_Returns_True()
        {
            // Arrange
            var localId = Guid.NewGuid();

            var reservation = new ReservationEntity
            {
                Id = Guid.NewGuid(),
                Description = "Reservation",
                CheckInDate = new DateTime(2026, 1, 15, 10, 0, 0),
                CheckOutDate = new DateTime(2026, 1, 18, 10, 0, 0),
                LocalId = localId,
                AppUserId = "user-id-123"
            };

            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.IsLocalBusyAsync(localId, new DateTime(2026, 1, 16, 12, 0, 0), new DateTime(2026, 1, 19, 10, 0, 0));

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task IsLocalBusyAsync_When_NewRange_Covers_ExistingReservation_Returns_True()
        {
            // Arrange
            var localId = Guid.NewGuid();

            var reservation = new ReservationEntity
            {
                Id = Guid.NewGuid(),
                Description = "Reservation",
                CheckInDate = new DateTime(2026, 1, 17, 10, 0, 0),
                CheckOutDate = new DateTime(2026, 1, 20, 10, 0, 0),
                LocalId = localId,
                AppUserId = "user-id-123",
            };

            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.IsLocalBusyAsync(localId, new DateTime(2026, 1, 16, 10, 0, 0), new DateTime(2026, 1, 21, 10, 0, 0));

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task IsLocalBusyAsync_When_NewRange_DoesNotOverlap_Returns_False()
        {
            // Arrange
            var localId = Guid.NewGuid();

            var reservation = new ReservationEntity
            {
                Id = Guid.NewGuid(),
                Description = "Reservation",
                CheckInDate = new DateTime(2026, 1, 16, 10, 0, 0),
                CheckOutDate = new DateTime(2026, 1, 17, 10, 0, 0),
                LocalId = localId,
                AppUserId = "user-id-123"
            };

            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.IsLocalBusyAsync(localId, new DateTime(2026, 1, 14, 10, 0, 0), new DateTime(2026, 1, 16, 10, 0, 0));

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task DeleteReservationAsync_When_ReservationExists_Returns_True()
        {
            // Arrange
            var appUserId = "user-id-123";

            var reservation = new ReservationEntity
            {
                Id = Guid.NewGuid(),
                Description = "Reservation",
                CheckInDate = new DateTime(2026, 1, 15, 10, 0, 0),
                CheckOutDate = new DateTime(2026, 1, 18, 10, 0, 0),
                LocalId = Guid.NewGuid(),
                AppUserId = appUserId
            };

            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            var reservationCountBefore = await _context.Reservations.CountAsync();

            // Act
            var result = await _repository.DeleteReservationAsync(reservation.Id, appUserId);

            var deletedReservation = await _context.Reservations.FirstOrDefaultAsync(x => x.Id == reservation.Id && x.AppUserId == appUserId);

            var reservationCountAfter = await _context.Reservations.CountAsync();

            // Assert
            Assert.IsTrue(result);
            Assert.IsNull(deletedReservation);
            Assert.AreEqual(reservationCountBefore - 1, reservationCountAfter);
        }

        [TestMethod]
        public async Task DeleteReservationAsync_When_ReservationDoesNotExist_Returns_False()
        {
            // Arrange
            var appUserId = "user-id-123";

            // Act
            var result = await _repository.DeleteReservationAsync(Guid.NewGuid(), appUserId);

            // Assert
            Assert.IsFalse(result);
        }
    }
}