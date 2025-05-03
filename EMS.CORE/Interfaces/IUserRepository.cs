using EMS.CORE.Entities;
using EMS.INFRASTRUCTURE.Extensions;

namespace EMS.CORE.Interfaces
{
    public interface IUserRepository
    {
        Task<PaginatedList<AppUserEntity>> GettAllUsersAsync(int pageNumber, int pageSize, string searchTerm);
        Task<bool> DeleteUserAsync(string appUserId);
        Task<int> GetNumberOfUsersAsync();
    }
}
