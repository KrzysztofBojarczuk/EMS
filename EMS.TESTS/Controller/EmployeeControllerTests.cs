using AutoMapper;
using EMS.API.Controllers;
using EMS.APPLICATION.Dtos;
using EMS.APPLICATION.Features.Task.Commands;
using EMS.APPLICATION.Features.Task.Queries;
using EMS.CORE.Entities;
using EMS.CORE.Enums;
using FakeItEasy;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace EMS.TESTS.Controller
{
    public class EmployeeControllerTests(ISender sender, IMapper mapper)
    {

        [Fact]
        public async Task TaskController_GetTasks_ReturnsOk()
        {
            // Arrange
            var employee = A.Fake<ICollection<EmployeeGetDto>>();
            var employeeList = A.Fake<List<TaskGetDto>>();

            A.CallTo(() => mapper.Map<List<TaskGetDto>>(employeeList)).Returns(employeeList);

            var controll = new EmployeesController(sender, null, mapper);

            var result = controll.GetAllEmployeesAsync();

            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkObjectResult));
        }
    }
}
