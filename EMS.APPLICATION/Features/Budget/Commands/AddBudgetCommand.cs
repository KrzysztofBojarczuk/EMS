using EMS.APPLICATION.Dtos;
using EMS.CORE.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.APPLICATION.Features.Budget.Commands
{
    public record AddBudgetCommand(BudgetEntity Budget): IRequest<BudgetEntity>;

    public class AddBudgetCommandHandler(IBudgetRepository budgetRepository, IPublisher publisher)
        : IRequestHandler<AddBudgetCommand, BudgetEntity>
    {
        public async Task<BudgetEntity> Handle(AddBudgetCommand request, CancellationToken cancellationToken)
        {
            var budget = await budgetRepository.AddBudgetAsync(request.Budget);
            return budget;
        }
    }

}
