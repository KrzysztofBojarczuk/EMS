using AutoMapper;
using EMS.APPLICATION.Dtos;
using EMS.APPLICATION.Extensions;
using EMS.APPLICATION.Features.Task.Commands;
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
    [Authorize(Roles = "User")]
    public class TaskController(ISender sender, UserManager<AppUserEntity> userManager, IMapper mapper) : ControllerBase
    {
        [HttpGet("User")]
        public async Task<IActionResult> GetUserTaskssAsync()
        {
            var username = User.GetUsername();

            var appUser = await userManager.FindByNameAsync(username);

            var result = await sender.Send(new GetUserTasksQuery(appUser.Id));

            var employeeDtos = mapper.Map<IEnumerable<TaskGetDto>>(result);

            return Ok(employeeDtos);
        }

        [HttpPost()]
        public async Task<IActionResult> AddTaskAsync([FromBody] TaskCreateDto taskDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var username = User.GetUsername();

            var appUser = await userManager.FindByNameAsync(username);

            var taskEntity = mapper.Map<TaskEntity>(taskDto);

            taskEntity.AppUserId = appUser.Id;
            taskEntity.AppUserEntity = appUser;

            var result = await sender.Send(new AddTaskCommand(taskEntity));

            return Ok(result);
        }

        [HttpPut("{taskId}")]
        public async Task<IActionResult> UpdateTaskAsync([FromRoute] Guid taskId, [FromBody] TaskCreateDto updateTaskDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var taskEntity = mapper.Map<TaskEntity>(updateTaskDto);

            var result = await sender.Send(new UpdateTaskCommand(taskId, taskEntity));

            return Ok(result);
        }

        [HttpDelete("{taskId}")]
        public async Task<IActionResult> DeleteTaskAsync([FromRoute] Guid taskId)
        {
            var result = await sender.Send(new DeleteTaskCommand(taskId));

            return Ok(result);
        }
    }
}
