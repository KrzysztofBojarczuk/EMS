using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using EMS.INFRASTRUCTURE.Extensions;
using MediatR;

namespace EMS.APPLICATION.Features.Task.Queries
{
    public record GetAllTasksQuery(int pageNumber, int pageSize, string searchTerm) : IRequest<PaginatedList<TaskEntity>>;

    public class GetAllTasksQueryHandler(ITaskRepository taskRepository)
        : IRequestHandler<GetAllTasksQuery, PaginatedList<TaskEntity>>
    {
        public async Task<PaginatedList<TaskEntity>> Handle(GetAllTasksQuery request, CancellationToken cancellationToken)
        {
            return await taskRepository.GetAllTasksAsync(request.pageNumber, request.pageSize, request.searchTerm);
        }
    }
}
