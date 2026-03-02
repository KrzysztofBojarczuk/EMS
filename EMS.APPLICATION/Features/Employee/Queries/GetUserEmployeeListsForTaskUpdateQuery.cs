using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using MediatR;

namespace EMS.APPLICATION.Features.Employee.Queries
{
    public record GetUserEmployeeListsForTaskUpdateQuery(string appUserId, Guid taskId, string searchTerm) : IRequest<IEnumerable<EmployeeListsEntity>>;

    public class GetUserEmployeeListsForTaskUpdateQueryHandler(IEmployeeRepository employeeRepository) : IRequestHandler<GetUserEmployeeListsForTaskUpdateQuery, IEnumerable<EmployeeListsEntity>>
    {
        public async Task<IEnumerable<EmployeeListsEntity>> Handle(GetUserEmployeeListsForTaskUpdateQuery request, CancellationToken cancellationToken)
        {
            return await employeeRepository.GetUserEmployeeListsForTaskUpdateAsync(request.appUserId, request.taskId, request.searchTerm);
        }
    }
}