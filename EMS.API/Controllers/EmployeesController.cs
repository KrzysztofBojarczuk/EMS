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
    [Authorize]
    public class EmployeesController(ISender sender, UserManager<AppUserEntity> userManager, IMapper mapper) : ControllerBase
    {
        [HttpGet("User")]
        public async Task<IActionResult> GetUserEmployeesAsync()
        {
            var username = User.GetUsername();

            var appUser = await userManager.FindByNameAsync(username);

            var result = await sender.Send(new GetUserEmployeesQuery(appUser.Id));

            var employeeDtos = mapper.Map<IEnumerable<EmployeeGetDto>>(result);

            return Ok(employeeDtos);
        }


        [HttpGet("GetUserNumberOfEmployee")]
        public async Task<IActionResult> GetUserNumberOfEmployeesAsync()
        {
            var username = User.GetUsername();

            var appUser = await userManager.FindByNameAsync(username);

            var result = await sender.Send(new GetUserNumberOfEmployeesQuery(appUser.Id));

            return Ok(result);
        }

        [HttpPost()]
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
        public async Task<IActionResult> GetAllEmployeesAsync()
        {
            var employees = await sender.Send(new GetAllEmployeesQuery());

            var employeeDtos = mapper.Map<IEnumerable<EmployeeGetDto>>(employees);

            return Ok(employeeDtos);
        }

        [HttpGet("GetNumberOfEmployee")]
        public async Task<IActionResult> GetNumberOfEmployeesAsync()
        {
            var result = await sender.Send(new GetNumberOfEmployeesQuery());

            return Ok(result);
        }

        [HttpGet("{employeeId}")]
        public async Task<IActionResult> GetEmployeeByIdAsync([FromRoute] Guid employeeId)
        {
            var result = await sender.Send(new GetEmployeeByIdQuery(employeeId));

            var employeeDtos = mapper.Map<EmployeeGetDto>(result);

            return Ok(employeeDtos);
        }

        [HttpPut("{employeeId}")]
        public async Task<IActionResult> UpdateEmployeeAsync([FromRoute] Guid employeeId, [FromBody] EmployeeCreateDto updateEmployeeDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var employeeEntity = mapper.Map<EmployeeEntity>(updateEmployeeDto);

            var result = await sender.Send(new UpdateEmployeeCommand(employeeId, employeeEntity));

            return Ok(result);
        }

        [HttpDelete("{employeeId}")]
        public async Task<IActionResult> DeleteEmployeeAsync([FromRoute] Guid employeeId)
        {
            var result = await sender.Send(new DeleteEmployeeCommand(employeeId));

            return Ok(result);
        }
    }
}
