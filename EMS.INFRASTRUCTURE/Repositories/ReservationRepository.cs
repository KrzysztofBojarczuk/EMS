using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using EMS.INFRASTRUCTURE.Data;
using EMS.INFRASTRUCTURE.Extensions;
using Microsoft.EntityFrameworkCore;

namespace EMS.INFRASTRUCTURE.Repositories
{
    public class ReservationRepository(AppDbContext dbContext) : IReservationRepository
    {
        public async Task<ReservationEntity> AddReservationAsync(ReservationEntity entity)
        {
            entity.Id = Guid.NewGuid();
            entity.CheckInDate = entity.CheckInDate.ToLocalTime();
            entity.CheckOutDate = entity.CheckOutDate.ToLocalTime();
            dbContext.Reservations.Add(entity);

            await dbContext.SaveChangesAsync();

            return entity;
        }

        public async Task<ReservationEntity> GetReservationByIdAsync(Guid id)
        {
            return await dbContext.Reservations.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<PaginatedList<ReservationEntity>> GetUserReservationsAsync(string appUserId, int pageNumber, int pageSize, string searchTerm, string sortOrderDate)
        {
            var query = dbContext.Reservations.Where(x => x.AppUserId == appUserId);

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(x => x.Id.ToString().ToLower().Contains(searchTerm.ToLower())
                                      || x.Description.ToLower().Contains(searchTerm.ToLower()));
            }

            if (!string.IsNullOrEmpty(sortOrderDate))
            {
                switch (sortOrderDate)
                {
                    case "start_asc":
                        query = query.OrderBy(x => x.CheckInDate);
                        break;
                    case "start_desc":
                        query = query.OrderByDescending(x => x.CheckInDate);
                        break;
                    case "end_asc":
                        query = query.OrderBy(x => x.CheckOutDate);
                        break;
                    case "end_desc":
                        query = query.OrderByDescending(x => x.CheckOutDate);
                        break;
                    default:
                        query = query.OrderByDescending(x => x.CheckOutDate);
                        break;
                }
            }
            else
            {
                query = query.OrderByDescending(x => x.CheckOutDate);
            }

            return await PaginatedList<ReservationEntity>.CreateAsync(query, pageNumber, pageSize);
        }

        public async Task<bool> IsLocalBusyAsync(Guid localId, DateTime? checkIn, DateTime? checkOut)
        {
            return await dbContext.Reservations
                .Where(x => x.LocalId == localId)
                .AnyAsync(x =>
                    (checkIn >= x.CheckInDate && checkIn < x.CheckOutDate) || // zaczyna siê w trakcie
                    (checkOut > x.CheckInDate && checkOut <= x.CheckOutDate) || // koñczy siê w trakcie
                    (checkIn <= x.CheckInDate && checkOut >= x.CheckOutDate)    // obejmuje ca³¹ istniej¹c¹ rezerwacj
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
    }
}