using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using EMS.INFRASTRUCTURE.Data;
using EMS.INFRASTRUCTURE.Extensions;
using Microsoft.EntityFrameworkCore;

namespace EMS.INFRASTRUCTURE.Repositories
{
    public class AddressRepository(AppDbContext dbContext) : IAddressRepository
    {
        public async Task<AddressEntity> AddAddressAsync(AddressEntity entity)
        {
            entity.Id = Guid.NewGuid(); //służy do przypisania nowego, unikalnego identyfikatora
            dbContext.Address.Add(entity);

            await dbContext.SaveChangesAsync();

            return entity;
        }

        public async Task<bool> DeleteAddressAsync(Guid addressId, string appUserId)
        {
            var address = await dbContext.Address.FirstOrDefaultAsync(x => x.Id == addressId && x.AppUserId == appUserId);

            if (address is not null)
            {
                var tasksWithAddress = await dbContext.Tasks.Where(x => x.AddressId == addressId).ToListAsync();

                foreach (var task in tasksWithAddress)
                {
                    task.AddressId = null;
                }

                dbContext.Address.Remove(address);

                return await dbContext.SaveChangesAsync() > 0;
            }

            return false;
        }

        public async Task<AddressEntity> GetAddressByIdAsync(Guid id)
        {
            return await dbContext.Address.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<PaginatedList<AddressEntity>> GetUserAddressesAsync(string appUserId, int pageNumber, int pageSize, string searchTerm)
        {
            var query = dbContext.Address.Where(x => x.AppUserId == appUserId);

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(x => x.Street.ToLower().Contains(searchTerm.ToLower())
                                      || x.City.ToLower().Contains(searchTerm.ToLower())
                                      || x.ZipCode.ToLower().Contains(searchTerm.ToLower())
                                      || x.Number.ToLower().Contains(searchTerm.ToLower()));
            }

            return await PaginatedList<AddressEntity>.CreateAsync(query, pageNumber, pageSize);
        }

        public async Task<IEnumerable<AddressEntity>> GetUserAddressesForTaskAsync(string appUserId, string searchTerm)
        {
            var query = dbContext.Address.Where(x => x.AppUserId == appUserId);

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(x => x.Street.ToLower().Contains(searchTerm.ToLower())
                                      || x.City.ToLower().Contains(searchTerm.ToLower())
                                      || x.ZipCode.ToLower().Contains(searchTerm.ToLower())
                                      || x.Number.ToLower().Contains(searchTerm.ToLower()));
            }

            return await query.ToListAsync();
        }

        public async Task<AddressEntity> UpdateAddressAsync(Guid addressId, string appUserId, AddressEntity entity)
        {
            var address = await dbContext.Address.FirstOrDefaultAsync(x => x.Id == addressId && x.AppUserId == appUserId);

            if (address is not null)
            {
                address.City = entity.City;
                address.Street = entity.Street;
                address.Number = entity.Number;
                address.ZipCode = entity.ZipCode;

                await dbContext.SaveChangesAsync();

                return address;
            }

            return entity;
        }
    }
}