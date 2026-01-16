using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using EMS.INFRASTRUCTURE.Data;
using EMS.INFRASTRUCTURE.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EMS.INFRASTRUCTURE.Repositories
{
    public class UsersRepository(AppDbContext dbContext, UserManager<AppUserEntity> userManager) : IUserRepository
    {
        public async Task<PaginatedList<AppUserEntity>> GetAllUsersAsync(int pageNumber, int pageSize, string searchTerm, string sortOrder)
        {
            var query = userManager.Users.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(x => x.UserName.ToLower().Contains(searchTerm.ToLower()) || x.Email.ToLower().Contains(searchTerm.ToLower()));
            }

            if (!string.IsNullOrEmpty(sortOrder))
            {
                switch (sortOrder.ToLower())
                {
                    case "createdate_asc":
                        query = query.OrderBy(x => x.CreatedAt);
                        break;
                    case "createdate_desc":
                        query = query.OrderByDescending(x => x.CreatedAt);
                        break;
                    default:
                        query = query.OrderByDescending(x => x.CreatedAt);
                        break;
                }
            }

            return await PaginatedList<AppUserEntity>.CreateAsync(query, pageNumber, pageSize);
        }

        public async Task<int> GetNumberOfUsersAsync()
        {
            return await userManager.Users.CountAsync();
        }

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
    }
}