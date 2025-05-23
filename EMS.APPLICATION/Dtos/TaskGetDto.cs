﻿using EMS.CORE.Enums;

namespace EMS.APPLICATION.Dtos
{
    public class TaskGetDto
    {
        public Guid Id { get; set; } //unikalny identyfikator
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public StatusOfTask Status { get; set; }
        public AddressGetDto Address { get; set; } = null!;
        public List<EmployeeListsGetDto> EmployeeLists { get; set; } = null!;
    }
}
