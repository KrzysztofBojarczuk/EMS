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
                LocalId = Guid.NewGuid(),
                AppUserId = "user-id-123",
                CheckInDate = DateTime.UtcNow,
                CheckOutDate = DateTime.UtcNow.AddDays(2)
            };

            // Act
            var result = await _repository.AddReservationAsync(reservation);

            var reservationCount = await _context.Reservations.CountAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(reservation.LocalId, result.LocalId);
            Assert.AreEqual(reservation.AppUserId, result.AppUserId);
            Assert.AreEqual(reservation.CheckInDate, result.CheckInDate);
            Assert.AreEqual(reservation.CheckOutDate, result.CheckOutDate);
            Assert.AreNotEqual(Guid.Empty, result.Id);
            Assert.AreEqual(1, reservationCount);
        }

        [TestMethod]
        public async Task GetReservationByIdAsync_When_ReservationExists_Returns_Reservation()
        {
            // Arrange
            var reservation = new ReservationEntity
            {
                LocalId = Guid.NewGuid(),
                AppUserId = "user-id-123",
                CheckInDate = DateTime.UtcNow,
                CheckOutDate = DateTime.UtcNow.AddDays(2)
            };

            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetReservationByIdAsync(reservation.Id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(reservation.Id, result.Id);
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
                LocalId = localId,
                AppUserId = "user-id-123",
                CheckInDate = DateTime.UtcNow,
                CheckOutDate = DateTime.UtcNow.AddDays(2)
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
                LocalId = localId,
                AppUserId = "user",
                CheckInDate = DateTime.UtcNow.AddDays(1),
                CheckOutDate = DateTime.UtcNow.AddDays(3)
            };

            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.IsLocalBusyAsync(localId, reservation.CheckInDate.AddDays(-1), reservation.CheckInDate.AddHours(12));

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task DeleteReservationAsync_When_ReservationExists_Returns_True()
        {
            // Arrange
            var appUserId = "user-id-123";
            var reservation = new ReservationEntity
            {
                LocalId = Guid.NewGuid(),
                AppUserId = appUserId,
                CheckInDate = DateTime.UtcNow,
                CheckOutDate = DateTime.UtcNow.AddDays(2)
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