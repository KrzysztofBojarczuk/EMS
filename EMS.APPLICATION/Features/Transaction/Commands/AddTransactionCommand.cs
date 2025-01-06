using EMS.APPLICATION.Features.Employee.Commands;
using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.APPLICATION.Features.Transaction.Commands
{
   public record AddTransactionCommand(TransactionEntity transaction) : IRequest<TransactionEntity>;

    public class AddTransactioCommandHandler(ITransactionRepository transactionRepository, IPublisher mediator) 
        : IRequestHandler<AddTransactionCommand, TransactionEntity>
    {
        public async Task<TransactionEntity> Handle(AddTransactionCommand request, CancellationToken cancellationToken)
        {
            var transaction = await transactionRepository.AddTransactionAsync(request.transaction);
            return transaction;   
        }
    }
}
