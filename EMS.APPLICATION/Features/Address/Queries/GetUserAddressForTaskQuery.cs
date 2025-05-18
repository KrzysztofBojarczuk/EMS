using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using MediatR;

namespace EMS.APPLICATION.Features.Address.Queries
{
    public record GetUserAddressForTaskQuery(string appUserId, string searchTerm) : IRequest<IEnumerable<AddressEntity>>;

    public class GetUserAddressForTaskQueryHandler(IAddressRepository addressRepository)
        : IRequestHandler<GetUserAddressForTaskQuery, IEnumerable<AddressEntity>>
    {
        public async Task<IEnumerable<AddressEntity>> Handle(GetUserAddressForTaskQuery request, CancellationToken cancellationToken)
        {
            return await addressRepository.GetUserAddressesForTaskAsync(request.appUserId, request.searchTerm);
        }
    }
}