using EMS.APPLICATION.Features.Reservation.Commands;
using EMS.CORE.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EMS.TESTS.FeaturesTests.ReservationTests.CommandsTests
{
    [TestClass]
    public class DeleteReservationCommandHandlerTests
    {
        private Mock<IReservationRepository> _mockReservationRepository;
        private DeleteReservationCommandHandler _handler;

        [TestInitialize]
        public void Setup()
        {
            _mockReservationRepository = new Mock<IReservationRepository>();
            _handler = new DeleteReservationCommandHandler(_mockReservationRepository.Object);
        }

        [TestMethod]
        public async Task Handle_ShouldReturnTrue_When_ReservationIsDeletedSuccessfully()
        {
            // Arrange
            var reservationId = Guid.NewGuid();
            var expectedResult = true;
            var appUserId = "user-id-123";

            _mockReservationRepository.Setup(x => x.DeleteReservationAsync(reservationId, appUserId))
                .ReturnsAsync(expectedResult);

            var command = new DeleteReservationCommand(reservationId, appUserId);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsTrue(result);
            _mockReservationRepository.Verify(x => x.DeleteReservationAsync(reservationId, appUserId), Times.Once);
        }
    }
}