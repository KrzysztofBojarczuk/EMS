using EMS.CORE.Common;
using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using MediatR;

namespace EMS.APPLICATION.Features.Employee.Commands
{
    public record AddEmployeeListCommand(EmployeeListsEntity employeeList, List<Guid> employeeIds) : IRequest<Result<EmployeeListsEntity>>;

    public class AddEmployeeListCommandHandler(IEmployeeRepository employeeRepository) : IRequestHandler<AddEmployeeListCommand, Result<EmployeeListsEntity>>
    {
        public async Task<Result<EmployeeListsEntity>> Handle(AddEmployeeListCommand request, CancellationToken cancellationToken)
        {
            var exists = await employeeRepository.EmployeeListExistsAsync(request.employeeList.Name, request.employeeList.AppUserId);

            if (exists)
            {
                return Result<EmployeeListsEntity>.Failure("A list with that name already exists.");
            }

            var savedEmployeeList = await employeeRepository.AddEmployeeListsAsync(request.employeeList, request.employeeIds);

            return Result<EmployeeListsEntity>.Success(savedEmployeeList);
        }
    }
}