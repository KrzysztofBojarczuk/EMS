using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using EMS.INFRASTRUCTURE.Data;
using EMS.INFRASTRUCTURE.Extensions;

namespace EMS.INFRASTRUCTURE.Repositories
{
    public class LogRepository(AppDbContext dbContext) : ILogsRepository
    {
        public async Task AddAsync(LogEntity entity)
        {
            entity.Id = Guid.NewGuid(); //s³u¿y do przypisania nowego, unikalnego identyfikatora
            entity.CreatedAt = entity.CreatedAt.ToLocalTime();
            dbContext.Logs.Add(entity);

            await dbContext.SaveChangesAsync();
        }

        public Task<PaginatedList<LogEntity>> GetLogsAsync(int pageNumber, int pageSize, string searchTerm, DateTime? dateFrom, DateTime? dateTo, string sortOrder)
        {
            var query = dbContext.Logs.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = dbContext.Logs.Where(x => x.UserId.ToLower().Contains(searchTerm.ToLower())
                                                  || x.Username.ToLower().Contains(searchTerm.ToLower())
                                                  || x.Action.ToLower().Contains(searchTerm.ToLower())
                                                  || x.RequestData.ToLower().Contains(searchTerm.ToLower())
                                                  || x.Status.ToLower().Contains(searchTerm.ToLower()));
            }

            if (dateFrom.HasValue && dateTo.HasValue)
            {
                query = query.Where(x => x.CreatedAt >= dateFrom.Value && x.CreatedAt <= dateTo.Value);
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

            return PaginatedList<LogEntity>.CreateAsync(query, pageNumber, pageSize);
        }
    }
}