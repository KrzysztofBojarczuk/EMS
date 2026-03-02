using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using MediatR;

namespace EMS.APPLICATION.Features.Vehicle.Queries
{
    public record GetUserVehiclesForTaskUpdateQuery(string appUserId, Guid taskId, string searchTerm) : IRequest<IEnumerable<VehicleEntity>>;

    public class GetUserVehiclesForTaskUpdateQueryHandler(IVehicleRepository vehicleRepository) : IRequestHandler<GetUserVehiclesForTaskUpdateQuery, IEnumerable<VehicleEntity>>
    {
        public async Task<IEnumerable<VehicleEntity>> Handle(GetUserVehiclesForTaskUpdateQuery request, CancellationToken cancellationToken)
        {
            return await vehicleRepository.GetUserVehiclesForTaskUpdateAsync(request.appUserId, request.taskId, request.searchTerm);
        }
    }
}