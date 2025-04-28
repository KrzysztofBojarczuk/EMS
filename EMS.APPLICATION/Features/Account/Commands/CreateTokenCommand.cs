using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using MediatR;

namespace EMS.APPLICATION.Features.Account.Commands
{
    public record CreateTokenCommand(AppUserEntity User, IList<string> Roles) : IRequest<string>;

    public class CreateTokenCommandHandler(ITokenService tokenService) : IRequestHandler<CreateTokenCommand, string>
    {
        public async Task<string> Handle(CreateTokenCommand request, CancellationToken cancellationToken)
        {
            return tokenService.CreateToken(request.User, request.Roles);
        }
    }
}
