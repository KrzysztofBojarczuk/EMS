using EMS.CORE.Common;
using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using EMS.INFRASTRUCTURE.Repositories;
using MediatR;

namespace EMS.APPLICATION.Features.Reservation.Commands
{
    public record AddReservationCommand(ReservationEntity Reservation) : IRequest<Result<ReservationEntity>>;

    public class AddReservationHandler(IReservationRepository reservationRepository, ILocalRepository localRepository)
       : IRequestHandler<AddReservationCommand, Result<ReservationEntity>>
    {
        public async Task<Result<ReservationEntity>> Handle(AddReservationCommand request, CancellationToken cancellationToken)
        {
            var reservation = request.Reservation;

            reservation.CheckInDate = reservation.CheckInDate?.ToUniversalTime();
            reservation.CheckOutDate = reservation.CheckOutDate?.ToUniversalTime();

            var local = await localRepository.GetLocalByIdAsync(reservation.LocalId);

            if (local == null)
            {
                return Result<ReservationEntity>.Failure("Local not found.");
            }

            if (local.NeedsRepair)
            {
                return Result<ReservationEntity>.Failure("Local is under repair.");
            }

            var isBusy = await reservationRepository.IsLocalBusyAsync(reservation.LocalId, reservation.CheckInDate, reservation.CheckOutDate);

            if (isBusy)
            {
                return Result<ReservationEntity>.Failure("Local is already reserved in the given time period.");
            }

            var savedReservation = await reservationRepository.AddReservationAsync(reservation);

            return Result<ReservationEntity>.Success(savedReservation);
        }
    }
}