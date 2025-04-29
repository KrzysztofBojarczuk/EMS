using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using MediatR;

namespace EMS.APPLICATION.Features.Employee.Commands
{
    public record AddEmployeeListCommand(EmployeeListsEntity employeeList, List<Guid> employeeIds) : IRequest<EmployeeListsEntity>;

    public class AddEmployeeListCommandHandler(IEmployeeRepository employeeRepository, IPublisher mediator)
        : IRequestHandler<AddEmployeeListCommand, EmployeeListsEntity>
    {
        public async Task<EmployeeListsEntity> Handle(AddEmployeeListCommand request, CancellationToken cancellationToken)
        {
            var employeeList = await employeeRepository.AddEmployeeListsAsync(request.employeeList, request.employeeIds);
            return employeeList;
        }
    }
}
