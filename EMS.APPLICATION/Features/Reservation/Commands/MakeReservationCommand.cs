using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.APPLICATION.Features.Reservation.Commands
{
    public record MakeReservationCommand(ReservationEntity Reservation) : IRequest<ReservationEntity>;
  
    public class MakeReservationHandler(IReservationRepository reservationRepository, IPublisher mediator) 
        : IRequestHandler<MakeReservationCommand, ReservationEntity>
    {
        public async Task<ReservationEntity> Handle(MakeReservationCommand request, CancellationToken cancellationToken)
        {
            var reservation = await reservationRepository.MakeReservationAsync(request.Reservation);
            return reservation;
        }
    }
}
