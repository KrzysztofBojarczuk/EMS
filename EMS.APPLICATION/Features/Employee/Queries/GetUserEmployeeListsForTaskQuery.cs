﻿using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using MediatR;

namespace EMS.APPLICATION.Features.Employee.Queries
{
    public record GetUserEmployeeListsForTaskQuery(string appUserId, string searchTerm) : IRequest<IEnumerable<EmployeeListsEntity>>;

    public class GetUserEmployeeListsForTaskQueryHandler(IEmployeeRepository employeeRepository)
            : IRequestHandler<GetUserEmployeeListsForTaskQuery, IEnumerable<EmployeeListsEntity>>
    {
        public async Task<IEnumerable<EmployeeListsEntity>> Handle(GetUserEmployeeListsForTaskQuery request, CancellationToken cancellationToken)
        {
            return await employeeRepository.GetUserEmployeeListsForTaskAsync(request.appUserId, request.searchTerm);
        }
    }
}
