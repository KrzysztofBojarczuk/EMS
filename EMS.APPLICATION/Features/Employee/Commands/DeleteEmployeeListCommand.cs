using EMS.CORE.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.APPLICATION.Features.Employee.Commands
{
    public record  DeleteEmployeeListCommand(Guid employeeListId) : IRequest<bool>;

    public class DeleteEmployeeListCommandHandler(IEmployeeRepository employeeRepository) 
        : IRequestHandler<DeleteEmployeeListCommand, bool>
    {
        public async Task<bool> Handle(DeleteEmployeeListCommand request, CancellationToken cancellationToken)
        {
            return await employeeRepository.DeleteEmployeeListsAsync(request.employeeListId);
        }
    }
}
