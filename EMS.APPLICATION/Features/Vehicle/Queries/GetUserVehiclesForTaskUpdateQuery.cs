using EMS.APPLICATION.Dtos;
using EMS.CORE.Common;
using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace EMS.APPLICATION.Features.Vehicle.Queries
{
    public record GetUserVehiclesForTaskUpdateQuery(string username, Guid taskId, string searchTerm) : IRequest<Result<IEnumerable<VehicleGetDto>>>;

    public class GetUserVehiclesForTaskUpdateQueryHandler(IVehicleRepository vehicleRepository, UserManager<AppUserEntity> userManager) : IRequestHandler<GetUserVehiclesForTaskUpdateQuery, Result<IEnumerable<VehicleGetDto>>>
    {
        public async Task<Result<IEnumerable<VehicleGetDto>>> Handle(GetUserVehiclesForTaskUpdateQuery request, CancellationToken cancellationToken)
        {
            var appUser = await userManager.FindByNameAsync(request.username);

            if (appUser == null)
            {
                return Result<IEnumerable<VehicleGetDto>>
                    .Failure("User not found.");
            }

            var vehicles = await vehicleRepository.GetUserVehiclesForTaskUpdateAsync(appUser.Id, request.taskId, request.searchTerm);

            var vehicleDtos = vehicles.Select(vehicle => new VehicleGetDto
            {
                Id = vehicle.Id,
                Brand = vehicle.Brand,
                Model = vehicle.Model,
                Name = vehicle.Name,
                RegistrationNumber = vehicle.RegistrationNumber,
                Mileage = vehicle.Mileage,
                VehicleType = vehicle.VehicleType,
                DateOfProduction = vehicle.DateOfProduction,
                InsuranceOcValidUntil = vehicle.InsuranceOcValidUntil,
                InsuranceOcCost = vehicle.InsuranceOcCost,
                TechnicalInspectionValidUntil = vehicle.TechnicalInspectionValidUntil,
                IsAvailable = vehicle.IsAvailable
            }).ToList();

            return Result<IEnumerable<VehicleGetDto>>.Success(vehicleDtos);
        }
    }
}