using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using MediatR;

namespace EMS.APPLICATION.Features.Vehicle.Commands
{
    public record AddVehicleCommand(VehicleEntity vehicle) : IRequest<VehicleEntity>;

    public class AddVehicleCommandHandler(IVehicleRepository vehicleRepository) : IRequestHandler<AddVehicleCommand, VehicleEntity>
    {
        public async Task<VehicleEntity> Handle(AddVehicleCommand request, CancellationToken cancellationToken)
        {
            return await vehicleRepository.AddAVehicleAsync(request.vehicle);
        }
    }
}