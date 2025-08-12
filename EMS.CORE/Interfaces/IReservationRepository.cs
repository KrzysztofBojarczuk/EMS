using EMS.CORE.Entities;
using EMS.INFRASTRUCTURE.Extensions;

namespace EMS.CORE.Interfaces
{
    public interface IReservationRepository
    {
        Task<ReservationEntity> MakeReservationAsync(ReservationEntity reservation);
        Task<bool> IsLocalBusyAsync(Guid localId, DateTime? checkIn, DateTime? checkOut);
        Task<bool> DeleteReservationAsync(Guid reservationId);
        Task<ReservationEntity> GetReservationByIdAsync(Guid id);
        Task<PaginatedList<ReservationEntity>> GetUserReservationsAsync(string appUserId, int pageNumber, int pageSize, string searchTer);
    }
}
