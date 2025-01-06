﻿using EMS.CORE.Entities;
using EMS.CORE.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.APPLICATION.Dtos
{
    public class TransactionCreateDto
    {
        public string Name { get; set; } = null!;
        public DateTimeOffset CreationDate { get; set; }
        public CategoryType Category { get; set; }
        public decimal Amount { get; set; }
    }
}