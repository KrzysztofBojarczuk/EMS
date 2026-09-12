using EMS.APPLICATION.Dtos;
using EMS.CORE.Common;
using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace EMS.APPLICATION.Features.Vehicle.Commands
{
    public record AddVehicleCommand(VehicleCreateDto vehicle, string username) : IRequest<Result<VehicleGetDto>>;

    public class AddVehicleCommandHandler(IVehicleRepository vehicleRepository, UserManager<AppUserEntity> userManager, ILogsRepository logsRepository) : IRequestHandler<AddVehicleCommand, Result<VehicleGetDto>>
    {
        public async Task<Result<VehicleGetDto>> Handle(AddVehicleCommand request, CancellationToken cancellationToken)
        {
            var appUser = await userManager.FindByNameAsync(request.username);

            if (appUser == null)
            {
                return Result<VehicleGetDto>.Failure("User not found.");
            }

            var vehicle = new VehicleEntity
            {
                Id = Guid.NewGuid(),
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
                IsAvailable = true,
                AppUserId = appUser.Id
            };

            var result = await vehicleRepository.AddVehicleAsync(vehicle);

            await logsRepository.AddAsync(new LogEntity { UserId = appUser.Id, Username = appUser.UserName, Action = $"Added vehicle {vehicle.RegistrationNumber}" });

            return Result<VehicleGetDto>.Success(new VehicleGetDto
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
            });
        }
    }
}