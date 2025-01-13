using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.APPLICATION.Features.Address.Queries
{
    public record GetUserAddressQuery(string appUserId, string searchTerm) : IRequest<IEnumerable<AddressEntity>>;

    public class GetUserAddressQueryHandler(IAddressRepository addressRepository)
        : IRequestHandler<GetUserAddressQuery, IEnumerable<AddressEntity>>
    {
        public async Task<IEnumerable<AddressEntity>> Handle(GetUserAddressQuery request, CancellationToken cancellationToken)
        {
           return await addressRepository.GetUserAddressesAsync(request.appUserId, request.searchTerm);
        }
    }
}
