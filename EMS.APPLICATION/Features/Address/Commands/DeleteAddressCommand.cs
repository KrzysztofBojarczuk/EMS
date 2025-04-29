using EMS.CORE.Interfaces;
using MediatR;

namespace EMS.APPLICATION.Features.Address.Commands
{
    public record DeleteAddressCommand(Guid addressId) : IRequest<bool>;

    public class DeleteAddressCommandHandler(IAddressRepository addressRepository) : IRequestHandler<DeleteAddressCommand, bool>
    {
        public async Task<bool> Handle(DeleteAddressCommand request, CancellationToken cancellationToken)
        {
            return await addressRepository.DeleteAddressAsync(request.addressId);
        }
    }
}
