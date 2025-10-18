using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using MediatR;

namespace EMS.APPLICATION.Features.Task.Commands
{
    public record AddTaskCommand(TaskEntity Task, List<Guid> EmployeeListIds) : IRequest<TaskEntity>;

    public class AddTaskCommandHandler(ITaskRepository taskRepository, IPublisher mediator) : IRequestHandler<AddTaskCommand, TaskEntity>
    {
        public async Task<TaskEntity> Handle(AddTaskCommand request, CancellationToken cancellationToken)
        {
            return await taskRepository.AddTaskAsync(request.Task, request.EmployeeListIds);
        }
    }
}
