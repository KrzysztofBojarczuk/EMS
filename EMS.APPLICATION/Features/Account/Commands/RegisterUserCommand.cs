using EMS.APPLICATION.Dtos;
using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace EMS.APPLICATION.Features.Account.Commands
{
    public record RegisterUserCommand(string Username, string Email, string Password) : IRequest<NewUserDto>;

    public class RegisterUserCommandHandler(UserManager<AppUserEntity> userManager, ITokenService tokenService) : IRequestHandler<RegisterUserCommand, NewUserDto>
    {
        public async Task<NewUserDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var appUser = new AppUserEntity
            {
                UserName = request.Username,
                Email = request.Email
            };

            var createdUser = await userManager.CreateAsync(appUser, request.Password);

            if (createdUser.Succeeded)
            {
                // Przypisanie roli
                var roleResult = await userManager.AddToRoleAsync(appUser, "User");

                if (!roleResult.Succeeded)
                {
                    throw new Exception("Error assigning role");
                }

                var roles = await userManager.GetRolesAsync(appUser);

                var token = tokenService.CreateToken(appUser, roles);

                return new NewUserDto
                {
                    UserName = appUser.UserName,
                    Email = appUser.Email,
                    Token = token,
                    Roles = roles
                };
            }
            else
            {
                throw new Exception("User creation failed");
            }
        }
    }
}