using AutoMapper;
using EMS.APPLICATION.Dtos;
using EMS.APPLICATION.Extensions;
using EMS.APPLICATION.Features.Budget.Commands;
using EMS.APPLICATION.Features.Budget.Queries;
using EMS.CORE.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BudgetController(ISender sender, UserManager<AppUserEntity> userManager, IMapper mapper) : ControllerBase
    {
        [HttpPost()]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BudgetGetDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> AddBudgetAsync([FromBody] BudgetCreateDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var username = User.GetUsername();

            var appUser = await userManager.FindByNameAsync(username);

            var budgetEntity = mapper.Map<BudgetEntity>(createDto);

            budgetEntity.AppUserId = appUser.Id;

            var result = await sender.Send(new AddBudgetCommand(budgetEntity));

            var budgetGet = mapper.Map<BudgetGetDto>(result);

            return Ok(budgetGet);
        }

        [HttpGet("User")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetUserBudgetAsync()
        {
            var username = User.GetUsername();

            var appUser = await userManager.FindByNameAsync(username);

            var result = await sender.Send(new GetUserBudgetQuery(appUser.Id));

            var budgetGet = mapper.Map<BudgetGetDto>(result);

            return Ok(budgetGet);
        }

        [HttpDelete("{budgetId}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteBudgetAsync([FromRoute] Guid budgetId)
        {
            var username = User.GetUsername();

            var appUser = await userManager.FindByNameAsync(username);

            var result = await sender.Send(new DeleteBudgetCommand(budgetId, appUser.Id));

            return result ? Ok(result) : NotFound(result);
        }
    }
}