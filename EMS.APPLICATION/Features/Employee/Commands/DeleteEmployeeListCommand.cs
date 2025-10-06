using EMS.CORE.Interfaces;
using MediatR;

namespace EMS.APPLICATION.Features.Employee.Commands
{
    public record  DeleteEmployeeListCommand(Guid employeeListId, string appUserId) : IRequest<bool>;

    public class DeleteEmployeeListCommandHandler(IEmployeeRepository employeeRepository) 
        : IRequestHandler<DeleteEmployeeListCommand, bool>
    {
        public async Task<bool> Handle(DeleteEmployeeListCommand request, CancellationToken cancellationToken)
        {
            return await employeeRepository.DeleteEmployeeListsAsync(request.employeeListId, request.appUserId);
        }
    }
}
