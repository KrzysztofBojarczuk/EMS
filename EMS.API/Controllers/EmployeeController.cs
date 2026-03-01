using AutoMapper;
using EMS.APPLICATION.Dtos;
using EMS.APPLICATION.Extensions;
using EMS.APPLICATION.Features.Employee.Commands;
using EMS.APPLICATION.Features.Employee.Queries;
using EMS.CORE.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController(ISender sender, UserManager<AppUserEntity> userManager, IMapper mapper) : ControllerBase
    {
        [HttpPost()]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EmployeeGetDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> AddEmployeeAsync([FromBody] EmployeeCreateDto employeeDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var username = User.GetUsername();

            var appUser = await userManager.FindByNameAsync(username);

            var employeeEntity = mapper.Map<EmployeeEntity>(employeeDto);

            employeeEntity.AppUserId = appUser.Id;

            var result = await sender.Send(new AddEmployeeCommand(employeeEntity));

            var employeeGet = mapper.Map<EmployeeGetDto>(result);

            return Ok(employeeGet);
        }

        [HttpPost("AddEmployeeList")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EmployeeListsGetDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> AddEmployeeListAsync([FromBody] EmployeeListsCreateDto employeeListsDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var username = User.GetUsername();

            var appUser = await userManager.FindByNameAsync(username);

            var employeeListsEntity = mapper.Map<EmployeeListsEntity>(employeeListsDto);

            employeeListsEntity.AppUserId = appUser.Id;

            var result = await sender.Send(new AddEmployeeListCommand(employeeListsEntity, employeeListsDto.EmployeeIds));

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            var employeeListsGet = mapper.Map<EmployeeListsGetDto>(result.Value);

            return Ok(employeeListsGet);
        }

        [HttpGet("{employeeId}")]
        [Authorize(Roles = "User, Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetEmployeeByIdAsync([FromRoute] Guid employeeId)
        {
            var result = await sender.Send(new GetEmployeeByIdQuery(employeeId));

            var employeeGet = mapper.Map<EmployeeGetDto>(result);

            return Ok(employeeGet);
        }

        [HttpGet("User")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetUserEmployeesAsync([FromQuery] int pageNumber, [FromQuery] int pageSize, [FromQuery] string searchTerm = null, [FromQuery] string sortOrder = null)
        {
            var username = User.GetUsername();

            var appUser = await userManager.FindByNameAsync(username);

            var paginatedEmployees = await sender.Send(new GetUserEmployeesQuery(appUser.Id, pageNumber, pageSize, searchTerm, sortOrder));

            var employeeGet = mapper.Map<IEnumerable<EmployeeGetDto>>(paginatedEmployees.Items);

            return Ok(new
            {
                EmployeeGet = employeeGet,
                paginatedEmployees.TotalItems,
                paginatedEmployees.TotalPages,
                paginatedEmployees.PageIndex
            });
        }

        [HttpGet()]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetAllEmployeesAsync([FromQuery] int pageNumber, [FromQuery] int pageSize, [FromQuery] string searchTerm = null, [FromQuery] string sortOrder = null)
        {
            var paginatedEmployees = await sender.Send(new GetAllEmployeesQuery(pageNumber, pageSize, searchTerm, sortOrder));

            var employeeGet = mapper.Map<IEnumerable<EmployeeGetDto>>(paginatedEmployees.Items);

            return Ok(new
            {
                EmployeeGet = employeeGet,
                paginatedEmployees.TotalItems,
                paginatedEmployees.TotalPages,
                paginatedEmployees.PageIndex
            });
        }

        [HttpGet("UserEmployeeList")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetUserEmployeeListAsync([FromQuery] string searchTerm = null)
        {
            var username = User.GetUsername();

            var appUser = await userManager.FindByNameAsync(username);

            var result = await sender.Send(new GetUserEmployeeListsQuery(appUser.Id, searchTerm));

            var listEmployeeGet = mapper.Map<IEnumerable<EmployeeListsGetDto>>(result);

            return Ok(listEmployeeGet);
        }

        [HttpGet("UserEmployeeListForTask")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetUserEmployeeListForTaskAsync([FromQuery] string searchTerm = null)
        {
            var username = User.GetUsername();

            var appUser = await userManager.FindByNameAsync(username);

            var result = await sender.Send(new GetUserEmployeeListsForTaskQuery(appUser.Id, searchTerm));

            var listEmployeeGet = mapper.Map<IEnumerable<EmployeeListsGetDto>>(result);

            return Ok(listEmployeeGet);
        }

        [HttpGet("UserListAdd")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetUserEmployeesForListAddAsync([FromQuery] string searchTerm = null)
        {
            var username = User.GetUsername();

            var appUser = await userManager.FindByNameAsync(username);

            var result = await sender.Send(new GetUserEmployeesForListAddQuery(appUser.Id, searchTerm));

            var employeeGet = mapper.Map<IEnumerable<EmployeeGetDto>>(result);

            return Ok(employeeGet);
        }

        [HttpGet("UserListUpdate/{employeeListId}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetUserEmployeesForListUpdateAsync([FromRoute] Guid employeeListId, [FromQuery] string searchTerm = null)
        {
            var username = User.GetUsername();

            var appUser = await userManager.FindByNameAsync(username);

            var result = await sender.Send(new GetUserEmployeesForListUpdateQuery(appUser.Id, employeeListId, searchTerm));

            var employeeGet = mapper.Map<IEnumerable<EmployeeGetDto>>(result);

            return Ok(employeeGet);
        }

        [HttpGet("GetUserNumberOfEmployee")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetUserNumberOfEmployeesAsync()
        {
            var username = User.GetUsername();

            var appUser = await userManager.FindByNameAsync(username);

            var result = await sender.Send(new GetUserNumberOfEmployeesQuery(appUser.Id));

            return Ok(result);
        }

        [HttpGet("GetNumberOfEmployee")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetNumberOfEmployeesAsync()
        {
            var result = await sender.Send(new GetNumberOfEmployeesQuery());

            return Ok(result);
        }

        [HttpPut("{employeeId}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EmployeeGetDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> UpdateEmployeeAsync([FromRoute] Guid employeeId, [FromBody] EmployeeCreateDto updateEmployeeDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var username = User.GetUsername();

            var appUser = await userManager.FindByNameAsync(username);

            var employeeEntity = mapper.Map<EmployeeEntity>(updateEmployeeDto);

            var result = await sender.Send(new UpdateEmployeeCommand(employeeId, appUser.Id, employeeEntity));

            var employeeGet = mapper.Map<EmployeeGetDto>(result);

            return Ok(employeeGet);
        }

        [HttpPut("EmployeeList/{employeeListId}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EmployeeListsGetDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> UpdateEmployeeList([FromRoute] Guid employeeListId, [FromBody] EmployeeListsCreateDto employeeListsDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var username = User.GetUsername();

            var appUser = await userManager.FindByNameAsync(username);

            var employeeListsEntity = mapper.Map<EmployeeListsEntity>(employeeListsDto);

            var result = await sender.Send(new UpdateEmployeeListCommand(employeeListId, appUser.Id, employeeListsEntity, employeeListsDto.EmployeeIds));

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            var employeeListsGet = mapper.Map<EmployeeListsGetDto>(result.Value);

            return Ok(employeeListsGet);
        }

        [HttpDelete("{employeeId}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteEmployeeAsync([FromRoute] Guid employeeId)
        {
            var username = User.GetUsername();

            var appUser = await userManager.FindByNameAsync(username);

            var result = await sender.Send(new DeleteEmployeeCommand(employeeId, appUser.Id));

            return result ? Ok(result) : NotFound(result);
        }

        [HttpDelete("EmployeeList/{employeeListId}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteEmployeeListAsync([FromRoute] Guid employeeListId)
        {
            var username = User.GetUsername();

            var appUser = await userManager.FindByNameAsync(username);

            var result = await sender.Send(new DeleteEmployeeListCommand(employeeListId, appUser.Id));

            return result ? Ok(result) : NotFound(result);
        }
    }
}