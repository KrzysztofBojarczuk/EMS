using AutoMapper;
using EMS.APPLICATION.Dtos;
using EMS.CORE.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.APPLICATION.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<TaskCreateDto, TaskEntity>();

            CreateMap<TaskEntity, TaskGetDto>();

            CreateMap<EmployeeCreateDto, EmployeeEntity>();

            CreateMap<EmployeeEntity, EmployeeGetDto>();
        }
    }
}
