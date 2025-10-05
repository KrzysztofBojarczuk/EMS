using EMS.CORE.Entities;
using EMS.INFRASTRUCTURE.Data;
using EMS.INFRASTRUCTURE.Extensions;
using Microsoft.EntityFrameworkCore;

namespace EMS.INFRASTRUCTURE.Repositories
{
    public class LocalRepository(AppDbContext dbContext) : ILocalRepository
    {
        public async Task<LocalEntity> AddLocalAsync(LocalEntity entity)
        {
            entity.Id = Guid.NewGuid(); //służy do przypisania nowego, unikalnego identyfikatora
            dbContext.Locals.Add(entity);

            await dbContext.SaveChangesAsync();

            return entity;
        }

        public async Task<bool> DeleteLocalAsync(Guid localId)
        {
            var local = await dbContext.Locals.Include(x => x.ReservationsEntities).FirstOrDefaultAsync(x => x.Id == localId);

            if (local is not null)
            {
                dbContext.Reservations.RemoveRange(local.ReservationsEntities);
                dbContext.Locals.Remove(local);

                return await dbContext.SaveChangesAsync() > 0; //Jeśli usunięcie się powiodło: SaveChangesAsync() zwróci liczbę większą od 0, więc metoda zwróci true.
            }

            return false;
        }

        public async Task<LocalEntity> GetLocalByIdAsync(Guid id)
        {
            return await dbContext.Locals.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<PaginatedList<LocalEntity>> GetUserLocalAsync(string appUserId, int pageNumber, int pageSize, string searchTerm)
        {
            var query = dbContext.Locals.Include(x => x.ReservationsEntities).Where(x => x.AppUserId == appUserId);

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(x => x.LocalNumber.ToString().Contains(searchTerm) || x.Description.ToLower().Contains(searchTerm.ToLower()));
            }

            return await PaginatedList<LocalEntity>.CreateAsync(query, pageNumber, pageSize);
        }

        public async Task<LocalEntity> UpdateLocalAsync(Guid localId, LocalEntity entity)
        {
            var local = await dbContext.Locals.FirstOrDefaultAsync(x => x.Id == localId);

            if (local is not null)
            {
                local.LocalNumber = entity.LocalNumber;
                local.Surface = entity.Surface;
                local.NeedsRepair = entity.NeedsRepair;

                await dbContext.SaveChangesAsync();

                return local;
            }

            return entity;
        }
    }
}
