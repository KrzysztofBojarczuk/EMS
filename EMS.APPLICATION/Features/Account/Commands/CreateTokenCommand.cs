using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.APPLICATION.Features.Account.Commands
{
    public record CreateTokenCommand(AppUserEntity User) : IRequest<string>;

    public class CreateTokenCommandHandler(ITokenService tokenService) : IRequestHandler<CreateTokenCommand, string>
    {
        public async Task<string> Handle(CreateTokenCommand request, CancellationToken cancellationToken)
        {
            return tokenService.CreateToken(request.User);
        }
    }
}
