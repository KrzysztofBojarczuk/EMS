using EMS.CORE.Entities;
using EMS.INFRASTRUCTURE.Extensions;

namespace EMS.CORE.Interfaces
{
    public interface IUserRepository
    {
        Task<PaginatedList<AppUserEntity>> GetAllUsersAsync(int pageNumber, int pageSize, string searchTerm);
        Task<int> GetNumberOfUsersAsync();
        Task<bool> DeleteUserAsync(string appUserId);
    }
}