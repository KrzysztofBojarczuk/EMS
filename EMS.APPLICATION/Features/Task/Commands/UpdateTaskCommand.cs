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
    public record UpdateTaskCommand(Guid TaskId, TaskEntity Task) : IRequest<TaskEntity>;

    public class UpdateTaskCommandHandler(ITaskRepository taskRepository) : IRequestHandler<UpdateTaskCommand, TaskEntity>
    {
        public async Task<TaskEntity> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
        {
            return await taskRepository.UpdateTaskAsync(request.TaskId, request.Task);
        }
    }
}
