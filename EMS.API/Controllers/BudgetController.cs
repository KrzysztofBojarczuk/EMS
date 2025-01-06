using AutoMapper;
using EMS.APPLICATION.Dtos;
using EMS.APPLICATION.Extensions;
using EMS.APPLICATION.Features.Budget.Commands;
using EMS.APPLICATION.Features.Budget.Queries;
using EMS.APPLICATION.Features.Employee.Commands;
using EMS.APPLICATION.Features.Employee.Queries;
using EMS.APPLICATION.Features.Task.Queries;
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
    public class BudgetController(ISender sender, UserManager<AppUserEntity> userManager, IMapper mapper) : ControllerBase
    {
        [HttpPost()]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> AddUserBudgetAsync([FromBody] BudgetCreateDto createDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var username = User.GetUsername();

            var appUser = await userManager.FindByNameAsync(username);

            var budgetEntity = mapper.Map<BudgetEntity>(createDto);

            budgetEntity.AppUserId = appUser.Id;
            budgetEntity.AppUserEntity = appUser;

            var result = await sender.Send(new AddBudgetCommand(budgetEntity));

            return Ok(result);
        }

        [HttpGet("User")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetUserBudgetAsync()
        {
            var username = User.GetUsername();

            var appUser = await userManager.FindByNameAsync(username);

            var result = await sender.Send(new GetUserBudgetQuery(appUser.Id));

            var budgetDto = mapper.Map<BudgetGetDto>(result);

            return Ok(budgetDto);
        }

        [HttpDelete("{budgetId}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> DeleteBudgetAsync([FromRoute] Guid budgetId)
        {
            var result = await sender.Send(new DeleteBudgetCommand(budgetId));

            return Ok(result);
        }
    }
}
