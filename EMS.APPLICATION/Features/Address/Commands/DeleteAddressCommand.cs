using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace EMS.APPLICATION.Features.Address.Commands
{
    public record DeleteAddressCommand(Guid addressId, string userName) : IRequest<bool>;

    public class DeleteAddressCommandHandler(IAddressRepository addressRepository, UserManager<AppUserEntity> userManager, ILogsRepository logsRepository) : IRequestHandler<DeleteAddressCommand, bool>
    {
        public async Task<bool> Handle(DeleteAddressCommand request, CancellationToken cancellationToken)
        {
            var appUser = await userManager.FindByNameAsync(request.userName);

            var result = await addressRepository.DeleteAddressAsync(request.addressId, appUser.Id);

            await logsRepository.AddAsync(new LogEntity { UserId = appUser.Id, Username = appUser.UserName, Action = $"Deleted address {request.addressId}" });

            return result;
        }
    }
}