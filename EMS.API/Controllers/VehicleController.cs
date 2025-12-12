using AutoMapper;
using EMS.APPLICATION.Dtos;
using EMS.APPLICATION.Extensions;
using EMS.APPLICATION.Features.Vehicle.Commands;
using EMS.APPLICATION.Features.Vehicle.Queries;
using EMS.CORE.Entities;
using EMS.CORE.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleController(ISender sender, UserManager<AppUserEntity> userManager, IMapper mapper) : ControllerBase
    {
        [HttpPost()]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> AddVehicleAsync([FromBody] VehicleCreateDto vehicleDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var username = User.GetUsername();

            var appUser = await userManager.FindByNameAsync(username);

            var vehicleEntity = mapper.Map<VehicleEntity>(vehicleDto);

            vehicleEntity.AppUserId = appUser.Id;

            var result = await sender.Send(new AddVehicleCommand(vehicleEntity));

            var vehicleGet = mapper.Map<VehicleGetDto>(result);

            return Ok(vehicleGet);
        }

        [HttpGet("User")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetUserVehiclesAsync([FromQuery] int pageNumber, [FromQuery] int pageSize, [FromQuery] string searchTerm = null, [FromQuery] List<VehicleType> vehicleType = null, [FromQuery] DateTime? dateFrom = null, [FromQuery] DateTime? dateTo = null, [FromQuery] string sortOrder = null)
        {
            var username = User.GetUsername();

            var appUser = await userManager.FindByNameAsync(username);

            var paginatedVehicles = await sender.Send(new GetUserVehiclesQuery(appUser.Id, pageNumber, pageSize, searchTerm, vehicleType, dateFrom, dateTo, sortOrder));

            var vehicleGet = mapper.Map<IEnumerable<VehicleGetDto>>(paginatedVehicles.Items);

            return Ok(new
            {
                VehicleGet = vehicleGet,
                paginatedVehicles.TotalItems,
                paginatedVehicles.TotalPages,
                paginatedVehicles.PageIndex
            });
        }

        [HttpGet("UserVehiclesForTask")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetUserVehiclesForTaskAsync([FromQuery] string searchTerm = null)
        {
            var username = User.GetUsername();

            var appUser = await userManager.FindByNameAsync(username);

            var result = await sender.Send(new GetUserVehiclesForTaskQuery(appUser.Id, searchTerm));

            var vehicleGet = mapper.Map<IEnumerable<VehicleGetDto>>(result);

            return Ok(vehicleGet);
        }

        [HttpPut("{vehicleId}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> UpdateVehicleAsync([FromRoute] Guid vehicleId, [FromBody] VehicleCreateDto vehicleDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var username = User.GetUsername();

            var appUser = await userManager.FindByNameAsync(username);

            var vehicleEntity = mapper.Map<VehicleEntity>(vehicleDto);

            var result = await sender.Send(new UpdateVehicleCommand(vehicleId, appUser.Id, vehicleEntity));

            var vehicleGet = mapper.Map<VehicleGetDto>(result);

            return Ok(vehicleGet);
        }

        [HttpDelete("{vehicleId}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> DeleteVehicleAsync([FromRoute] Guid vehicleId)
        {
            var username = User.GetUsername();

            var appUser = await userManager.FindByNameAsync(username);

            var result = await sender.Send(new DeleteVehicleCommand(vehicleId, appUser.Id));

            return Ok(result);
        }
    }
}