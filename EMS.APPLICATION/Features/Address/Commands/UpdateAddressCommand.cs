using EMS.APPLICATION.Dtos;
using EMS.CORE.Common;
using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace EMS.APPLICATION.Features.Address.Commands
{
    public record UpdateAddressCommand(Guid addressId, string userName, AddressCreateDto address) : IRequest<Result<AddressGetDto>>;

    public class UpdateAddressCommandHandler(IAddressRepository addressRepository, UserManager<AppUserEntity> userManager, ILogsRepository logsRepository) : IRequestHandler<UpdateAddressCommand, Result<AddressGetDto>>
    {
        public async Task<Result<AddressGetDto>> Handle(UpdateAddressCommand request, CancellationToken cancellationToken)
        {
            var appUser = await userManager.FindByNameAsync(request.userName);

            if (appUser == null)
            {
                return Result<AddressGetDto>.Failure("User not found.");
            }

            var address = new AddressEntity
            {
                City = request.address.City,
                Street = request.address.Street,
                Number = request.address.Number,
                ZipCode = request.address.ZipCode
            };

            var result = await addressRepository.UpdateAddressAsync(request.addressId, appUser.Id, address);

            await logsRepository.AddAsync(new LogEntity { UserId = appUser.Id, Username = appUser.UserName, Action = $"Updated address {result.Id}" });

            return Result<AddressGetDto>.Success(new AddressGetDto
            {
                Id = result.Id,
                City = result.City,
                Street = result.Street,
                Number = result.Number,
                ZipCode = result.ZipCode
            });
        }
    }
}