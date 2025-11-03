using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using MediatR;

namespace EMS.APPLICATION.Features.Vehicle.Commands
{
    public record UpdateVehicleCommand(Guid VehicleId, string appUserId, VehicleEntity Vehicle) : IRequest<VehicleEntity>;

    public class UpdateVehicleCommandHandler(IVehicleRepository vehicleRepository) : IRequestHandler<UpdateVehicleCommand, VehicleEntity>
    {
        public async Task<VehicleEntity> Handle(UpdateVehicleCommand request, CancellationToken cancellationToken)
        {
            return await vehicleRepository.UpdateVehicleAsync(request.VehicleId, request.appUserId, request.Vehicle);
        }
    }
}