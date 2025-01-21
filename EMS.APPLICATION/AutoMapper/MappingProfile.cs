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
            CreateMap<TaskEntity, TaskGetDto>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.AddressEntity)); 

            CreateMap<EmployeeCreateDto, EmployeeEntity>();
            CreateMap<EmployeeEntity, EmployeeGetDto>();

            CreateMap<TransactionCreateDto, TransactionEntity>();
            CreateMap<TransactionEntity, TransactionGetDto>();

            CreateMap<PlannedExpenseCreateDto, PlannedExpenseEntity>();
            CreateMap<PlannedExpenseEntity, PlannedExpenseGetDto>();

            CreateMap<BudgetCreateDto, BudgetEntity>();
            CreateMap<BudgetEntity, BudgetGetDto>();

            CreateMap<AddressCreateDto, AddressEntity>();
            CreateMap<AddressEntity, AddressGetDto>();
        }
    }
}
