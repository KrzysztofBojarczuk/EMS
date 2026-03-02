using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using MediatR;

namespace EMS.APPLICATION.Features.Vehicle.Queries
{
    public record GetUserVehiclesForTaskAddQuery(string appUserId, string searchTerm) : IRequest<IEnumerable<VehicleEntity>>;

    public class GetUserVehiclesForTaskAddQueryHandler(IVehicleRepository vehicleRepository) : IRequestHandler<GetUserVehiclesForTaskAddQuery, IEnumerable<VehicleEntity>>
    {
        public async Task<IEnumerable<VehicleEntity>> Handle(GetUserVehiclesForTaskAddQuery request, CancellationToken cancellationToken)
        {
            return await vehicleRepository.GetUserVehiclesForTaskAddAsync(request.appUserId, request.searchTerm);
        }
    }
}