using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.APPLICATION.Features.Task.Commands
{
    public record AddTaskCommand(TaskEntity Task, List<Guid> EmployeeListIds) : IRequest<TaskEntity>;

    public class AddTaskCommandHandler(ITaskRepository taskRepository, IPublisher mediator)
        : IRequestHandler<AddTaskCommand, TaskEntity>
    {
        public async Task<TaskEntity> Handle(AddTaskCommand request, CancellationToken cancellationToken)
        {
            var task = await taskRepository.AddTaskAsync(request.Task, request.EmployeeListIds);
            return task;
        }
    }
}
