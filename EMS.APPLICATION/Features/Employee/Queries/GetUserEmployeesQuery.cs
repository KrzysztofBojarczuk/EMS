using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using EMS.INFRASTRUCTURE.Extensions;
using MediatR;

namespace EMS.APPLICATION.Features.Employee.Queries
{
    public record GetUserEmployeesQuery(string appUserId, int pageNumber, int pageSize, string searchTerm) : IRequest<PaginatedList<EmployeeEntity>>;

    public class GetUserEmployeesQueryHandler(IEmployeeRepository employeeRepository)
        : IRequestHandler<GetUserEmployeesQuery, PaginatedList<EmployeeEntity>>
    {
        public async Task<PaginatedList<EmployeeEntity>> Handle(GetUserEmployeesQuery request, CancellationToken cancellationToken)
        {
            return await employeeRepository.GetUserEmployeesAsync(request.appUserId, request.pageNumber, request.pageSize, request.searchTerm);
        }
    }
}