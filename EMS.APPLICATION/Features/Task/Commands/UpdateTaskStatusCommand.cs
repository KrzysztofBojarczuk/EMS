using EMS.CORE.Enums;
using EMS.CORE.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.APPLICATION.Features.Task.Commands
{
    public record UpdateTaskStatusCommand(Guid TaskId, StatusOfTask newStatus) : IRequest<bool>;

    public class UpdateTaskStatusCommandHandler(ITaskRepository taskRepository) : IRequestHandler<UpdateTaskStatusCommand, bool>
    {
        public async Task<bool> Handle(UpdateTaskStatusCommand request, CancellationToken cancellationToken)
        {
            return await taskRepository.UpdateTaskStatusAsync(request.TaskId, request.newStatus);
        }
    }
}

