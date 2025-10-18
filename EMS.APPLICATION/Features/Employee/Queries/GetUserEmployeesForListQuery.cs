using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using MediatR;

namespace EMS.APPLICATION.Features.Employee.Queries
{

   public record GetUserEmployeesForListQuery(string appUserId, string searchTerm) : IRequest<IEnumerable<EmployeeEntity>>;

    public class GetUserEmployeesForListQueryHandler(IEmployeeRepository employeeRepository) : IRequestHandler<GetUserEmployeesForListQuery, IEnumerable<EmployeeEntity>>
    {
        public async Task<IEnumerable<EmployeeEntity>> Handle(GetUserEmployeesForListQuery request, CancellationToken cancellationToken)
        {
            return await employeeRepository.GetUserEmployeesForListAsync(request.appUserId, request.searchTerm);
        }
    }
}
