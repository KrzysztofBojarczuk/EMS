using AutoMapper;
using EMS.APPLICATION.Dtos;
using EMS.APPLICATION.Extensions;
using EMS.APPLICATION.Features.Employee.Commands;
using EMS.APPLICATION.Features.Employee.Queries;
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
    public class EmployeeController(ISender sender, UserManager<AppUserEntity> userManager, IMapper mapper) : ControllerBase
    {
        [HttpGet("User")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetUserEmployeesAsync(int pageNumber, int pageSize, string searchTerm = null)
        {
            var username = User.GetUsername();

            var appUser = await userManager.FindByNameAsync(username);

            var paginatedEmployees = await sender.Send(new GetUserEmployeesQuery(appUser.Id, pageNumber, pageSize, searchTerm));

            var employeeDtos = mapper.Map<IEnumerable<EmployeeGetDto>>(paginatedEmployees.Items);

            return Ok(new
            {
                EmployeeGet = employeeDtos,
                paginatedEmployees.TotalItems,
                paginatedEmployees.TotalPages,
                paginatedEmployees.PageIndex
            });
        }

        [HttpGet("UserList")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetUserEmployeesForListAsync(string searchTerm = null)
        {
            var username = User.GetUsername();

            var appUser = await userManager.FindByNameAsync(username);

            var result = await sender.Send(new GetUserEmployeesForListQuery(appUser.Id, searchTerm));

            var employeeDtos = mapper.Map<IEnumerable<EmployeeGetDto>>(result);

            return Ok(employeeDtos);
        }

        [HttpGet("GetUserNumberOfEmployee")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetUserNumberOfEmployeesAsync()
        {
            var username = User.GetUsername();

            var appUser = await userManager.FindByNameAsync(username);

            var result = await sender.Send(new GetUserNumberOfEmployeesQuery(appUser.Id));

            return Ok(result);
        }

        [HttpPost()]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> AddEmployeeAsync([FromBody] EmployeeCreateDto employeeDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var username = User.GetUsername();

            var appUser = await userManager.FindByNameAsync(username);

            var employeeEntity = mapper.Map<EmployeeEntity>(employeeDto);

            employeeEntity.AppUserId = appUser.Id;
            employeeEntity.AppUserEntity = appUser;

            var result = await sender.Send(new AddEmployeeCommand(employeeEntity));

            return Ok(result);
        }

        [HttpGet()]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllEmployeesAsync(int pageNumber, int pageSize, string searchTerm = null)
        {
            var paginatedEmployees = await sender.Send(new GetAllEmployeesQuery(pageNumber, pageSize, searchTerm));

            var employeeDtos = mapper.Map<IEnumerable<EmployeeGetDto>>(paginatedEmployees.Items); 

            return Ok(new
            {
                EmployeeGet = employeeDtos,
                paginatedEmployees.TotalItems,
                paginatedEmployees.TotalPages,
                paginatedEmployees.PageIndex
            });
        }

        [HttpGet("GetNumberOfEmployee")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetNumberOfEmployeesAsync()
        {
            var result = await sender.Send(new GetNumberOfEmployeesQuery());

            return Ok(result);
        }

        [HttpGet("{employeeId}")]
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> GetEmployeeByIdAsync([FromRoute] Guid employeeId)
        {
            var result = await sender.Send(new GetEmployeeByIdQuery(employeeId));

            var employeeDtos = mapper.Map<EmployeeGetDto>(result);

            return Ok(employeeDtos);
        }

        [HttpPut("{employeeId}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> UpdateEmployeeAsync([FromRoute] Guid employeeId, [FromBody] EmployeeCreateDto updateEmployeeDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var employeeEntity = mapper.Map<EmployeeEntity>(updateEmployeeDto);

            var result = await sender.Send(new UpdateEmployeeCommand(employeeId, employeeEntity));

            return Ok(result);
        }

        [HttpDelete("{employeeId}")]
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> DeleteEmployeeAsync([FromRoute] Guid employeeId)
        {
            var result = await sender.Send(new DeleteEmployeeCommand(employeeId));

            return Ok(result);
        }

        [HttpGet("UserEmployeeListForTask")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetUserEmployeeListForTaskAsync(string searchTerm = null)
        {
            var username = User.GetUsername();

            var appUser = await userManager.FindByNameAsync(username);

            var result = await sender.Send(new GetUserEmployeeListsForTaskQuery(appUser.Id, searchTerm));

            var listEmployeeDtos = mapper.Map<IEnumerable<EmployeeListsGetDto>>(result);

            return Ok(listEmployeeDtos);
        }

        [HttpGet("UserEmployeeList")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetUserEmployeeListAsync(string searchTerm = null)
        {
            var username = User.GetUsername();

            var appUser = await userManager.FindByNameAsync(username);

            var result = await sender.Send(new GetUserEmployeeListsQuery(appUser.Id, searchTerm));

            var listEmployeeDtos = mapper.Map<IEnumerable<EmployeeListsGetDto>>(result);

            return Ok(listEmployeeDtos);
        }

        [HttpPost("AddEmployeeList")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> AddEmployeeListAsync([FromBody] EmployeeListsCreateDto employeeListsDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var username = User.GetUsername();

            var appUser = await userManager.FindByNameAsync(username);

            var employeeListsEntity = mapper.Map<EmployeeListsEntity>(employeeListsDto);

            employeeListsEntity.AppUserId = appUser.Id;
            employeeListsEntity.AppUserEntity = appUser;

            var result = await sender.Send(new AddEmployeeListCommand(employeeListsEntity, employeeListsDto.EmployeeIds));

            return Ok(result);
        }

        [HttpDelete("EmployeeList/{employeeListId}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> DeletedEmployeeListAsync([FromRoute] Guid employeeListId)
        {
            var result = await sender.Send(new DeleteEmployeeListCommand(employeeListId));

            return Ok(result);
        }
    }
}
