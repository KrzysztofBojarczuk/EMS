using EMS.CORE.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.APPLICATION.Features.Budget.Commands
{
    public record DeleteBudgetCommand(Guid budgetId) : IRequest<bool>;

    public class DeleteBudgetCommandHandler(IBudgetRepository budgetRepository)
        : IRequestHandler<DeleteBudgetCommand, bool>
    {
        public async Task<bool> Handle(DeleteBudgetCommand request, CancellationToken cancellationToken)
        {
            return await budgetRepository.DeleteBudgetAsync(request.budgetId);
        }
    }
}
