﻿using EMS.CORE.Common;
using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using EMS.INFRASTRUCTURE.Data;
using EMS.INFRASTRUCTURE.Extensions;
using Microsoft.EntityFrameworkCore;

namespace EMS.INFRASTRUCTURE.Repositories
{
    public class ReservationRepository(AppDbContext dbContext) : IReservationRepository
    {
        public async Task<Result<ReservationEntity>> MakeReservationAsync(ReservationEntity reservation)
        {
            reservation.Id = Guid.NewGuid();

            reservation.CheckInDate = reservation.CheckInDate?.ToLocalTime();
            reservation.CheckOutDate = reservation.CheckOutDate?.ToLocalTime();

            var local = await dbContext.Locals.FirstOrDefaultAsync(x => x.Id == reservation.LocalId);

            if (local is null)
            {
                return Result<ReservationEntity>.Failure("Local has not found"); // Nie można zarezerwować niedostępnego lokalu
            }

            if (local.NeedsRepair)
            {
                return Result<ReservationEntity>.Failure("Lokcal is in reapir"); // Nie można zarezerwować niedostępnego lokalu
            }

            var isBusy = await dbContext.Reservations.Where(x => x.LocalId == reservation.LocalId)
                                                     .AnyAsync(x => (reservation.CheckInDate >= x.CheckInDate && reservation.CheckInDate < x.CheckOutDate) // zaczyna się w trakcie
                                                                 || (reservation.CheckOutDate > x.CheckInDate && reservation.CheckOutDate <= x.CheckOutDate) // kończy się w trakcie
                                                                 || (reservation.CheckInDate <= x.CheckInDate && reservation.CheckOutDate >= x.CheckOutDate)); // obejmuje całą istniejącą rezerwacj
            if (isBusy)
            {
                return Result<ReservationEntity>.Failure("Local has reservation");
            }

            //dbContext.Locals.Update(local);
            dbContext.Reservations.Add(reservation);
            await dbContext.SaveChangesAsync();

            return Result<ReservationEntity>.Success(reservation);
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
