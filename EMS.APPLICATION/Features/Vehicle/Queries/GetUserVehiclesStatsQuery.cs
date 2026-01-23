using EMS.CORE.Interfaces;
using EMS.CORE.Stats;
using MediatR;

namespace EMS.APPLICATION.Features.Vehicle.Queries
{
    public record GetUserVehiclesStatsQuery(string appUserId) : IRequest<UserVehiclesStats>;

    public class GetUserVehiclesStatsQueryHandler(IVehicleRepository vehicleRepository) : IRequestHandler<GetUserVehiclesStatsQuery, UserVehiclesStats>
    {
        public async Task<UserVehiclesStats> Handle(GetUserVehiclesStatsQuery request, CancellationToken cancellationToken)
        {
            return await vehicleRepository.GetUserVehiclesStatsAsync(request.appUserId);
        }
    }
}