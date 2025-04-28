using EMS.APPLICATION.Dtos;
using EMS.APPLICATION.Features.Account.Commands;
using EMS.APPLICATION.Features.Account.Queries;
using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EMS.API.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController(UserManager<AppUserEntity> userManager, ITokenService tokenService, SignInManager<AppUserEntity> signinManager, ISender sender) : ControllerBase
    {

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await userManager.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.Username.ToLower());

            if (user == null) return Unauthorized("Invalid username!");

            var roles = await userManager.GetRolesAsync(user);

            var result = await signinManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded) return Unauthorized("Username not found and/or password incorrect");

            var token = await sender.Send(new CreateTokenCommand(user, roles));

            return Ok(new NewUserDto
            {
                UserName = user.UserName,
                Email = user.Email,
                Token = token,
                Roles = roles
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                var result = await sender.Send(new RegisterUserCommand(registerDto.Username, registerDto.Email, registerDto.Password));

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("GetAllUser")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUserAsync(int pageNumber, int pageSize, string searchTerm = null)
        {
            var result = await sender.Send(new GetAllUserQuery(pageNumber, pageSize, searchTerm));

            return Ok(new
            {
                userGet = result.Items,
                result.TotalItems,
                result.TotalPages,
                result.PageIndex
            });
        }

        [HttpDelete("{appUserId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteEmployeeAsync([FromRoute] string appUserId)
        {
            var result = await sender.Send(new DeleteUserCommand(appUserId));

            return Ok(result);
        }

        [HttpGet("GetNumberOfUsers")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetNumberOfUsersAsync()
        {
            var result = await sender.Send(new GetNumberOfUsersQuery());

            return Ok(result);
        }
    }
}
