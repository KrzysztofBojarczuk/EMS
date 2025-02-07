using AutoMapper;
using EMS.APPLICATION.Dtos;
using EMS.APPLICATION.Extensions;
using EMS.APPLICATION.Features.Reservation.Commands;
using EMS.CORE.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController(ISender sender, UserManager<AppUserEntity> userManager, IMapper mapper) : ControllerBase
    {
        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> MakeReservation([FromBody] ReservationCreateDto reservationDto)
        {
            var username = User.GetUsername();

            var appUser = await userManager.FindByNameAsync(username);

            var reservation = mapper.Map<ReservationEntity>(reservationDto);

            reservation.AppUserId = appUser.Id;
            reservation.AppUserEntity = appUser;

            var result = await sender.Send(new MakeReservationCommand(reservation));

            return Ok(result);
        }
    }
}
