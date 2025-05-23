﻿using EMS.CORE.Interfaces;
using MediatR;

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
