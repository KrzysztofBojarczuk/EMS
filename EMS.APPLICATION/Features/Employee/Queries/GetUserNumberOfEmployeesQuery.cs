using EMS.CORE.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.APPLICATION.Features.Employee.Queries
{
    public record GetUserNumberOfEmployeesQuery(string appUserId) : IRequest<int>;

    public class GetUserNumberOfEmployeesQueryHandler(IEmployeeRepository employeeRepository) : IRequestHandler<GetUserNumberOfEmployeesQuery, int>
    {
        public async Task<int> Handle(GetUserNumberOfEmployeesQuery reguest, CancellationToken cancellationToken)
        {
            return await employeeRepository.GetUserNumberOfEmployeesAsync(reguest.appUserId);
        }
    }
}
