using EMS.CORE.Entities;
using EMS.CORE.Enums;
using EMS.CORE.Interfaces;
using MediatR;

namespace EMS.APPLICATION.Features.Transaction.Queries
{
    public record GetTransactionsByBudgetIdQuery(Guid budgetId, string searchTerm, List<CategoryType> category, DateTime? dateFrom, DateTime? dateTo, decimal? amountFrom, decimal? amountTo, string sortOrder) : IRequest<IEnumerable<TransactionEntity>>;

    public class GetTransactionsByIdBudgetIdQueryHandler(ITransactionRepository transactionRepository) : IRequestHandler<GetTransactionsByBudgetIdQuery, IEnumerable<TransactionEntity>>
    {
        public async Task<IEnumerable<TransactionEntity>> Handle(GetTransactionsByBudgetIdQuery request, CancellationToken cancellation)
        {
            return await transactionRepository.GetTransactionsByBudgetIdAsync(request.budgetId, request.searchTerm, request.category, request.dateFrom, request.dateTo, request.amountFrom, request.amountTo, request.sortOrder);
        }
    }
}