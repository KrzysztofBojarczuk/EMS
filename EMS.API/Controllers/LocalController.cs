using AutoMapper;
using EMS.APPLICATION.Dtos;
using EMS.APPLICATION.Extensions;
using EMS.APPLICATION.Features.Employee.Commands;
using EMS.APPLICATION.Features.Employee.Queries;
using EMS.APPLICATION.Features.Local.Commands;
using EMS.APPLICATION.Features.Local.Queries;
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
    public class LocalController(ISender sender, UserManager<AppUserEntity> userManager, IMapper mapper) : ControllerBase
    {

        [HttpGet()]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetUserLocalAsync(int pageNumber, int pageSize, string searchTerm = null)
        {
            var username = User.GetUsername();

            var appUser = await userManager.FindByNameAsync(username);

            var paginatedLocal = await sender.Send(new GetUserLocalQuery(appUser.Id, pageNumber, pageSize, searchTerm));

            var localDtos = mapper.Map<IEnumerable<LocalGetDto>>(paginatedLocal.Items);

            return Ok(new
            {
                LocalGet = localDtos,
                paginatedLocal.TotalItems,
                paginatedLocal.TotalPages,
                paginatedLocal.PageIndex
            });
        }

        [HttpPost()]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> AddLocalAsync([FromBody] LocalCreateDto localDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var username = User.GetUsername();

            var appUser = await userManager.FindByNameAsync(username);

            var localEntity = mapper.Map<LocalEntity>(localDto);

            localEntity.AppUserId = appUser.Id;
            localEntity.AppUserEntity = appUser;

            var result = await sender.Send(new AddLocalCommand(localEntity));

            return Ok(result);
        }

        [HttpDelete("{localId}")]
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> DeleteLocalAsync([FromRoute] Guid localId)
        {
            var result = await sender.Send(new DeleteLocalCommand(localId));

            return Ok(result);
        }
    }
}
