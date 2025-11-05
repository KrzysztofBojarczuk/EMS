using EMS.CORE.Entities;
using EMS.CORE.Enums;
using EMS.CORE.Interfaces;
using EMS.INFRASTRUCTURE.Extensions;
using MediatR;

namespace EMS.APPLICATION.Features.Vehicle.Queries
{
    public record GetUserVehiclesQuery(string appUserId, int pageNumber, int pageSize, List<VehicleType> vehicleType, string searchTerm, DateTime? dateFrom, DateTime? dateTo, string sortOrderDate, string sortOrderMileage) : IRequest<PaginatedList<VehicleEntity>>;

    public class GetUserVehiclesQueryHandler(IVehicleRepository vehicleRepository) : IRequestHandler<GetUserVehiclesQuery, PaginatedList<VehicleEntity>>
    {
        public async Task<PaginatedList<VehicleEntity>> Handle(GetUserVehiclesQuery request, CancellationToken cancellationToken)
        {
           return await vehicleRepository.GetUserVehiclesAsync(request.appUserId, request.pageNumber, request.pageSize, request.vehicleType, request.searchTerm, request.dateFrom, request.dateTo, request.sortOrderDate, request.sortOrderMileage);
        }
    }
}