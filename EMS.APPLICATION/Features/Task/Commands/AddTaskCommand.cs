using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using MediatR;

namespace EMS.APPLICATION.Features.Task.Commands
{
    public record AddTaskCommand(TaskEntity task, List<Guid> employeeListIds, List<Guid> vehicleIds) : IRequest<TaskEntity>;

    public class AddTaskCommandHandler(ITaskRepository taskRepository) : IRequestHandler<AddTaskCommand, TaskEntity>
    {
        public async Task<TaskEntity> Handle(AddTaskCommand request, CancellationToken cancellationToken)
        {
            return await taskRepository.AddTaskAsync(request.task, request.employeeListIds, request.vehicleIds);
        }
    }
}