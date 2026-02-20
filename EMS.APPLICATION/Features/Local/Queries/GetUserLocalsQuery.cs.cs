using EMS.CORE.Entities;
using EMS.INFRASTRUCTURE.Extensions;
using EMS.INFRASTRUCTURE.Repositories;
using MediatR;

namespace EMS.APPLICATION.Features.Local.Queries
{
    public record GetUserLocalsQuery(string appUserId, int pageNumber, int pageSize, string searchTerm) : IRequest<PaginatedList<LocalEntity>>;

    public class GetUserLocalsQueryHandler(ILocalRepository localRepository) : IRequestHandler<GetUserLocalsQuery, PaginatedList<LocalEntity>>
    {
        public async Task<PaginatedList<LocalEntity>> Handle(GetUserLocalsQuery request, CancellationToken cancellationToken)
        {
            return await localRepository.GetUserLocalsAsync(request.appUserId, request.pageNumber, request.pageSize, request.searchTerm);
        }
    }
}