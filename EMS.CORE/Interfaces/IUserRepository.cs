using EMS.CORE.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.CORE.Interfaces
{
    public interface IUserRepository
    {
        Task<ICollection<AppUserEntity>> GettAllUsersAsync(string searchTerm);
        Task<bool> DeleteUserAsync(string appUserId);
    }
}
