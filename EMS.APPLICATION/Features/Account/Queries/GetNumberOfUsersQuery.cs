using EMS.CORE.Interfaces;
using MediatR;

namespace EMS.APPLICATION.Features.Account.Queries
{
    public record GetNumberOfUsersQuery() : IRequest<int>;

    public class GetNumberOfUsersQueryHandler(IUserRepository userRepository) : IRequestHandler<GetNumberOfUsersQuery, int>
    {
        public async Task<int> Handle(GetNumberOfUsersQuery request, CancellationToken cancellationToken)
        {
            return await userRepository.GetNumberOfUsersAsync();
        }
    }
}
