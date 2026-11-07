using EMS.APPLICATION.Dtos;
using EMS.CORE.Common;
using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace EMS.APPLICATION.Features.Transaction.Commands
{
    public record AddTransactionCommand(TransactionCreateDto transaction, Guid budgetId, string username) : IRequest<Result<TransactionGetDto>>;

    public class AddTransactionCommandHandler(ITransactionRepository transactionRepository, UserManager<AppUserEntity> userManager, ILogsRepository logsRepository) : IRequestHandler<AddTransactionCommand, Result<TransactionGetDto>>
    {
        public async Task<Result<TransactionGetDto>> Handle(AddTransactionCommand request, CancellationToken cancellationToken)
        {
            var appUser = await userManager.FindByNameAsync(request.username);

            if (appUser == null)
            {
                return Result<TransactionGetDto>.Failure("User not found.");
            }

            var transaction = new TransactionEntity
            {
                Id = Guid.NewGuid(),
                Name = request.transaction.Name,
                CreatedAt = DateTime.UtcNow,
                Category = request.transaction.Category,
                Amount = request.transaction.Amount,
                BudgetId = request.budgetId
            };

            var result = await transactionRepository.AddTransactionAsync(transaction);

            await logsRepository.AddAsync(new LogEntity { UserId = appUser.Id, Username = appUser.UserName, Action = $"Added transaction {transaction.Name}" });

            return Result<TransactionGetDto>.Success(new TransactionGetDto
            {
                Id = result.Id,
                Name = result.Name,
                CreatedAt = result.CreatedAt,
                Category = result.Category,
                Amount = result.Amount
            });
        }
    }
}