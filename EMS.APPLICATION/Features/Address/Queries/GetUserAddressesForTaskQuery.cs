using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using MediatR;

namespace EMS.APPLICATION.Features.Address.Queries
{
    public record GetUserAddressesForTaskQuery(string appUserId, string searchTerm) : IRequest<IEnumerable<AddressEntity>>;

    public class GetUserAddressesForTaskQueryHandler(IAddressRepository addressRepository) : IRequestHandler<GetUserAddressesForTaskQuery, IEnumerable<AddressEntity>>
    {
        public async Task<IEnumerable<AddressEntity>> Handle(GetUserAddressesForTaskQuery request, CancellationToken cancellationToken)
        {
            return await addressRepository.GetUserAddressesForTaskAsync(request.appUserId, request.searchTerm);
        }
    }
}