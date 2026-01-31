using EMS.CORE.Entities;
using EMS.INFRASTRUCTURE.Repositories;
using MediatR;

namespace EMS.APPLICATION.Features.Local.Commands
{
    public record UpdateLocalCommand(Guid localId, string appUserId, LocalEntity Local) : IRequest<LocalEntity>;

    public class UpdateLocalCommandHandler(ILocalRepository localRepository) : IRequestHandler<UpdateLocalCommand, LocalEntity>
    {
        public async Task<LocalEntity> Handle(UpdateLocalCommand request, CancellationToken cancellationToken)
        {
            return await localRepository.UpdateLocalAsync(request.localId, request.appUserId, request.Local);
        }
    }
}