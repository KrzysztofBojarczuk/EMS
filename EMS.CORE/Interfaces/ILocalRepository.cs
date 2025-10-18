using EMS.CORE.Entities;
using EMS.INFRASTRUCTURE.Extensions;

namespace EMS.INFRASTRUCTURE.Repositories
{
    public interface ILocalRepository
    {
        Task<PaginatedList<LocalEntity>> GetUserLocalAsync(string appUserId, int pageNumber, int pageSize, string searchTerm);
        Task<LocalEntity> GetLocalByIdAsync(Guid id);
        Task<LocalEntity> AddLocalAsync(LocalEntity entity);
        Task<LocalEntity> UpdateLocalAsync(Guid localId, string appUserId, LocalEntity entity);
        Task<bool> DeleteLocalAsync(Guid localId, string appUserId);
    }
}