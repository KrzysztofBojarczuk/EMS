using EMS.CORE.Interfaces;
using MediatR;

namespace EMS.APPLICATION.Features.Budget.Commands
{
    public record DeleteBudgetCommand(Guid budgetId, string appUserId) : IRequest<bool>;

    public class DeleteBudgetCommandHandler(IBudgetRepository budgetRepository) : IRequestHandler<DeleteBudgetCommand, bool>
    {
        public async Task<bool> Handle(DeleteBudgetCommand request, CancellationToken cancellationToken)
        {
            return await budgetRepository.DeleteBudgetAsync(request.budgetId, request.appUserId);
        }
    }
}
