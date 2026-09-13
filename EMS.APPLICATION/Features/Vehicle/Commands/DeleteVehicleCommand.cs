using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace EMS.APPLICATION.Features.Vehicle.Commands
{
    public record DeleteVehicleCommand(Guid vehicleId, string username) : IRequest<bool>;

    public class DeleteVehicleCommandHandler(IVehicleRepository vehicleRepository, UserManager<AppUserEntity> userManager, ILogsRepository logsRepository) : IRequestHandler<DeleteVehicleCommand, bool>
    {
        public async Task<bool> Handle(DeleteVehicleCommand request, CancellationToken cancellationToken)
        {
            var appUser = await userManager.FindByNameAsync(request.username);

            var result = await vehicleRepository.DeleteVehicleAsync(request.vehicleId, appUser.Id);

            await logsRepository.AddAsync(new LogEntity { UserId = appUser.Id, Username = appUser.UserName, Action = $"Deleted vehicle {request.vehicleId}" });

            return result;
        }
    }
}