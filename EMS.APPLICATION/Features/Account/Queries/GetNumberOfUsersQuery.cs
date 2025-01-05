using EMS.CORE.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
