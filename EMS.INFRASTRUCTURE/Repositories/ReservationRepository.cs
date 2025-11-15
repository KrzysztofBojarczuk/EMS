using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using EMS.INFRASTRUCTURE.Data;
using EMS.INFRASTRUCTURE.Extensions;
using Microsoft.EntityFrameworkCore;

namespace EMS.INFRASTRUCTURE.Repositories
{
    public class ReservationRepository(AppDbContext dbContext) : IReservationRepository
    {
        public async Task<ReservationEntity> AddReservationAsync(ReservationEntity reservation)
        {
            reservation.Id = Guid.NewGuid();
            dbContext.Reservations.Add(reservation);

            await dbContext.SaveChangesAsync();

            return reservation;
        }

        public async Task<bool> IsLocalBusyAsync(Guid localId, DateTime? checkIn, DateTime? checkOut)
        {
            return await dbContext.Reservations
                .Where(x => x.LocalId == localId)
                .AnyAsync(x =>
                    (checkIn >= x.CheckInDate && checkIn < x.CheckOutDate) || // zaczyna się w trakcie
                    (checkOut > x.CheckInDate && checkOut <= x.CheckOutDate) || // kończy się w trakcie
                    (checkIn <= x.CheckInDate && checkOut >= x.CheckOutDate)    // obejmuje całą istniejącą rezerwacj
                );
        }

        public async Task<bool> DeleteReservationAsync(Guid reservationId, string appUserId)
        {
            var reservation = await dbContext.Reservations.FirstOrDefaultAsync(x => x.Id == reservationId && x.AppUserId == appUserId);

            if (reservation is not null)
            {
                dbContext.Reservations.Remove(reservation);
                return await dbContext.SaveChangesAsync() > 0;
            }

            return false;
        }

        public async Task<ReservationEntity> GetReservationByIdAsync(Guid id)
        {
            return await dbContext.Reservations.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<PaginatedList<ReservationEntity>> GetUserReservationsAsync(string appUserId, int pageNumber, int pageSize, string searchTerm)
        {
            var query = dbContext.Reservations.Where(x => x.AppUserId == appUserId);

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(x => x.Id.ToString().ToLower().Contains(searchTerm.ToLower()));
            }

            return await PaginatedList<ReservationEntity>.CreateAsync(query, pageNumber, pageSize);
        }
    }
}