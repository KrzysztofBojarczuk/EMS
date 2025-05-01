using EMS.CORE.Entities;
using EMS.INFRASTRUCTURE.Repositories;
using MediatR;

namespace EMS.APPLICATION.Features.Local.Commands
{
    public record AddLocalCommand(LocalEntity local) : IRequest<LocalEntity>;

    public class AddLocalCommandHandler(ILocalRepository localRepository, IPublisher mediator)
        : IRequestHandler<AddLocalCommand, LocalEntity>
    {
        public async Task<LocalEntity> Handle(AddLocalCommand request, CancellationToken cancellationToken)
        {
            var local = await localRepository.AddLocalAsync(request.local);
            return local;
        }
    }
}
