using EMS.APPLICATION.Features.Address.Commands;
using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using MediatR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EMS.TESTS.Features.AddressTests.CommandsTests
{
    [TestClass]
    public class AddAddressCommandHandlerTests
    {
        private Mock<IAddressRepository> _mockAddressRepository;
        private AddAddressCommandHandler _handler;

        [TestInitialize]
        public void Setup()
        {
            _mockAddressRepository = new Mock<IAddressRepository>();
            var mockMediator = new Mock<IPublisher>();
            _handler = new AddAddressCommandHandler(_mockAddressRepository.Object, mockMediator.Object);
        }

        [TestMethod]
        public async Task Handle_AddAddress_And_Returns_Address()
        {
            // Arrange
            var addressToAdd = new AddressEntity
            {
                City = "Test City",
                Street = "Test Street",
                Number = "123",
                ZipCode = "00-001",
                AppUserId = "user123"
            };

            _mockAddressRepository
                .Setup(repo => repo.AddAddressAsync(addressToAdd))
                .ReturnsAsync(addressToAdd);

            var command = new AddAddressCommand(addressToAdd);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(addressToAdd, result);

            _mockAddressRepository.Verify(repo => repo.AddAddressAsync(addressToAdd), Times.Once);
        }
    }
}