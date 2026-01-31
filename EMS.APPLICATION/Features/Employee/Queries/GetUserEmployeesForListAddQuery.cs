using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using MediatR;

namespace EMS.APPLICATION.Features.Employee.Queries
{
    public record GetUserEmployeesForListAddQuery(string appUserId, string searchTerm) : IRequest<IEnumerable<EmployeeEntity>>;

    public class GetUserEmployeesForListAddQueryHandler(IEmployeeRepository employeeRepository) : IRequestHandler<GetUserEmployeesForListAddQuery, IEnumerable<EmployeeEntity>>
    {
        public async Task<IEnumerable<EmployeeEntity>> Handle(GetUserEmployeesForListAddQuery request, CancellationToken cancellationToken)
        {
            return await employeeRepository.GetUserEmployeesForListAddAsync(request.appUserId, request.searchTerm);
        }
    }
}