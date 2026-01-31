using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using MediatR;

namespace EMS.APPLICATION.Features.Address.Commands
{
    public record UpdateAddressCommand(Guid addressId, string appUserId, AddressEntity address) : IRequest<AddressEntity>;

    public class UpdateAddressCommandHandler(IAddressRepository addressRepository) : IRequestHandler<UpdateAddressCommand, AddressEntity>
    {
        public async Task<AddressEntity> Handle(UpdateAddressCommand request, CancellationToken cancellationToken)
        {
            return await addressRepository.UpdateAddressAsync(request.addressId, request.appUserId, request.address);
        }
    }
}