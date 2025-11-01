using EMS.CORE.Entities;
using EMS.CORE.Enums;
using EMS.CORE.Interfaces;
using EMS.INFRASTRUCTURE.Data;
using EMS.INFRASTRUCTURE.Extensions;

namespace EMS.INFRASTRUCTURE.Repositories
{
    public class VehicleRepository(AppDbContext dbContext) : IVehicleRepository
    {
        public async Task<VehicleEntity> AddAVehicleAsync(VehicleEntity entity)
        {
            entity.Id = Guid.NewGuid(); //służy do przypisania nowego, unikalnego identyfikatora
            dbContext.Vehicles.Add(entity);

            await dbContext.SaveChangesAsync();

            return entity;
        }
        public Task<bool> DeleteVehicleAsync(Guid vehicleId, string appUserId)
        {
            throw new NotImplementedException();
        }

        public Task<PaginatedList<VehicleEntity>> GetUserVehiclesAsync(string appUserId, int pageNumber, int pageSize, List<VehicleType> vehicleType, string searchTerm)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<VehicleEntity>> GetUserVehiclesForTaskAsync(string appUserId, string searchTerm)
        {
            throw new NotImplementedException();
        }

        public Task<VehicleEntity> GetVehicleByIdAsync(Guid vehicleId)
        {
            throw new NotImplementedException();
        }

        public Task<VehicleEntity> UpdateVehicleAsync(Guid vehicleId, string appUserId, VehicleEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
