﻿using EMS.APPLICATION.Dtos;
using EMS.APPLICATION.Features.Account;
using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

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

            var role = userManager.GetClaimsAsync(user);

            var result = await signinManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded) return Unauthorized("Username not found and/or password incorrect");

            var token = await sender.Send(new CreateTokenCommand(user));

            return Ok(new NewUserDto
            {
                UserName = user.UserName,
                Email = user.Email,
                Token = token
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
    }
}
