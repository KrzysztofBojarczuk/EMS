using EMS.APPLICATION.Dtos;
using EMS.CORE.Common;
using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using MediatR;

namespace EMS.APPLICATION.Features.Vehicle.Commands
{
    public record UpdateVehicleCommand(Guid vehicleId, string appUserId, VehicleCreateDto vehicle) : IRequest<Result<VehicleGetDto>>;

    public class UpdateVehicleCommandHandler(IVehicleRepository vehicleRepository) : IRequestHandler<UpdateVehicleCommand, Result<VehicleGetDto>>
    {
        public async Task<Result<VehicleGetDto>> Handle(UpdateVehicleCommand request, CancellationToken cancellationToken)
        {
            var vehicle = new VehicleEntity
            {
                Brand = request.vehicle.Brand,
                Model = request.vehicle.Model,
                Name = request.vehicle.Name,
                RegistrationNumber = request.vehicle.RegistrationNumber,
                Mileage = request.vehicle.Mileage,
                VehicleType = request.vehicle.VehicleType,
                DateOfProduction = request.vehicle.DateOfProduction.ToLocalTime(),
                InsuranceOcValidUntil = request.vehicle.InsuranceOcValidUntil.ToLocalTime(),
                InsuranceOcCost = request.vehicle.InsuranceOcCost,
                TechnicalInspectionValidUntil = request.vehicle.TechnicalInspectionValidUntil.ToLocalTime(),
                IsAvailable = request.vehicle.IsAvailable
            };

            var result = await vehicleRepository.UpdateVehicleAsync(request.vehicleId, request.appUserId, vehicle);

            if (result == null)
            {
                return Result<VehicleGetDto>.Failure("Vehicle not found.");
            }

            var vehicleDto = new VehicleGetDto
            {
                Id = result.Id,
                Brand = result.Brand,
                Model = result.Model,
                Name = result.Name,
                RegistrationNumber = result.RegistrationNumber,
                Mileage = result.Mileage,
                VehicleType = result.VehicleType,
                DateOfProduction = result.DateOfProduction,
                InsuranceOcValidUntil = result.InsuranceOcValidUntil,
                InsuranceOcCost = result.InsuranceOcCost,
                TechnicalInspectionValidUntil = result.TechnicalInspectionValidUntil,
                IsAvailable = result.IsAvailable
            };

            return Result<VehicleGetDto>.Success(vehicleDto);
        }
    }
}