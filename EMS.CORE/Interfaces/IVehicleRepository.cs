using EMS.CORE.Entities;
using EMS.CORE.Enums;
using EMS.INFRASTRUCTURE.Extensions;

namespace EMS.CORE.Interfaces
{
    public interface IVehicleRepository
    {
        Task<VehicleEntity> AddAVehicleAsync(VehicleEntity entity);
        Task<VehicleEntity> GetVehicleByIdAsync(Guid vehicleId);
        Task<PaginatedList<VehicleEntity>> GetUserVehiclesAsync(string appUserId, int pageNumber, int pageSize, string searchTerm, List<VehicleType> vehicleType, DateTime? dateFrom, DateTime? dateTo, string sortOrder);
        Task<IEnumerable<VehicleEntity>> GetUserVehiclesForTaskAsync(string appUserId, string searchTerm);
        Task<VehicleEntity> UpdateVehicleAsync(Guid vehicleId, string appUserId, VehicleEntity entity);
        Task<bool> DeleteVehicleAsync(Guid vehicleId, string appUserId);
    }
}