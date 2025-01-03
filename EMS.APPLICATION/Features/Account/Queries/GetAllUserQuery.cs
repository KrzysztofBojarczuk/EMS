using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.APPLICATION.Features.Account.Queries
{
    public record GetAllUserQuery(string searchTerm) : IRequest<IEnumerable<AppUserEntity>>;

    public class GetAllUserQueryHandler(IUserRepository userRepository) : IRequestHandler<GetAllUserQuery, IEnumerable<AppUserEntity>>
    {
        public async Task<IEnumerable<AppUserEntity>> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
        {
            return await userRepository.GettAllUsersAsync(request.searchTerm);
        }
    }
}
