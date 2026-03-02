using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using MediatR;

namespace EMS.APPLICATION.Features.Task.Commands
{
    public record UpdateTaskCommand(Guid taskId, string appUserId, TaskEntity task, List<Guid> employeeListIds, List<Guid> vehicleIds) : IRequest<TaskEntity>;

    public class UpdateTaskCommandHandler(ITaskRepository taskRepository) : IRequestHandler<UpdateTaskCommand, TaskEntity>
    {
        public async Task<TaskEntity> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
        {
            return await taskRepository.UpdateTaskAsync(request.taskId, request.appUserId, request.task, request.employeeListIds, request.vehicleIds);
        }
    }
}