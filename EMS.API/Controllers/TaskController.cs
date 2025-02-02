using AutoMapper;
using EMS.APPLICATION.Dtos;
using EMS.APPLICATION.Extensions;
using EMS.APPLICATION.Features.Employee.Queries;
using EMS.APPLICATION.Features.Task.Commands;
using EMS.APPLICATION.Features.Task.Queries;
using EMS.CORE.Entities;
using EMS.CORE.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController(ISender sender, UserManager<AppUserEntity> userManager, IMapper mapper) : ControllerBase
    {
        [HttpGet("User")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetUserTaskssAsync(string searchTerm = null)
        {
            var username = User.GetUsername();

            var appUser = await userManager.FindByNameAsync(username);

            var result = await sender.Send(new GetUserTasksQuery(appUser.Id, searchTerm));

            var taskDtos = mapper.Map<IEnumerable<TaskGetDto>>(result);

            return Ok(taskDtos);
        }

        [HttpPost()]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> AddTaskAsync([FromBody] TaskCreateDto taskDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var username = User.GetUsername();

            var appUser = await userManager.FindByNameAsync(username);

            var taskEntity = mapper.Map<TaskEntity>(taskDto);

            taskEntity.AppUserId = appUser.Id;
            taskEntity.AppUserEntity = appUser;

            var result = await sender.Send(new AddTaskCommand(taskEntity, taskDto.EmployeeListIds));

            return Ok(result);
        }

        [HttpPut("{taskId}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> UpdateTaskAsync([FromRoute] Guid taskId, [FromBody] TaskCreateDto updateTaskDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var taskEntity = mapper.Map<TaskEntity>(updateTaskDto);

            var result = await sender.Send(new UpdateTaskCommand(taskId, taskEntity));

            return Ok(result);
        }

        [HttpPatch("{taskId}/status")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> UpdateTaskStatusAsync([FromRoute] Guid taskId, [FromBody] StatusOfTask newStatus)
        {
            var result = await sender.Send(new UpdateTaskStatusCommand(taskId, newStatus));

            return Ok(result);
        }

        [HttpDelete("{taskId}")]
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> DeleteTaskAsync([FromRoute] Guid taskId)
        {
            var result = await sender.Send(new DeleteTaskCommand(taskId));

            return Ok(result);
        }

        [HttpGet()]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllTaskAsync(int pageNumber, int pageSize, string searchTerm = null)
        {
            var paginatedTasks = await sender.Send(new GetAllTasksQuery(pageNumber, pageSize, searchTerm));

            var taskDtos = mapper.Map<IEnumerable<TaskGetDto>>(paginatedTasks.Items);

            return Ok(new
            {
                TaskGet = taskDtos,
                paginatedTasks.TotalItems,
                paginatedTasks.TotalPages,
                paginatedTasks.PageIndex
            });
        }
    }
}
