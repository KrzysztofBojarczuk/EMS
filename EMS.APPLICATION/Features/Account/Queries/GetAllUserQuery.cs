using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using EMS.INFRASTRUCTURE.Extensions;
using MediatR;

namespace EMS.APPLICATION.Features.Account.Queries
{
    public record GetAllUserQuery(int pageNumber, int pageSize, string searchTerm) : IRequest<PaginatedList<AppUserEntity>>;

    public class GetAllUserQueryHandler(IUserRepository userRepository) : IRequestHandler<GetAllUserQuery, PaginatedList<AppUserEntity>>
    {
        public async Task<PaginatedList<AppUserEntity>> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
        {
            return await userRepository.GettAllUsersAsync(request.pageNumber, request.pageSize, request.searchTerm);
        }
    }
}
