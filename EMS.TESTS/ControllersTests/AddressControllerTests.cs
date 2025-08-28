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
using EMS.APPLICATION.Features.Address.Commands;

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
            var appUserId = "user-id-123";
            var username = "testuser";
            var pageNumber = 1;
            var pageSize = 10;
            var searchTerm = "Main";

            var appUser = new AppUserEntity { Id = appUserId, UserName = username };

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
                    x.appUserId == appUserId &&
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
            var appUserId = "user-id-123";
            var username = "testuser";
            var pageNumber = 1;
            var pageSize = 10;
            var searchTerm = "nonexistent";

            var appUser = new AppUserEntity { Id = appUserId, UserName = username };

            var emptyEntities = new List<AddressEntity>();
            var paginatedResult = new PaginatedList<AddressEntity>(emptyEntities, 0, pageNumber, pageSize);

            var expectedDtos = new List<AddressGetDto>();

            _mockUserManager.Setup(x => x.FindByNameAsync(username))
                .ReturnsAsync(appUser);

            _mockSender.Setup(x => x.Send(
                It.Is<GetUserAddressQuery>(x =>
                    x.appUserId == appUserId &&
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
            var appUserId = "user-id-123";
            var username = "testuser";
            var searchTerm = "City";

            var appUser = new AppUserEntity { Id = appUserId, UserName = username };

            var addressEntities = new List<AddressEntity>
            {
                new AddressEntity { Id = Guid.NewGuid(), City = "City1", Street = "Street1", Number = "1", ZipCode = "00-000" },
                new AddressEntity { Id = Guid.NewGuid(), City = "City2", Street = "Street2", Number = "2", ZipCode = "11-111" }
            };

            var expectedDtos = new List<AddressGetDto>
            {
                   new AddressGetDto { Id = addressEntities[0].Id, City = "City1", Street = "Street1", Number = "1", ZipCode = "00-000" },
                   new AddressGetDto { Id = addressEntities[1].Id, City = "City2", Street = "Street2", Number = "2", ZipCode = "11-111" }
            };

            _mockUserManager.Setup(x => x.FindByNameAsync(username))
                .ReturnsAsync(appUser);

            _mockSender.Setup(x => x.Send(It.IsAny<GetUserAddressForTaskQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(addressEntities);

            _mockMapper.Setup(x => x.Map<IEnumerable<AddressGetDto>>(It.IsAny<IEnumerable<AddressEntity>>()))
                .Returns(expectedDtos);

            // Act
            var result = await _controller.GetUserAddressForTaskAsync(searchTerm);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var returnedDtos = okResult.Value as IEnumerable<AddressGetDto>;
            Assert.IsNotNull(returnedDtos);
            Assert.AreEqual(expectedDtos.Count(), returnedDtos.Count());
        }

        [TestMethod]
        public async Task GetUserAddressForTaskAsync_ReturnsOkResult_NotFound_WithEmptyList()
        {
            // Arrange
            var appUserId = "user-id-123";
            var searchTerm = "nonexistent";
            var username = "testuser";

            var appUser = new AppUserEntity { Id = appUserId, UserName = username };

            var addressEntities = new List<AddressEntity>();

            var expectedDtos = new List<AddressGetDto>();

            _mockUserManager.Setup(x => x.FindByNameAsync(username))
                .ReturnsAsync(appUser);

            _mockSender.Setup(x => x.Send(It.IsAny<GetUserAddressForTaskQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(addressEntities);

            _mockMapper.Setup(x => x.Map<IEnumerable<AddressGetDto>>(It.IsAny<IEnumerable<AddressEntity>>()))
                .Returns(expectedDtos);

            // Act
            var result = await _controller.GetUserAddressForTaskAsync(searchTerm);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var returnedDtos = okResult.Value as IEnumerable<AddressGetDto>;
            Assert.IsNotNull(returnedDtos);
            Assert.AreEqual(expectedDtos.Count(), returnedDtos.Count());
        }

        [TestMethod]
        public async Task AddAddressAsync_ReturnsOkResult_WithAddressGetDto()
        {
            // Arrange
            var appUserId = "user-id-123";
            var username = "testuser";

            var appUser = new AppUserEntity { Id = appUserId, UserName = username };

            var createDto = new AddressCreateDto
            {
                City = "CityX",
                Street = "StreetX",
                Number = "1A",
                ZipCode = "00-001"
            };

            var addressEntity = new AddressEntity
            {
                Id = Guid.NewGuid(),
                City = createDto.City,
                Street = createDto.Street,
                Number = createDto.Number,
                ZipCode = createDto.ZipCode,
                AppUserId = appUser.Id,
                AppUserEntity = appUser
            };

            var resultEntity = new AddressEntity
            {
                Id = addressEntity.Id,
                City = addressEntity.City,
                Street = addressEntity.Street,
                Number = addressEntity.Number,
                ZipCode = addressEntity.ZipCode,
                AppUserId = appUser.Id,
                AppUserEntity = appUser
            };

            var expectedDto = new AddressGetDto
            {
                Id = resultEntity.Id,
                City = resultEntity.City,
                Street = resultEntity.Street,
                Number = resultEntity.Number,
                ZipCode = resultEntity.ZipCode
            };

            _mockUserManager.Setup(x => x.FindByNameAsync(username))
                .ReturnsAsync(appUser);

            _mockMapper.Setup(x => x.Map<AddressEntity>(createDto))
                .Returns(addressEntity);

            _mockSender.Setup(x => x.Send(It.IsAny<AddAddressCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(resultEntity);

            _mockMapper.Setup(x => x.Map<AddressGetDto>(resultEntity))
                .Returns(expectedDto);

            // Act
            var result = await _controller.AddAddressAsync(createDto);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var returnedDto = okResult.Value as AddressGetDto;
            Assert.IsNotNull(returnedDto);
            Assert.AreEqual(expectedDto.Id, returnedDto.Id);
            Assert.AreEqual(expectedDto.City, returnedDto.City);
            Assert.AreEqual(expectedDto.Street, returnedDto.Street);
            Assert.AreEqual(expectedDto.Number, returnedDto.Number);
            Assert.AreEqual(expectedDto.ZipCode, returnedDto.ZipCode);
        }

        [TestMethod]
        public async Task UpdateAddressAsync_ReturnsOkResult_WithUpdatedAddressDto()
        {
            // Arrange
            var addressId = Guid.NewGuid();
            var username = "testuser";

            var updateDto = new AddressCreateDto
            {
                City = "UpdatedCity",
                Street = "UpdatedStreet",
                Number = "99A",
                ZipCode = "99-999"
            };

            var addressEntity = new AddressEntity
            {
                Id = addressId,
                City = updateDto.City,
                Street = updateDto.Street,
                Number = updateDto.Number,
                ZipCode = updateDto.ZipCode
            };

            var updatedEntity = new AddressEntity
            {
                Id = addressId,
                City = updateDto.City,
                Street = updateDto.Street,
                Number = updateDto.Number,
                ZipCode = updateDto.ZipCode
            };

            var expectedDto = new AddressGetDto
            {
                Id = addressId,
                City = updateDto.City,
                Street = updateDto.Street,
                Number = updateDto.Number,
                ZipCode = updateDto.ZipCode
            };

            _mockMapper.Setup(m => m.Map<AddressEntity>(updateDto))
                .Returns(addressEntity);

            _mockSender.Setup(s => s.Send(
                It.Is<UpdateAddressCommand>(x =>
                    x.AddressId == addressId &&
                    x.Address == addressEntity),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(updatedEntity);

            _mockMapper.Setup(m => m.Map<AddressGetDto>(updatedEntity))
                .Returns(expectedDto);

            // Act
            var result = await _controller.UpdateAddressAsync(addressId, updateDto);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var returnedDto = okResult.Value as AddressGetDto;
            Assert.IsNotNull(returnedDto);
            Assert.AreEqual(expectedDto.Id, returnedDto.Id);
            Assert.AreEqual(expectedDto.City, returnedDto.City);
            Assert.AreEqual(expectedDto.Street, returnedDto.Street);
            Assert.AreEqual(expectedDto.Number, returnedDto.Number);
            Assert.AreEqual(expectedDto.ZipCode, returnedDto.ZipCode);
        }

        [TestMethod]
        public async Task DeleteAddressAsync_ReturnsOkResult_WithTrue_When_DeletedSuccessfully()
        {
            // Arrange
            var addressId = Guid.NewGuid();
            var expectedResult = true;

            _mockSender.Setup(x => x.Send(It.Is<DeleteAddressCommand>(x => x.addressId == addressId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.DeleteAddressAsync(addressId);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(expectedResult, okResult.Value);
        }

        [TestMethod]
        public async Task DeleteAddressAsync_ReturnsOkResult_WithFalse_When_DeletionFails()
        {
            // Arrange
            var addressId = Guid.NewGuid();
            var expectedResult = false;

            _mockSender.Setup(x => x.Send(It.Is<DeleteAddressCommand>(x => x.addressId == addressId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.DeleteAddressAsync(addressId);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(expectedResult, okResult.Value);
        }
    }
}