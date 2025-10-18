using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using EMS.INFRASTRUCTURE.Extensions;
using MediatR;

namespace EMS.APPLICATION.Features.Address.Queries
{
    public record GetUserAddressQuery(string appUserId, int pageNumber, int pageSize, string searchTerm) : IRequest<PaginatedList<AddressEntity>>;

    public class GetUserAddressQueryHandler(IAddressRepository addressRepository) : IRequestHandler<GetUserAddressQuery, PaginatedList<AddressEntity>>
    {
        public async Task<PaginatedList<AddressEntity>> Handle(GetUserAddressQuery request, CancellationToken cancellationToken)
        {
           return await addressRepository.GetUserAddressesAsync(request.appUserId, request.pageNumber, request.pageSize, request.searchTerm);
        }
    }
}