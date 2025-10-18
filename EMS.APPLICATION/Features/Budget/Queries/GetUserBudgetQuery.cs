using EMS.APPLICATION.Dtos;
using EMS.CORE.Interfaces;
using MediatR;

namespace EMS.APPLICATION.Features.Budget.Queries
{
    public record GetUserBudgetQuery(string appUserId) : IRequest<BudgetEntity>;

    public class GetUserBudgetQueryHandler(IBudgetRepository budgetRepository) : IRequestHandler<GetUserBudgetQuery, BudgetEntity>
    {
        public async Task<BudgetEntity> Handle(GetUserBudgetQuery request, CancellationToken cancellationToken)
        {
            return await budgetRepository.GetUserBudgetAsync(request.appUserId);
        }
    }
}
