using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.APPLICATION.Features.Task.Queries
{
    public record GetUserTasksQuery(string appUserId) : IRequest<IEnumerable<TaskEntity>>;

    public class GetUserTasksQueryHandler(ITaskRepository taskRepository)
        : IRequestHandler<GetUserTasksQuery, IEnumerable<TaskEntity>>
    {
        public async Task<IEnumerable<TaskEntity>> Handle(GetUserTasksQuery request, CancellationToken cancellationToken)
        {
            return await taskRepository.GetUserTasksAsync(request.appUserId);
        }
    }
}
