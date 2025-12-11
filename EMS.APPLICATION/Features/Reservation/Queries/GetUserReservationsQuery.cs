using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using EMS.INFRASTRUCTURE.Extensions;
using MediatR;

namespace EMS.APPLICATION.Features.Reservation.Queries
{
    public record GetUserReservationsQuery(string appUserId, int pageNumber, int pageSize, string searchTerm, string sortOrderDate) : IRequest<PaginatedList<ReservationEntity>>;

    public class GetUserReservationsQueryHandler(IReservationRepository reservationRepository) : IRequestHandler<GetUserReservationsQuery, PaginatedList<ReservationEntity>>
    {
        public async Task<PaginatedList<ReservationEntity>> Handle(GetUserReservationsQuery request, CancellationToken cancellationToken)
        {
            return await reservationRepository.GetUserReservationsAsync(request.appUserId, request.pageNumber, request.pageSize, request.searchTerm, request.sortOrderDate);
        }
    }
}