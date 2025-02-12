using EMS.CORE.Entities;
using EMS.INFRASTRUCTURE.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.CORE.Interfaces
{
    public interface IReservationRepository
    {
        Task<ReservationEntity> MakeReservationAsync(ReservationEntity reservation);
        Task<bool> DeleteReservationAsync(Guid reservationId);
        Task<ReservationEntity> GetReservationByIdAsync(Guid id);
        Task<PaginatedList<ReservationEntity>> GetUserReservationsAsync(string appUserId, int pageNumber, int pageSize, string searchTer);
    }
}
