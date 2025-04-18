using AutoMapper;
using EMS.API.Controllers;
using EMS.APPLICATION.Dtos;
using EMS.APPLICATION.Features.Employee.Queries;
using EMS.CORE.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EMS.TESTS.Controller
{
    public class EmployeeControllerTests(ISender sender, UserManager<AppUserEntity> userManager, IMapper mapper)
    {
        
    }
}
