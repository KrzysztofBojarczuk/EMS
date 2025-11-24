using EMS.APPLICATION.Dtos;
using EMS.CORE.Interfaces;
using MediatR;

namespace EMS.APPLICATION.Features.Budget.Commands
{
    public record AddBudgetCommand(BudgetEntity Budget) : IRequest<BudgetEntity>;

    public class AddBudgetCommandHandler(IBudgetRepository budgetRepository, IPublisher publisher) : IRequestHandler<AddBudgetCommand, BudgetEntity>
    {
        public async Task<BudgetEntity> Handle(AddBudgetCommand request, CancellationToken cancellationToken)
        {
            return await budgetRepository.AddBudgetAsync(request.Budget);
        }
    }
}