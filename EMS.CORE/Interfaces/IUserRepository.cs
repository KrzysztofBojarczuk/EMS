using EMS.CORE.Entities;
using EMS.INFRASTRUCTURE.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.CORE.Interfaces
{
    public interface IUserRepository
    {
        Task<PaginatedList<AppUserEntity>> GettAllUsersAsync(int pageNumber, int pageSize, string searchTerm);
        Task<bool> DeleteUserAsync(string appUserId);
        Task<int> GetNumberOfUsersAsync();
    }
}
