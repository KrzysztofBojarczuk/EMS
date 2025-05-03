using EMS.CORE.Interfaces;
using MediatR;

namespace EMS.APPLICATION.Features.Task.Commands
{
    public record DeleteTaskCommand(Guid taskId) : IRequest<bool>;

    public class DeleteTaskCommandHandler(ITaskRepository taskRepository) : IRequestHandler<DeleteTaskCommand, bool>
    {
        public async Task<bool> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
        {
            return await taskRepository.DeleteTaskAsync(request.taskId);
        }
    }
}
