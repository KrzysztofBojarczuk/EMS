using EMS.APPLICATION.Features.Logs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogsController(ISender sender) : ControllerBase
    {
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetAuditLogsAsync([FromQuery] int pageNumber, [FromQuery] int pageSize, [FromQuery] string searchTerm = null, [FromQuery] DateTime? dateFrom = null, [FromQuery] DateTime? dateTo = null, [FromQuery] string sortOrder = null)
        {
            var paginatedLogs = await sender.Send(new GetLogsQuery(pageNumber, pageSize, searchTerm, dateFrom, dateTo, sortOrder));

            return Ok(new
            {
                Logs = paginatedLogs.Items,
                paginatedLogs.TotalItems,
                paginatedLogs.TotalPages,
                paginatedLogs.PageIndex
            });
        }
    }
}