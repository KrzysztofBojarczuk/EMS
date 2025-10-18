using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using MediatR;

namespace EMS.APPLICATION.Features.Transaction.Commands
{
   public record AddTransactionCommand(TransactionEntity transaction) : IRequest<TransactionEntity>;

    public class AddTransactioCommandHandler(ITransactionRepository transactionRepository, IPublisher mediator) : IRequestHandler<AddTransactionCommand, TransactionEntity>
    {
        public async Task<TransactionEntity> Handle(AddTransactionCommand request, CancellationToken cancellationToken)
        {
            return await transactionRepository.AddTransactionAsync(request.transaction);
        }
    }
}
