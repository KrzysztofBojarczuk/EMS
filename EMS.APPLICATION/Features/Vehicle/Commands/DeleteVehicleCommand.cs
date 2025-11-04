using EMS.CORE.Interfaces;
using MediatR;

namespace EMS.APPLICATION.Features.Vehicle.Commands
{
    public record DeleteVehicleCommand(Guid vehicleId, string appUserId) : IRequest<bool>;

    public class DeleteVehicleCommandHandler(IVehicleRepository vehicleRepository) : IRequestHandler<DeleteVehicleCommand, bool>
    {
        public async Task<bool> Handle(DeleteVehicleCommand request, CancellationToken cancellationToken)
        {
            return await vehicleRepository.DeleteVehicleAsync(request.vehicleId, request.appUserId);
        }
    }
}