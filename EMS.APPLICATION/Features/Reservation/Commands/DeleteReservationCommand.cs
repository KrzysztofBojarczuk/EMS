using EMS.CORE.Interfaces;
using MediatR;

namespace EMS.APPLICATION.Features.Reservation.Commands
{
    public record DeleteReservationCommand(Guid reservationId, string appUserId) : IRequest<bool>;

    public class DeleteReservationCommandHandler(IReservationRepository reservationRepository) : IRequestHandler<DeleteReservationCommand, bool>
    {
        public async Task<bool> Handle(DeleteReservationCommand request, CancellationToken cancellationToken)
        {
            return await reservationRepository.DeleteReservationAsync(request.reservationId, request.appUserId);
        }
    }
}