using EMS.APPLICATION.Dtos;
using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.APPLICATION.Features.Budget.Queries
{
    public record GetUserBudgetQuery(string appUserId) : IRequest<BudgetEntity>;

    public class GetUserBudgetQueryHandler(IBudgetRepository budgetRepository)
        : IRequestHandler<GetUserBudgetQuery, BudgetEntity>
    {
        public async Task<BudgetEntity> Handle(GetUserBudgetQuery request, CancellationToken cancellationToken)
        {
            return await budgetRepository.GetUserBudgetAsync(request.appUserId);
        }
    }
}
