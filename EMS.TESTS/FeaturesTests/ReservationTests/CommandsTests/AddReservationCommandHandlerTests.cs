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

            var expectedEeservation = new ReservationEntity
            {
                Description = "Test",
                CheckInDate = DateTime.UtcNow,
                CheckOutDate = DateTime.UtcNow.AddDays(2),
                LocalId = localId,
                AppUserId = "user-id-123"
            };

            var expectedFailureMessage = "Local not found.";

            _mockLocalRepository.Setup(x => x.GetLocalByIdAsync(localId))
                .ReturnsAsync((LocalEntity)null);

            var command = new AddReservationCommand(expectedEeservation);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual(expectedFailureMessage, result.Error);
            _mockLocalRepository.Verify(x => x.GetLocalByIdAsync(localId), Times.Once);
            _mockReservationRepository.Verify(x => x.AddReservationAsync(It.IsAny<ReservationEntity>()), Times.Never);
        }
    }
}