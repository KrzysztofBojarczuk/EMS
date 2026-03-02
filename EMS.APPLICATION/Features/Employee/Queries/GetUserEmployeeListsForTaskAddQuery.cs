using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using MediatR;

namespace EMS.APPLICATION.Features.Employee.Queries
{
    public record GetUserEmployeeListsForTaskAddQuery(string appUserId, string searchTerm) : IRequest<IEnumerable<EmployeeListsEntity>>;

    public class GetUserEmployeeListsForTaskAddQueryHandler(IEmployeeRepository employeeRepository) : IRequestHandler<GetUserEmployeeListsForTaskAddQuery, IEnumerable<EmployeeListsEntity>>
    {
        public async Task<IEnumerable<EmployeeListsEntity>> Handle(GetUserEmployeeListsForTaskAddQuery request, CancellationToken cancellationToken)
        {
            return await employeeRepository.GetUserEmployeeListsForTaskAddAsync(request.appUserId, request.searchTerm);
        }
    }
}