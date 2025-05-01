using EMS.INFRASTRUCTURE.Repositories;
using MediatR;

namespace EMS.APPLICATION.Features.Local.Commands
{
    public record DeleteLocalCommand(Guid localId) : IRequest<bool>;

    public class DeleteLocalCommandHandler(ILocalRepository localRepository)
        : IRequestHandler<DeleteLocalCommand, bool>
    {
        public async Task<bool> Handle(DeleteLocalCommand request, CancellationToken cancellationToken)
        {
            return await localRepository.DeleteLocalAsync(request.localId);
        }
    }
}
