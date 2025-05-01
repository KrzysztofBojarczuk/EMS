using EMS.CORE.Interfaces;
using MediatR;

namespace EMS.APPLICATION.Features.Employee.Queries
{
    public record GetNumberOfEmployeesQuery() : IRequest<int>;

    public class GetNumberOfEmployeesQueryHandler(IEmployeeRepository employeeRepository)
        : IRequestHandler<GetNumberOfEmployeesQuery, int>
    {
        public async Task<int> Handle(GetNumberOfEmployeesQuery request, CancellationToken cancellationToken)
        {
            return await employeeRepository.GetNumberOfEmployeesAsync();
        }
    }
}
