using EMS.APPLICATION.Features.Local.Queries;
using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using EMS.INFRASTRUCTURE.Extensions;
using EMS.INFRASTRUCTURE.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.APPLICATION.Features.Reservation.Queries
{
    public record GetUserReservationQuery(string appUserId, int pageNumber, int pageSize)
       : IRequest<PaginatedList<ReservationEntity>>;

    public class GetUserReservatioQueryHandler(IReservationRepository reservationRepository)
     : IRequestHandler<GetUserReservationQuery, PaginatedList<ReservationEntity>>
    {
        public async Task<PaginatedList<ReservationEntity>> Handle(GetUserReservationQuery request, CancellationToken cancellationToken)
        {
            return await reservationRepository.GetUserReservationsAsync(request.appUserId, request.pageNumber, request.pageSize);
        }
    }
}