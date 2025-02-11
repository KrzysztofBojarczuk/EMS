using EMS.APPLICATION.Features.Reservation.Commands;
using EMS.INFRASTRUCTURE.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.APPLICATION.Features.Local.Commands
{
    public record DeleteLocalCommand(Guid localId) : IRequest<bool>;

    public class DeleteLocalCommandHandler(ILocalRepository localRepository)
        : IRequestHandler<DeleteLocalCommand, bool>
    {
        public async Task<bool> Handle(DeleteLocalCommand request, CancellationToken cancellationToken)
        {
            return await localRepository.DeleteLocalAsync(request.localId);
        }
    }
}
