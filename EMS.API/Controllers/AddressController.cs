using EMS.APPLICATION.Dtos;
using EMS.APPLICATION.Extensions;
using EMS.APPLICATION.Features.Address.Commands;
using EMS.APPLICATION.Features.Address.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController(ISender sender) : ControllerBase
    {
        [HttpPost()]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AddressGetDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> AddAddressAsync([FromBody] AddressCreateDto addressDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userName = User.GetUsername();

            var result = await sender.Send(new AddAddressCommand(addressDto, userName));

            return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
        }

        [HttpGet("User")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetUserAddressesAsync([FromQuery] int pageNumber, [FromQuery] int pageSize, [FromQuery] string searchTerm = null)
        {
            var userName = User.GetUsername();

            var result = await sender.Send(new GetUserAddressesQuery(userName, pageNumber, pageSize, searchTerm));

            return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
        }

        [HttpGet("UserAddressesForTask")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<AddressGetDto>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetUserAddressesForTaskAsync([FromQuery] string searchTerm = null)
        {
            var userName = User.GetUsername();

            var result = await sender.Send(new GetUserAddressesForTaskQuery(userName, searchTerm));

            return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
        }

        [HttpPut("{addressId}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AddressGetDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> UpdateAddressAsync([FromRoute] Guid addressId, [FromBody] AddressCreateDto updateAddressDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userName = User.GetUsername();

            var result = await sender.Send(new UpdateAddressCommand(addressId, userName, updateAddressDto));

            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }

        [HttpDelete("{addressId}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAddressAsync([FromRoute] Guid addressId)
        {
            var userName = User.GetUsername();

            var result = await sender.Send(new DeleteAddressCommand(addressId, userName));

            return result ? Ok(result) : NotFound(result);
        }
    }
}