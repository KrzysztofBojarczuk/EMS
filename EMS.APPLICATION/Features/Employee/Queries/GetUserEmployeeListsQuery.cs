using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.APPLICATION.Features.Employee.Queries
{
    public record GetUserEmployeeListsQuery(string appUserId, string searchTerm) : IRequest<IEnumerable<EmployeeListsEntity>>;

    public class GetUserEmployeeListsQueryHandler(IEmployeeRepository employeeRepository)
            : IRequestHandler<GetUserEmployeeListsQuery, IEnumerable<EmployeeListsEntity>>
    {
        public async Task<IEnumerable<EmployeeListsEntity>> Handle(GetUserEmployeeListsQuery request, CancellationToken cancellationToken)
        {
            return await employeeRepository.GetUserEmployeeListsAsync(request.appUserId, request.searchTerm);
        }
    }
}
