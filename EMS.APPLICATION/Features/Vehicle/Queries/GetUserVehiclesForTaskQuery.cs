using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using MediatR;

namespace EMS.APPLICATION.Features.Vehicle.Queries
{
    public record GetUserVehiclesForTaskQuery(string appUserId, string searchTerm) : IRequest<IEnumerable<VehicleEntity>>;

    public class GetUserVehiclesForTaskQueryHandler(IVehicleRepository vehicleRepository) : IRequestHandler<GetUserVehiclesForTaskQuery, IEnumerable<VehicleEntity>>
    {
        public async Task<IEnumerable<VehicleEntity>> Handle(GetUserVehiclesForTaskQuery request, CancellationToken cancellationToken)
        {
            return await vehicleRepository.GetUserVehiclesForTaskAsync(request.appUserId, request.searchTerm);
        }
    }
}
