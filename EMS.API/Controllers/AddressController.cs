using AutoMapper;
using EMS.APPLICATION.Dtos;
using EMS.APPLICATION.Extensions;
using EMS.APPLICATION.Features.Address.Commands;
using EMS.APPLICATION.Features.Address.Queries;
using EMS.CORE.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController(ISender sender, UserManager<AppUserEntity> userManager, IMapper mapper) : ControllerBase
    {
        [HttpGet("User")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetUserAddressAsync(string searchTerm = null)
        {
            var username = User.GetUsername();

            var appUser = await userManager.FindByNameAsync(username);

            var result = await sender.Send(new GetUserAddressQuery(appUser.Id, searchTerm));

            var addressDtos = mapper.Map<IEnumerable<AddressGetDto>>(result);

            return Ok(addressDtos);
        }

        [HttpPost()]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> AddAddressAsync([FromBody] AddressCreateDto addressDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var username = User.GetUsername();

            var appUser = await userManager.FindByNameAsync(username);

            var addressEntity = mapper.Map<AddressEntity>(addressDto);

            addressEntity.AppUserId = appUser.Id;
            addressEntity.AppUserEntity = appUser;

            var result = await sender.Send(new AddAddressCommand(addressEntity));

            return Ok(result);
        }

        [HttpPut("{addressId}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> UpdateAddressAsync([FromRoute] Guid addressId, [FromBody] AddressCreateDto updateAddressDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var addressEntity = mapper.Map<AddressEntity>(updateAddressDto);

            var result = await sender.Send(new UpdateAddressCommand(addressId, addressEntity));

            return Ok(result);
        }

        [HttpDelete("{addressId}")]
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> DeleteAddressAsync([FromRoute] Guid addressId)
        {
            var result = await sender.Send(new DeleteAddressCommand(addressId));

            return Ok(result);
        }
    }
}
