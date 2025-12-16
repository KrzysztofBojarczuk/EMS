using EMS.CORE.Entities;
using EMS.CORE.Enums;
using EMS.CORE.Interfaces;
using EMS.INFRASTRUCTURE.Extensions;
using MediatR;

namespace EMS.APPLICATION.Features.Task.Queries
{
    public record GetUserTasksQuery(string appUserId, int pageNumber, int pageSize, string searchTerm, List<StatusOfTask> statusOfTask, string sortOrder) : IRequest<PaginatedList<TaskEntity>>;

    public class GetUserTasksQueryHandler(ITaskRepository taskRepository) : IRequestHandler<GetUserTasksQuery, PaginatedList<TaskEntity>>
    {
        public async Task<PaginatedList<TaskEntity>> Handle(GetUserTasksQuery request, CancellationToken cancellationToken)
        {
            return await taskRepository.GetUserTasksAsync(request.appUserId, request.pageNumber, request.pageSize, request.searchTerm, request.statusOfTask, request.sortOrder);
        }
    }
}