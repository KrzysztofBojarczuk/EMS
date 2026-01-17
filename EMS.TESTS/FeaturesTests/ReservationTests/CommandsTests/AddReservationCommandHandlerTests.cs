using EMS.APPLICATION.Features.Reservation.Commands;
using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using EMS.INFRASTRUCTURE.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EMS.TESTS.FeaturesTests.ReservationTests.CommandsTests
{
    [TestClass]
    public class AddReservationCommandHandlerTests
    {
        private Mock<IReservationRepository> _mockReservationRepository;
        private Mock<ILocalRepository> _mockLocalRepository;
        private AddReservationCommandHandler _handler;

        [TestInitialize]
        public void Setup()
        {
            _mockReservationRepository = new Mock<IReservationRepository>();
            _mockLocalRepository = new Mock<ILocalRepository>();
            _handler = new AddReservationCommandHandler(_mockReservationRepository.Object, _mockLocalRepository.Object);
        }

        [TestMethod]
        public async Task Handle_AddReservation_When_LocalNotFound_Returns_Failure()
        {
            // Arrange
            var localId = Guid.NewGuid();

            var expectedReservation = new ReservationEntity
            {
                Description = "Reservation",
                CheckInDate = new DateTime(2026, 1, 15, 10, 0, 0),
                CheckOutDate = new DateTime(2026, 1, 17, 10, 0, 0),
                LocalId = localId,
                AppUserId = "user-id-123"
            };

            var expectedFailureMessage = "Local not found.";

            _mockLocalRepository.Setup(x => x.GetLocalByIdAsync(localId))
                .ReturnsAsync((LocalEntity)null);

            var command = new AddReservationCommand(expectedReservation);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual(expectedFailureMessage, result.Error);
            _mockLocalRepository.Verify(x => x.GetLocalByIdAsync(localId), Times.Once);
            _mockReservationRepository.Verify(x => x.IsLocalBusyAsync(It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Never);
            _mockReservationRepository.Verify(x => x.AddReservationAsync(It.IsAny<ReservationEntity>()), Times.Never);
        }

        [TestMethod]
        public async Task Handle_AddReservation_When_LocalNeedsRepair_Returns_Failure()
        {
            // Arrange
            var localId = Guid.NewGuid();

            var expectedReservation = new ReservationEntity
            {
                Description = "Reservation",
                CheckInDate = new DateTime(2026, 1, 15, 10, 0, 0),
                CheckOutDate = new DateTime(2026, 1, 17, 10, 0, 0),
                LocalId = localId,
                AppUserId = "user-id-123"
            };

            var local = new LocalEntity
            {
                Id = localId,
                NeedsRepair = true
            };

            var expectedFailureMessage = "Local is under repair.";

            _mockLocalRepository.Setup(x => x.GetLocalByIdAsync(localId))
                .ReturnsAsync(local);

            var command = new AddReservationCommand(expectedReservation);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual(expectedFailureMessage, result.Error);
            _mockLocalRepository.Verify(x => x.GetLocalByIdAsync(localId), Times.Once);
            _mockReservationRepository.Verify(x => x.IsLocalBusyAsync(It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Never);
            _mockReservationRepository.Verify(x => x.AddReservationAsync(It.IsAny<ReservationEntity>()), Times.Never);
        }

        [TestMethod]
        public async Task Handle_AddReservation_When_LocalIsBusy_Returns_Failure()
        {
            // Arrange
            var localId = Guid.NewGuid();

            var reservation = new ReservationEntity
            {
                Description = "Reservation",
                CheckInDate = new DateTime(2026, 1, 15, 10, 0, 0),
                CheckOutDate = new DateTime(2026, 1, 17, 10, 0, 0),
                LocalId = localId,
                AppUserId = "user-id-123"
            };

            var local = new LocalEntity
            {
                Id = localId,
                NeedsRepair = false
            };

            var expectedFailureMessage = "Local is already reserved in the given time period.";

            _mockLocalRepository.Setup(x => x.GetLocalByIdAsync(localId))
                .ReturnsAsync(local);

            _mockReservationRepository.Setup(x => x.IsLocalBusyAsync(localId, It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .ReturnsAsync(true);

            var command = new AddReservationCommand(reservation);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual(expectedFailureMessage, result.Error);
            _mockLocalRepository.Verify(x => x.GetLocalByIdAsync(localId), Times.Once);
            _mockReservationRepository.Verify(x => x.IsLocalBusyAsync(localId, It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once);
            _mockReservationRepository.Verify(x => x.AddReservationAsync(It.IsAny<ReservationEntity>()), Times.Never);
        }

        [TestMethod]
        public async Task Handle_AddReservation_And_Returns_Reservation_Successful_Result()
        {
            // Arrange
            var localId = Guid.NewGuid();

            var expectedReservation = new ReservationEntity
            {
                Description = "Reservation",
                CheckInDate = new DateTime(2026, 1, 15, 10, 0, 0),
                CheckOutDate = new DateTime(2026, 1, 17, 10, 0, 0),
                LocalId = localId,
                AppUserId = "user-id-123"
            };

            var local = new LocalEntity
            {
                Id = localId,
                NeedsRepair = false
            };

            _mockLocalRepository.Setup(x => x.GetLocalByIdAsync(localId))
                    .ReturnsAsync(local);

            _mockReservationRepository.Setup(x => x.IsLocalBusyAsync(localId, It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                    .ReturnsAsync(false);

            _mockReservationRepository.Setup(x => x.AddReservationAsync(expectedReservation))
                    .ReturnsAsync(expectedReservation);

            var command = new AddReservationCommand(expectedReservation);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(expectedReservation, result.Value);
            _mockLocalRepository.Verify(x => x.GetLocalByIdAsync(localId), Times.Once);
            _mockReservationRepository.Verify(x => x.IsLocalBusyAsync(localId, It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once);
            _mockReservationRepository.Verify(x => x.AddReservationAsync(expectedReservation), Times.Once);
        }
    }
}