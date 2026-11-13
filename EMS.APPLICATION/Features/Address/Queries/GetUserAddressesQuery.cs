using EMS.APPLICATION.Dtos;
using EMS.CORE.Common;
using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using EMS.INFRASTRUCTURE.Extensions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace EMS.APPLICATION.Features.Address.Queries
{
    public record GetUserAddressesQuery(string userName, int pageNumber, int pageSize, string searchTerm) : IRequest<Result<PaginatedList<AddressGetDto>>>;

    public class GetUserAddressesQueryHandler(IAddressRepository addressRepository, UserManager<AppUserEntity> userManager) : IRequestHandler<GetUserAddressesQuery, Result<PaginatedList<AddressGetDto>>>
    {
        public async Task<Result<PaginatedList<AddressGetDto>>> Handle(GetUserAddressesQuery request, CancellationToken cancellationToken)
        {
            var appUser = await userManager.FindByNameAsync(request.userName);

            if (appUser == null)
            {
                return Result<PaginatedList<AddressGetDto>>.Failure("User not found.");
            }

            var paginatedAddresses = await addressRepository.GetUserAddressesAsync(appUser.Id, request.pageNumber, request.pageSize, request.searchTerm);
            var addressDtos = paginatedAddresses.Items.Select(address => new AddressGetDto
            {
                Id = address.Id,
                City = address.City,
                Street = address.Street,
                Number = address.Number,
                ZipCode = address.ZipCode
            }).ToList();

            var result = new PaginatedList<AddressGetDto>(addressDtos, paginatedAddresses.TotalItems, paginatedAddresses.PageIndex, request.pageSize);

            return Result<PaginatedList<AddressGetDto>>.Success(result);
        }
    }
}