using EMS.CORE.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.APPLICATION.Features.Transaction.Commands
{
    public record DeleteTransactionCommand(Guid transactionId) : IRequest<bool>;

    public class DeleteTransactionCommandHandler(ITransactionRepository transactionRepository) : IRequestHandler<DeleteTransactionCommand, bool>
    {
        public async Task<bool> Handle(DeleteTransactionCommand request, CancellationToken cancellationToken)
        {
            return await transactionRepository.DeleteTransactionsAsync(request.transactionId);
        }
    }
}
