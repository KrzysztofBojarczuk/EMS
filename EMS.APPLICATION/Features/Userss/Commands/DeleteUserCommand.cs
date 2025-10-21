using EMS.CORE.Interfaces;
using MediatR;

namespace EMS.APPLICATION.Features.Userss.Commands
{
    public record DeleteUserCommand(string appUserId) : IRequest<bool>;

    public class DeleteUserCommandHandler(IUserRepository userRepository) : IRequestHandler<DeleteUserCommand, bool>
    {
        public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellation)
        {
            return await userRepository.DeleteUserAsync(request.appUserId);
        }
    }
}