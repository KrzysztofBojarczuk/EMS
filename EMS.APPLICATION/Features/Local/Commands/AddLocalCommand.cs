using EMS.CORE.Entities;
using EMS.INFRASTRUCTURE.Repositories;
using MediatR;

namespace EMS.APPLICATION.Features.Local.Commands
{
    public record AddLocalCommand(LocalEntity local) : IRequest<LocalEntity>;

    public class AddLocalCommandHandler(ILocalRepository localRepository) : IRequestHandler<AddLocalCommand, LocalEntity>
    {
        public async Task<LocalEntity> Handle(AddLocalCommand request, CancellationToken cancellationToken)
        {
            return await localRepository.AddLocalAsync(request.local);
        }
    }
}