using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using EMS.INFRASTRUCTURE.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.INFRASTRUCTURE.Repositories
{
    public class UsersRepository(AppDbContext dbContext, UserManager<AppUserEntity> userManager) : IUserRepository
    {
        public async Task<bool> DeleteUserAsync(string appUserId)
        {
            var user = await userManager.FindByIdAsync(appUserId);

            if (user is not null)
            {
                await userManager.DeleteAsync(user);

                return true;
            }

            return false;
        }

        public async Task<int> GetNumberOfUsersAsync()
        {
            return await userManager.Users.CountAsync();
        }

        public async Task<ICollection<AppUserEntity>> GettAllUsersAsync(string searchTerm)
        {
            var query = userManager.Users.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(x => x.UserName.ToLower().Contains(searchTerm.ToLower()) || x.Email.ToLower().Contains(searchTerm.ToLower()));
            }

            return await query.ToListAsync();
        }
    }
}
