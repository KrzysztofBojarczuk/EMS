using EMS.CORE.Entities;
using EMS.INFRASTRUCTURE.Extensions;

namespace EMS.CORE.Interfaces
{
    public interface ILogsRepository
    {
        Task AddAsync(LogEntity auditLog);
        Task<PaginatedList<LogEntity>> GetLogsAsync(int pageNumber, int pageSize, string searchTerm, DateTime? dateFrom, DateTime? dateTo, string sortOrder);
    }
}