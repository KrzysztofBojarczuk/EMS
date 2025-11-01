using EMS.CORE.Entities;
using EMS.CORE.Enums;
using EMS.INFRASTRUCTURE.Extensions;

namespace EMS.CORE.Interfaces
{
    public interface IVehicleRepository
    {
        Task<VehicleEntity> AddAVehicleAsync(VehicleEntity entity);
        Task<VehicleEntity> GetVehicleByIdAsync(Guid vehicleId);
        Task<VehicleEntity> UpdateVehicleAsync(Guid vehicleId, string appUserId, VehicleEntity entity);
        Task<PaginatedList<VehicleEntity>> GetUserVehiclesAsync(string appUserId, int pageNumber, int pageSize, List<VehicleType> vehicleType, string searchTerm);
        Task<IEnumerable<VehicleEntity>> GetUserVehiclesForTaskAsync(string appUserId, string searchTerm);
        Task<bool> DeleteVehicleAsync(Guid vehicleId, string appUserId);
    }
}