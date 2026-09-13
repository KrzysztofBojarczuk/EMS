using EMS.APPLICATION.Dtos;
using EMS.CORE.Common;
using EMS.CORE.Entities;
using EMS.CORE.Enums;
using EMS.CORE.Interfaces;
using EMS.INFRASTRUCTURE.Extensions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace EMS.APPLICATION.Features.Vehicle.Queries
{
    public record GetUserVehiclesQuery(string username, int pageNumber, int pageSize, string searchTerm, List<VehicleType> vehicleType, DateTime? dateFrom, DateTime? dateTo, string sortOrder) : IRequest<Result<PaginatedList<VehicleGetDto>>>;

    public class GetUserVehiclesQueryHandler(IVehicleRepository vehicleRepository, UserManager<AppUserEntity> userManager) : IRequestHandler<GetUserVehiclesQuery, Result<PaginatedList<VehicleGetDto>>>
    {
        public async Task<Result<PaginatedList<VehicleGetDto>>> Handle(GetUserVehiclesQuery request, CancellationToken cancellationToken)
        {
            var appUser = await userManager.FindByNameAsync(request.username);

            if (appUser == null)
            {
                return Result<PaginatedList<VehicleGetDto>>.Failure("User not found.");
            }

            var paginatedVehicles = await vehicleRepository.GetUserVehiclesAsync(appUser.Id, request.pageNumber, request.pageSize, request.searchTerm, request.vehicleType, request.dateFrom, request.dateTo, request.sortOrder);

            var vehicleDtos = paginatedVehicles.Items.Select(vehicle => new VehicleGetDto
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

            var result = new PaginatedList<VehicleGetDto>(vehicleDtos, paginatedVehicles.TotalItems, paginatedVehicles.PageIndex, request.pageSize);

            return Result<PaginatedList<VehicleGetDto>>.Success(result);
        }
    }
}