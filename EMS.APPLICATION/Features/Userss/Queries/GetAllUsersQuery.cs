using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using EMS.INFRASTRUCTURE.Extensions;
using MediatR;

namespace EMS.APPLICATION.Features.Userss.Queries
{
    public record GetAllUsersQuery(int pageNumber, int pageSize, string searchTerm, string sortOrder) : IRequest<PaginatedList<AppUserEntity>>;

    public class GetAllUsersQueryHandler(IUserRepository userRepository) : IRequestHandler<GetAllUsersQuery, PaginatedList<AppUserEntity>>
    {
        public async Task<PaginatedList<AppUserEntity>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            return await userRepository.GetAllUsersAsync(request.pageNumber, request.pageSize, request.searchTerm, request.sortOrder);
        }
    }
}