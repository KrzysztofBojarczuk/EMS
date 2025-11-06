using AutoMapper;
using EMS.APPLICATION.Dtos;
using EMS.APPLICATION.Extensions;
using EMS.APPLICATION.Features.Local.Commands;
using EMS.APPLICATION.Features.Local.Queries;
using EMS.CORE.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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
        public async Task<IActionResult> GetUserLocalAsync([FromQuery] int pageNumber, [FromQuery] int pageSize, [FromQuery] string searchTerm = null)
        {
            var username = User.GetUsername();

            var appUser = await userManager.FindByNameAsync(username);

            var paginatedLocal = await sender.Send(new GetUserLocalQuery(appUser.Id, pageNumber, pageSize, searchTerm));

            var localGet = mapper.Map<IEnumerable<LocalGetDto>>(paginatedLocal.Items);

            return Ok(new
            {
                LocalGet = localGet,
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

            var result = await sender.Send(new AddLocalCommand(localEntity));

            var localGet = mapper.Map<LocalGetDto>(result);

            return Ok(localGet);
        }

        [HttpPut("{localId}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> UpdateLocalAsync([FromRoute] Guid localId, [FromBody] LocalCreateDto updateLocalDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var username = User.GetUsername();

            var appUser = await userManager.FindByNameAsync(username);

            var localEntity = mapper.Map<LocalEntity>(updateLocalDto);

            var result = await sender.Send(new UpdateLocalCommand(localId, appUser.Id, localEntity));

            var localGet = mapper.Map<LocalGetDto>(result);

            return Ok(localGet);
        }

        [HttpDelete("{localId}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> DeleteLocalAsync([FromRoute] Guid localId)
        {
            var username = User.GetUsername();

            var appUser = await userManager.FindByNameAsync(username);

            var result = await sender.Send(new DeleteLocalCommand(localId, appUser.Id));

            return Ok(result);
        }
    }
}