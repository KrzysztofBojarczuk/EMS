using EMS.CORE.Entities;
using EMS.CORE.Enums;
using EMS.CORE.Interfaces;
using EMS.INFRASTRUCTURE.Extensions;
using MediatR;

namespace EMS.APPLICATION.Features.Task.Queries
{
    public record GetAllTasksQuery(int pageNumber, int pageSize, string searchTerm, List<StatusOfTask> statusOfTask, string sortOrder) : IRequest<PaginatedList<TaskEntity>>;

    public class GetAllTasksQueryHandler(ITaskRepository taskRepository) : IRequestHandler<GetAllTasksQuery, PaginatedList<TaskEntity>>
    {
        public async Task<PaginatedList<TaskEntity>> Handle(GetAllTasksQuery request, CancellationToken cancellationToken)
        {
            return await taskRepository.GetAllTasksAsync(request.pageNumber, request.pageSize, request.searchTerm, request.statusOfTask, request.sortOrder);
        }
    }
}