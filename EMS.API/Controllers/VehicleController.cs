using AutoMapper;
using EMS.APPLICATION.Dtos;
using EMS.APPLICATION.Extensions;
using EMS.APPLICATION.Features.Vehicle.Commands;
using EMS.CORE.Entities;
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
        public async Task<IActionResult> AddVehicleAsync([FromBody] VehicleEntity vehicleEntity)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var username = User.GetUsername();

            var appUser = await userManager.FindByNameAsync(username);

            vehicleEntity.AppUserId = appUser.Id;

            var result = await sender.Send(new AddVehicleCommand(vehicleEntity));

            var vehicleGet = mapper.Map<VehicleGetDto>(result);

            return Ok(vehicleGet);
        }
    }
}