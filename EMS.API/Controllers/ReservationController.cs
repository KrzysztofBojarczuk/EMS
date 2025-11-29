using AutoMapper;
using EMS.APPLICATION.Dtos;
using EMS.APPLICATION.Extensions;
using EMS.APPLICATION.Features.Reservation.Commands;
using EMS.APPLICATION.Features.Reservation.Queries;
using EMS.CORE.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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
        public async Task<IActionResult> AddReservationAsync([FromBody] ReservationCreateDto reservationDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var username = User.GetUsername();

            var appUser = await userManager.FindByNameAsync(username);

            var reservationEntity = mapper.Map<ReservationEntity>(reservationDto);

            reservationEntity.AppUserId = appUser.Id;

            var result = await sender.Send(new AddReservationCommand(reservationEntity));

            if (result.IsFailure)
                return BadRequest(result.Error);

            var reservationGet = mapper.Map<ReservationGetDto>(result.Value);

            return Ok(reservationGet);
        }

        [HttpGet]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetUserReservationAsync([FromQuery] int pageNumber, [FromQuery] int pageSize, [FromQuery] string searchTerm = null, [FromQuery] string sortOrderDate = null)
        {
            var username = User.GetUsername();

            var appUser = await userManager.FindByNameAsync(username);

            var paginatedReservation = await sender.Send(new GetUserReservationQuery(appUser.Id, pageNumber, pageSize, searchTerm, sortOrderDate));

            var reservationGet = mapper.Map<IEnumerable<ReservationGetDto>>(paginatedReservation.Items);

            return Ok(new
            {
                ReservationGet = reservationGet,
                paginatedReservation.TotalItems,
                paginatedReservation.TotalPages,
                paginatedReservation.PageIndex
            });
        }

        [HttpDelete("{reservationId}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> DeleteReservationAsync([FromRoute] Guid reservationId)
        {
            var username = User.GetUsername();

            var appUser = await userManager.FindByNameAsync(username);

            var result = await sender.Send(new DeleteReservationCommand(reservationId, appUser.Id));

            return Ok(result);
        }
    }
}