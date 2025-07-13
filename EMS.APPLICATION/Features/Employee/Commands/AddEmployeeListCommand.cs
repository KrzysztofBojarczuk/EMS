using EMS.CORE.Common;
using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using MediatR;

namespace EMS.APPLICATION.Features.Employee.Commands
{
    public record AddEmployeeListCommand(EmployeeListsEntity employeeList, List<Guid> employeeIds) : IRequest<Result<EmployeeListsEntity>>;

    public class AddEmployeeListCommandHandler(IEmployeeRepository employeeRepository, IPublisher mediator)
        : IRequestHandler<AddEmployeeListCommand, Result<EmployeeListsEntity>>
    {
        public async Task<Result<EmployeeListsEntity>> Handle(AddEmployeeListCommand request, CancellationToken cancellationToken)
        {
            return await employeeRepository.AddEmployeeListsAsync(request.employeeList, request.employeeIds);
        }
    }
}