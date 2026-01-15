using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using EMS.INFRASTRUCTURE.Extensions;
using MediatR;

namespace EMS.APPLICATION.Features.Logs
{
    public record GetLogsQuery(int pageNumber, int pageSize, string searchTerm, DateTime? dateFrom, DateTime? dateTo, string sortOrder) : IRequest<PaginatedList<LogEntity>>;

    public class GetLogsQueryHandler(ILogsRepository logsRepository) : IRequestHandler<GetLogsQuery, PaginatedList<LogEntity>>
    {
        public async Task<PaginatedList<LogEntity>> Handle(GetLogsQuery request, CancellationToken cancellationToken)
        {
            return await logsRepository.GetLogsAsync(request.pageNumber, request.pageSize, request.searchTerm, request.dateFrom, request.dateTo, request.sortOrder);
        }
    }
}