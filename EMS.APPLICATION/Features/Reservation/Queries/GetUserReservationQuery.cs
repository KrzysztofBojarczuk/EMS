using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using EMS.INFRASTRUCTURE.Extensions;
using MediatR;

namespace EMS.APPLICATION.Features.Reservation.Queries
{
    public record GetUserReservationQuery(string appUserId, int pageNumber, int pageSize, string searchTerm) : IRequest<PaginatedList<ReservationEntity>>;

    public class GetUserReservatioQueryHandler(IReservationRepository reservationRepository) : IRequestHandler<GetUserReservationQuery, PaginatedList<ReservationEntity>>
    {
        public async Task<PaginatedList<ReservationEntity>> Handle(GetUserReservationQuery request, CancellationToken cancellationToken)
        {
            return await reservationRepository.GetUserReservationsAsync(request.appUserId, request.pageNumber, request.pageSize, request.searchTerm);
        }
    }
}