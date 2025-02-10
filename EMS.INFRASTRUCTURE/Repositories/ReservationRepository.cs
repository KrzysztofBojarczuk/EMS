using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using EMS.INFRASTRUCTURE.Data;
using EMS.INFRASTRUCTURE.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.INFRASTRUCTURE.Repositories
{
    public class ReservationRepository(AppDbContext dbContext) : IReservationRepository
    {
        public async Task<ReservationEntity> MakeReservationAsync(ReservationEntity reservation)
        {
            reservation.Id = Guid.NewGuid();

            reservation.CheckInDate = reservation.CheckInDate?.ToLocalTime();
            reservation.CheckOutDate = reservation.CheckOutDate?.ToLocalTime();

            var local = await dbContext.Locals.FirstOrDefaultAsync(x => x.Id == reservation.LocalId);

            if (local is null || local.NeedsRepair)
            {
                return null; // Nie można zarezerwować niedostępnego lokalu
            }

            var isBusy = await dbContext.Reservations.AnyAsync(x => (reservation.CheckInDate >= x.CheckInDate && reservation.CheckInDate < x.CheckOutDate) // zaczyna się w trakcie
                                                                 || (reservation.CheckOutDate > x.CheckInDate && reservation.CheckOutDate <= x.CheckOutDate) // kończy się w trakcie
                                                                 || (reservation.CheckInDate <= x.CheckInDate && reservation.CheckOutDate >= x.CheckOutDate)); // obejmuje całą istniejącą rezerwacj
            if (isBusy)
            {
                return null; // Lokal jest zajęty
            }

            //dbContext.Locals.Update(local);
            dbContext.Reservations.Add(reservation);
            await dbContext.SaveChangesAsync();

            return reservation;
        }

        public async Task<bool> DeleteReservationAsync(Guid reservationId)
        {
            var reservation = await dbContext.Reservations.FirstOrDefaultAsync(x => x.Id == reservationId);

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

        public async Task<PaginatedList<ReservationEntity>> GetUserReservationsAsync(string appUserId, int pageNumber, int pageSize)
        {
            var query = dbContext.Reservations.Where(x => x.AppUserId == appUserId);
            return await PaginatedList<ReservationEntity>.CreateAsync(query, pageNumber, pageSize);
        }
    }
}
