using EMS.CORE.Common;
using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using EMS.INFRASTRUCTURE.Repositories;
using MediatR;

namespace EMS.APPLICATION.Features.Reservation.Commands
{
    public record AddReservationCommand(ReservationEntity reservation) : IRequest<Result<ReservationEntity>>;

    public class AddReservationCommandHandler(IReservationRepository reservationRepository, ILocalRepository localRepository) : IRequestHandler<AddReservationCommand, Result<ReservationEntity>>
    {
        public async Task<Result<ReservationEntity>> Handle(AddReservationCommand request, CancellationToken cancellationToken)
        {
            var local = await localRepository.GetLocalByIdAsync(request.reservation.LocalId);

            if (local == null)
            {
                return Result<ReservationEntity>.Failure("Local not found.");
            }

            if (local.NeedsRepair)
            {
                return Result<ReservationEntity>.Failure("Local is under repair.");
            }

            var isBusy = await reservationRepository.IsLocalBusyAsync(request.reservation.LocalId, request.reservation.CheckInDate.ToLocalTime(), request.reservation.CheckOutDate.ToLocalTime());

            if (isBusy)
            {
                return Result<ReservationEntity>.Failure("Local is already reserved in the given time period.");
            }

            var savedReservation = await reservationRepository.AddReservationAsync(request.reservation);

            return Result<ReservationEntity>.Success(savedReservation);
        }
    }
}