using EMS.CORE.Common;
using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using MediatR;

namespace EMS.APPLICATION.Features.Employee.Commands
{
    public record UpdateEmployeeListCommand(Guid employeeListId, string appUserId, EmployeeListsEntity employeeList, List<Guid> employeeIds) : IRequest<Result<EmployeeListsEntity>>;

    public class UpdateEmployeeListCommandHandler(IEmployeeRepository employeeRepository) : IRequestHandler<UpdateEmployeeListCommand, Result<EmployeeListsEntity>>
    {
        public async Task<Result<EmployeeListsEntity>> Handle(UpdateEmployeeListCommand request, CancellationToken cancellationToken)
        {
            var exists = await employeeRepository.EmployeeListExistsForUpdateAsync(request.employeeList.Name, request.appUserId, request.employeeListId);

            if (exists)
            {
                return Result<EmployeeListsEntity>.Failure("A list with that name already exists.");
            }

            var updatedEmployeeList = await employeeRepository.UpdateEmployeeListAsync(request.employeeListId, request.appUserId, request.employeeList, request.employeeIds);

            return Result<EmployeeListsEntity>.Success(updatedEmployeeList);
        }
    }
}