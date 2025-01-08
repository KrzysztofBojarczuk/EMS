using EMS.CORE.Entities;
using EMS.CORE.Enums;
using EMS.CORE.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.APPLICATION.Features.Transaction.Queries
{
    public record GetTransactionByBudgetIdQuery(Guid budgetId, List<CategoryType> category, string searchTerm) : IRequest<IEnumerable<TransactionEntity>>;

    public class GetTransactionByIdBudgetIdQueryHandler(ITransactionRepository transactionRepository)
        : IRequestHandler<GetTransactionByBudgetIdQuery, IEnumerable<TransactionEntity>>
    {
        public async Task<IEnumerable<TransactionEntity>> Handle(GetTransactionByBudgetIdQuery request, CancellationToken cancellation)
        {
            return await transactionRepository.GetTransactionsByBudgetIdAsync(request.budgetId, request.category, request.searchTerm);
        }
    }
}
