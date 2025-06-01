using EMS.CORE.Common;
using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using MediatR;

namespace EMS.APPLICATION.Features.Reservation.Commands
{
    public record MakeReservationCommand(ReservationEntity Reservation) : IRequest<Result<ReservationEntity>>;

    public class MakeReservationHandler(IReservationRepository reservationRepository)
       : IRequestHandler<MakeReservationCommand, Result<ReservationEntity>>
    {
        public async Task<Result<ReservationEntity>> Handle(MakeReservationCommand request, CancellationToken cancellationToken)
        {
            var reservation = await reservationRepository.MakeReservationAsync(request.Reservation);
            return reservation;
        }
    }
}
