using EMS.CORE.Entities;
using EMS.CORE.Enums;
using EMS.CORE.Interfaces;
using EMS.INFRASTRUCTURE.Data;
using EMS.INFRASTRUCTURE.Extensions;
using Microsoft.EntityFrameworkCore;

namespace EMS.INFRASTRUCTURE.Repositories
{
    public class VehicleRepository(AppDbContext dbContext) : IVehicleRepository
    {
        public async Task<VehicleEntity> AddAVehicleAsync(VehicleEntity entity)
        {
            entity.Id = Guid.NewGuid(); //s³u¿y do przypisania nowego, unikalnego identyfikatora
            entity.DateOfProduction = entity.DateOfProduction.ToLocalTime();
            dbContext.Vehicles.Add(entity);

            await dbContext.SaveChangesAsync();

            return entity;
        }

        public async Task<VehicleEntity> GetVehicleByIdAsync(Guid Id)
        {
            return await dbContext.Vehicles.FirstOrDefaultAsync(x => x.Id == Id);
        }

        public async Task<PaginatedList<VehicleEntity>> GetUserVehiclesAsync(string appUserId, int pageNumber, int pageSize, string searchTerm, List<VehicleType> vehicleType, DateTime? dateFrom, DateTime? dateTo, string sortOrderDate, string sortOrderMileage)
        {
            var query = dbContext.Vehicles.Where(x => x.AppUserId == appUserId);

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(x => x.Brand.ToLower().Contains(searchTerm.ToLower())
                                      || x.Model.ToLower().Contains(searchTerm.ToLower())
                                      || x.Name.ToLower().Contains(searchTerm.ToLower())
                                      || x.RegistrationNumber.ToLower().Contains(searchTerm.ToLower()));
            }

            if (vehicleType != null && vehicleType.Any())
            {
                query = query.Where(x => vehicleType.Contains(x.VehicleType));
            }

            if (dateFrom.HasValue && dateTo.HasValue)
            {
                query = query.Where(x => x.DateOfProduction >= dateFrom.Value && x.DateOfProduction <= dateTo.Value);
            }

            if (!string.IsNullOrEmpty(sortOrderMileage))
            {
                switch (sortOrderMileage)
                {
                    case "mileage_asc":
                        query = query.OrderBy(x => x.Mileage);
                        break;
                    case "mileage_desc":
                        query = query.OrderByDescending(x => x.Mileage);
                        break;
                    default:
                        query = query.OrderByDescending(x => x.Mileage);
                        break;
                }
            }

            if (!string.IsNullOrEmpty(sortOrderDate))
            {
                switch (sortOrderDate)
                {
                    case "date_asc":
                        query = query.Expression.Type == typeof(IOrderedQueryable<VehicleEntity>)
                            ? ((IOrderedQueryable<VehicleEntity>)query).ThenBy(x => x.DateOfProduction)
                            : query.OrderBy(x => x.DateOfProduction);
                        break;

                    case "date_desc":
                        query = query.Expression.Type == typeof(IOrderedQueryable<VehicleEntity>)
                            ? ((IOrderedQueryable<VehicleEntity>)query).ThenByDescending(x => x.DateOfProduction)
                            : query.OrderByDescending(x => x.DateOfProduction);
                        break;
                    default:
                        query = query.Expression.Type == typeof(IOrderedQueryable<VehicleEntity>)
                             ? ((IOrderedQueryable<VehicleEntity>)query).ThenByDescending(x => x.DateOfProduction)
                             : query.OrderByDescending(x => x.DateOfProduction);
                        break;
                }
            }

            return await PaginatedList<VehicleEntity>.CreateAsync(query, pageNumber, pageSize);
        }

        public async Task<IEnumerable<VehicleEntity>> GetUserVehiclesForTaskAsync(string appUserId, string searchTerm)
        {
            var query = dbContext.Vehicles.Where(x => x.AppUserId == appUserId && x.TaskId == null && x.IsAvailable == true);

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(x => x.Name.ToLower().Contains(searchTerm.ToLower())
                                      || x.RegistrationNumber.ToLower().Contains(searchTerm.ToLower()));
            }

            return await query.ToListAsync();
        }

        public async Task<VehicleEntity> UpdateVehicleAsync(Guid vehicleId, string appUserId, VehicleEntity entity)
        {
            var vehicle = await dbContext.Vehicles.FirstOrDefaultAsync(x => x.Id == vehicleId && x.AppUserId == appUserId);

            if (vehicle is not null)
            {
                vehicle.Brand = entity.Brand;
                vehicle.Model = entity.Model;
                vehicle.Name = entity.Name;
                vehicle.RegistrationNumber = entity.RegistrationNumber;
                vehicle.Mileage = entity.Mileage;
                vehicle.VehicleType = entity.VehicleType;
                vehicle.DateOfProduction = entity.DateOfProduction.ToLocalTime();
                vehicle.IsAvailable = entity.IsAvailable;

                await dbContext.SaveChangesAsync();

                return vehicle;
            }

            return entity;
        }

        public async Task<bool> DeleteVehicleAsync(Guid vehicleId, string appUserId)
        {
            var vehicle = await dbContext.Vehicles.FirstOrDefaultAsync(x => x.Id == vehicleId && x.AppUserId == appUserId);

            if (vehicle is not null)
            {
                dbContext.Vehicles.Remove(vehicle);

                return await dbContext.SaveChangesAsync() > 0;  //Jeœli usuniêcie siê powiod³o: SaveChangesAsync() zwróci liczbê wiêksz¹ od 0, wiêc metoda zwróci true.
            }

            return true;
        }
    }
}