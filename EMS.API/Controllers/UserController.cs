using EMS.APPLICATION.Features.Userss.Commands;
using EMS.APPLICATION.Features.Userss.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(ISender sender) : ControllerBase
    {
        [HttpGet("GetAllUser")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUserAsync([FromQuery] int pageNumber, [FromQuery] int pageSize, [FromQuery] string searchTerm = null)
        {
            var result = await sender.Send(new GetAllUserQuery(pageNumber, pageSize, searchTerm));

            return Ok(new
            {
                userGet = result.Items,
                result.TotalItems,
                result.TotalPages,
                result.PageIndex
            });
        }

        [HttpGet("GetNumberOfUsers")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetNumberOfUsersAsync()
        {
            var result = await sender.Send(new GetNumberOfUsersQuery());

            return Ok(result);
        }

        [HttpDelete("{appUserId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUserAsync([FromRoute] string appUserId)
        {
            var result = await sender.Send(new DeleteUserCommand(appUserId));

            return Ok(result);
        }
    }
}