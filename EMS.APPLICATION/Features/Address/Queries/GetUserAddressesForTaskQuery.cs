using EMS.APPLICATION.Dtos;
using EMS.CORE.Common;
using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace EMS.APPLICATION.Features.Address.Queries
{
    public record GetUserAddressesForTaskQuery(string userName, string searchTerm) : IRequest<Result<IEnumerable<AddressGetDto>>>;

    public class GetUserAddressesForTaskQueryHandler(IAddressRepository addressRepository, UserManager<AppUserEntity> userManager) : IRequestHandler<GetUserAddressesForTaskQuery, Result<IEnumerable<AddressGetDto>>>
    {
        public async Task<Result<IEnumerable<AddressGetDto>>> Handle(GetUserAddressesForTaskQuery request, CancellationToken cancellationToken)
        {
            var appUser = await userManager.FindByNameAsync(request.userName);

            if (appUser == null)
            {
                return Result<IEnumerable<AddressGetDto>>.Failure("User not found.");
            }

            var addresses = await addressRepository.GetUserAddressesForTaskAsync(appUser.Id, request.searchTerm);

            var addressDtos = addresses.Select(address => new AddressGetDto
            {
                Id = address.Id,
                City = address.City,
                Street = address.Street,
                Number = address.Number,
                ZipCode = address.ZipCode
            }).ToList();

            return Result<IEnumerable<AddressGetDto>>.Success(addressDtos);
        }
    }
}