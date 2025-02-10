using EMS.CORE.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.APPLICATION.Features.Reservation.Commands
{
    public record DeleteReservationCommand(Guid reservationId) : IRequest<bool>;

    public class DeleReservationCommandHandler(IReservationRepository reservationRepository) : IRequestHandler<DeleteReservationCommand, bool>
    {
        public async Task<bool> Handle(DeleteReservationCommand request, CancellationToken cancellationToken)
        {
            return await reservationRepository.DeleteReservationAsync(request.reservationId);
        }
    }
}
