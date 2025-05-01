using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using EMS.INFRASTRUCTURE.Extensions;
using MediatR;

namespace EMS.APPLICATION.Features.Employee.Queries
{
    public record GetAllEmployeesQuery(int pageNumber, int pageSize, string searchTerm) : IRequest<PaginatedList<EmployeeEntity>>;

    public class GetAllEmployeesQueryHandler(IEmployeeRepository employeeRepository)
        : IRequestHandler<GetAllEmployeesQuery, PaginatedList<EmployeeEntity>>
    {
        public async Task<PaginatedList<EmployeeEntity>> Handle(GetAllEmployeesQuery request, CancellationToken cancellationToken)
        {
            return await employeeRepository.GetEmployeesAsync(request.pageNumber, request.pageSize, request.searchTerm);
        }
    }
}
