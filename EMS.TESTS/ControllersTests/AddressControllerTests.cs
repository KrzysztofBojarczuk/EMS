using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using EMS.API.Controllers;
using EMS.APPLICATION.Dtos;
using EMS.APPLICATION.Features.Address.Queries;
using EMS.CORE.Entities;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using EMS.INFRASTRUCTURE.Extensions;

namespace EMS.TESTS.ControllersTests
{
    [TestClass]
    public class AddressControllerTests
    {
        private Mock<ISender> _mockSender;
        private Mock<UserManager<AppUserEntity>> _mockUserManager;
        private Mock<IMapper> _mockMapper;
        private AddressController _controller;

        [TestInitialize]
        public void Setup()
        {
            _mockSender = new Mock<ISender>();
            _mockMapper = new Mock<IMapper>();

            var store = new Mock<IUserStore<AppUserEntity>>();
            _mockUserManager = new Mock<UserManager<AppUserEntity>>(store.Object, null, null, null, null, null, null, null, null);

            _controller = new AddressController(_mockSender.Object, _mockUserManager.Object, _mockMapper.Object);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname", "testuser")
            }, "mock"));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }

        [TestMethod]
        public async Task GetUserAddressAsync_ReturnsOkResult_BySearchTerm_WithAddressDtos()
        {
            // Arrange
            var userId = "user-id-123";
            var username = "testuser";
            var pageNumber = 1;
            var pageSize = 10;
            var searchTerm = "Main";

            var appUser = new AppUserEntity { Id = userId, UserName = "testuser" };

            var addressEntities = new List<AddressEntity>
            {
                new AddressEntity { Id = Guid.NewGuid(), City = "MainCity1", Street = "Street1", Number = "1", ZipCode = "00-000" },
                new AddressEntity { Id = Guid.NewGuid(), City = "MainCity2", Street = "Street2", Number = "2", ZipCode = "11-111" }
             };

            var paginatedResult = new PaginatedList<AddressEntity>(addressEntities, addressEntities.Count, pageNumber, pageSize);

            var expectedDtos = new List<AddressGetDto>
            {
                new AddressGetDto { Id = addressEntities[0].Id, City = "MainCity1", Street = "Street1", Number = "1", ZipCode = "00-000" },
                new AddressGetDto { Id = addressEntities[1].Id, City = "MainCity2", Street = "Street2", Number = "2", ZipCode = "11-111" }
            };

            _mockUserManager.Setup(x => x.FindByNameAsync(username))
                .ReturnsAsync(appUser);

            _mockSender.Setup(x => x.Send(
                It.Is<GetUserAddressQuery>(x =>
                    x.appUserId == userId &&
                    x.pageNumber == pageNumber &&
                    x.pageSize == pageSize &&
                    x.searchTerm == searchTerm),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(paginatedResult);

            _mockMapper.Setup(x => x.Map<IEnumerable<AddressGetDto>>(addressEntities))
                .Returns(expectedDtos);

            // Act
            var result = await _controller.GetUserAddressAsync(pageNumber, pageSize, searchTerm);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var value = okResult.Value;
            var addressGetProperty = value.GetType().GetProperty("AddressGet");
            var totalItemsProperty = value.GetType().GetProperty("TotalItems");
            var totalPagesProperty = value.GetType().GetProperty("TotalPages");
            var pageIndexProperty = value.GetType().GetProperty("PageIndex");

            Assert.IsNotNull(addressGetProperty);
            var returnedDtos = addressGetProperty.GetValue(value) as IEnumerable<AddressGetDto>;
            Assert.IsNotNull(returnedDtos);
            Assert.AreEqual(expectedDtos.Count(), returnedDtos.Count());

            Assert.AreEqual(paginatedResult.TotalItems, totalItemsProperty.GetValue(value));
            Assert.AreEqual(paginatedResult.TotalPages, totalPagesProperty.GetValue(value));
            Assert.AreEqual(paginatedResult.PageIndex, pageIndexProperty.GetValue(value));
        }

        [TestMethod]
        public async Task GetUserAddressAsync_ReturnsOkResult_NotFound_WithEmptyList()
        {
            // Arrange
            var userId = "user-id-123";
            var username = "testuser";
            var pageNumber = 1;
            var pageSize = 10;
            string searchTerm = "nonexistent";

            var appUser = new AppUserEntity { Id = userId, UserName = username };

            var emptyEntities = new List<AddressEntity>();
            var paginatedResult = new PaginatedList<AddressEntity>(emptyEntities, 0, pageNumber, pageSize);

            var expectedDtos = new List<AddressGetDto>();

            _mockUserManager.Setup(x => x.FindByNameAsync(username))
                .ReturnsAsync(appUser);

            _mockSender.Setup(x => x.Send(
                It.Is<GetUserAddressQuery>(x =>
                    x.appUserId == userId &&
                    x.pageNumber == pageNumber &&
                    x.pageSize == pageSize &&
                    x.searchTerm == searchTerm),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(paginatedResult);

            _mockMapper.Setup(x => x.Map<IEnumerable<AddressGetDto>>(emptyEntities))
                .Returns(expectedDtos);

            // Act
            var result = await _controller.GetUserAddressAsync(pageNumber, pageSize, searchTerm);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var value = okResult.Value;
            var addressGetProperty = value.GetType().GetProperty("AddressGet");
            var totalItemsProperty = value.GetType().GetProperty("TotalItems");
            var totalPagesProperty = value.GetType().GetProperty("TotalPages");
            var pageIndexProperty = value.GetType().GetProperty("PageIndex");

            Assert.IsNotNull(addressGetProperty);
            var returnedDtos = addressGetProperty.GetValue(value) as IEnumerable<AddressGetDto>;
            Assert.IsNotNull(returnedDtos);
            Assert.AreEqual(expectedDtos.Count(), returnedDtos.Count());

            Assert.AreEqual(paginatedResult.TotalItems, totalItemsProperty.GetValue(value));
            Assert.AreEqual(paginatedResult.TotalPages, totalPagesProperty.GetValue(value));
            Assert.AreEqual(paginatedResult.PageIndex, pageIndexProperty.GetValue(value));
        }

        [TestMethod]
        public async Task GetUserAddressForTaskAsync_ReturnsOkResult_BySearchTerm_WithAddressDtos()
        {
            // Arrange
            var userId = "user-id-123";
            var searchTerm = "City";

            var appUser = new AppUserEntity { Id = userId, UserName = "testuser" };

            var addressEntities = new List<AddressEntity>
            {
                new AddressEntity { Id = Guid.NewGuid(), City = "City1", Street = "Street1", Number = "1", ZipCode = "00-000" },
                new AddressEntity { Id = Guid.NewGuid(), City = "City2", Street = "Street2", Number = "2", ZipCode = "11-111" }
            };

            _mockUserManager
                .Setup(x => x.FindByNameAsync("testuser"))
                .ReturnsAsync(appUser);

            _mockSender
                .Setup(x => x.Send(It.IsAny<GetUserAddressForTaskQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(addressEntities);

            _mockMapper
                .Setup(x => x.Map<IEnumerable<AddressGetDto>>(It.IsAny<IEnumerable<AddressEntity>>()))
                .Returns(new List<AddressGetDto>
                {
                   new AddressGetDto { Id = addressEntities[0].Id, City = "City1", Street = "Street1", Number = "1", ZipCode = "00-000" },
                   new AddressGetDto { Id = addressEntities[1].Id, City = "City2", Street = "Street2", Number = "2", ZipCode = "11-111" }
                });

            // Act
            var result = await _controller.GetUserAddressForTaskAsync(searchTerm);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var addressDtos = okResult.Value as IEnumerable<AddressGetDto>;
            Assert.IsNotNull(addressDtos);
            Assert.AreEqual(2, addressDtos.Count());
        }

        [TestMethod]
        public async Task GetUserAddressForTaskAsync_ReturnsOkResult_NotFound_WithEmptyList()
        {
            // Arrange
            var userId = "user-id-123";
            var searchTerm = "nonexistent";

            var appUser = new AppUserEntity { Id = userId, UserName = "testuser" };

            var addressEntities = new List<AddressEntity>();

            var expectedDtos = new List<AddressGetDto>();

            _mockUserManager
                .Setup(x => x.FindByNameAsync("testuser"))
                .ReturnsAsync(appUser);

            _mockSender
                .Setup(x => x.Send(It.IsAny<GetUserAddressForTaskQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(addressEntities);

            _mockMapper
                .Setup(x => x.Map<IEnumerable<AddressGetDto>>(It.IsAny<IEnumerable<AddressEntity>>()))
                .Returns(expectedDtos);

            // Act
            var result = await _controller.GetUserAddressForTaskAsync(searchTerm);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var addressDtos = okResult.Value as IEnumerable<AddressGetDto>;
            Assert.IsNotNull(addressDtos);
            Assert.AreEqual(0, addressDtos.Count());
        }
    }
}