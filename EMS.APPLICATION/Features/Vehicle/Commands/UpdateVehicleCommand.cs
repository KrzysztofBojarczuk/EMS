using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using MediatR;

namespace EMS.APPLICATION.Features.Vehicle.Commands
{
    public record UpdateVehicleCommand(Guid vehicleId, string appUserId, VehicleEntity vehicle) : IRequest<VehicleEntity>;

    public class UpdateVehicleCommandHandler(IVehicleRepository vehicleRepository) : IRequestHandler<UpdateVehicleCommand, VehicleEntity>
    {
        public async Task<VehicleEntity> Handle(UpdateVehicleCommand request, CancellationToken cancellationToken)
        {
            return await vehicleRepository.UpdateVehicleAsync(request.vehicleId, request.appUserId, request.vehicle);
        }
    }
}