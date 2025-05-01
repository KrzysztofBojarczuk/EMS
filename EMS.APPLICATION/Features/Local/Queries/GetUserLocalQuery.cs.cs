using EMS.CORE.Entities;
using EMS.INFRASTRUCTURE.Extensions;
using EMS.INFRASTRUCTURE.Repositories;
using MediatR;

namespace EMS.APPLICATION.Features.Local.Queries
{
    public record GetUserLocalQuery(string appUserId, int pageNumber, int pageSize, string searchTerm)
       : IRequest<PaginatedList<LocalEntity>>;

    public class GetUserLocalQueryHandler(ILocalRepository localRepository)
        : IRequestHandler<GetUserLocalQuery, PaginatedList<LocalEntity>>
    {
        public async Task<PaginatedList<LocalEntity>> Handle(GetUserLocalQuery request, CancellationToken cancellationToken)
        {
            return await localRepository.GetUserLocalAsync(request.appUserId, request.pageNumber, request.pageSize, request.searchTerm);
        }
    }
}
