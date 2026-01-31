using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using MediatR;

namespace EMS.APPLICATION.Features.Employee.Queries
{
    public record GetUserEmployeesForListUpdateQuery(string appUserId, Guid employeeListId, string searchTerm) : IRequest<IEnumerable<EmployeeEntity>>;

    public class GetUserEmployeesForListUpdateQueryHandler(IEmployeeRepository employeeRepository) : IRequestHandler<GetUserEmployeesForListUpdateQuery, IEnumerable<EmployeeEntity>>
    {
        public async Task<IEnumerable<EmployeeEntity>> Handle(GetUserEmployeesForListUpdateQuery request, CancellationToken cancellationToken)
        {
            return await employeeRepository.GetUserEmployeesForListUpdateAsync(request.appUserId, request.employeeListId, request.searchTerm);
        }
    }
}