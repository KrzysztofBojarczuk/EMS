using AutoMapper;
using EMS.APPLICATION.Dtos;
using EMS.APPLICATION.Extensions;
using EMS.APPLICATION.Features.Task.Commands;
using EMS.APPLICATION.Features.Task.Queries;
using EMS.CORE.Entities;
using EMS.CORE.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController(ISender sender, UserManager<AppUserEntity> userManager, IMapper mapper) : ControllerBase
    {
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

            var result = await sender.Send(new AddTaskCommand(taskEntity, taskDto.EmployeeListIds, taskDto.VehicleIds));

            var taskGet = mapper.Map<TaskGetDto>(result);

            return Ok(taskGet);
        }

        [HttpGet("User")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetUserTaskssAsync([FromQuery] int pageNumber, [FromQuery] int pageSize, [FromQuery] string searchTerm = null, [FromQuery] List<StatusOfTask> statusOfTask = null, [FromQuery] string sortOrder = null)
        {
            var username = User.GetUsername();

            var appUser = await userManager.FindByNameAsync(username);

            var paginatedTasks = await sender.Send(new GetUserTasksQuery(appUser.Id, pageNumber, pageSize, searchTerm, statusOfTask, sortOrder));

            var taskGet = mapper.Map<IEnumerable<TaskGetDto>>(paginatedTasks.Items);

            return Ok(new
            {
                TaskGet = taskGet,
                paginatedTasks.TotalItems,
                paginatedTasks.TotalPages,
                paginatedTasks.PageIndex
            });
        }

        [HttpGet()]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllTaskAsync([FromQuery] int pageNumber, [FromQuery] int pageSize, [FromQuery] string searchTerm = null, [FromQuery] List<StatusOfTask> statusOfTask = null, [FromQuery] string sortOrder = null)
        {
            var paginatedTasks = await sender.Send(new GetAllTasksQuery(pageNumber, pageSize, searchTerm, statusOfTask, sortOrder));

            var taskGet = mapper.Map<IEnumerable<TaskGetDto>>(paginatedTasks.Items);

            return Ok(new
            {
                TaskGet = taskGet,
                paginatedTasks.TotalItems,
                paginatedTasks.TotalPages,
                paginatedTasks.PageIndex
            });
        }

        [HttpPut("{taskId}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> UpdateTaskAsync([FromRoute] Guid taskId, [FromBody] TaskCreateDto updateTaskDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var username = User.GetUsername();

            var appUser = await userManager.FindByNameAsync(username);

            var taskEntity = mapper.Map<TaskEntity>(updateTaskDto);

            var result = await sender.Send(new UpdateTaskCommand(taskId, appUser.Id, taskEntity));

            var taskGet = mapper.Map<TaskGetDto>(result);

            return Ok(taskGet);
        }

        [HttpPatch("{taskId}/status")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> UpdateTaskStatusAsync([FromRoute] Guid taskId, [FromBody] StatusOfTask newStatus)
        {
            var username = User.GetUsername();

            var appUser = await userManager.FindByNameAsync(username);

            var result = await sender.Send(new UpdateTaskStatusCommand(taskId, appUser.Id, newStatus));

            return Ok(result);
        }

        [HttpDelete("{taskId}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> DeleteTaskAsync([FromRoute] Guid taskId)
        {
            var username = User.GetUsername();

            var appUser = await userManager.FindByNameAsync(username);

            var result = await sender.Send(new DeleteTaskCommand(taskId, appUser.Id));

            return Ok(result);
        }
    }
}